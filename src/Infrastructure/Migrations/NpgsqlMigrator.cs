namespace Byndyusoft.Dotnet.Core.Infrastructure.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Dapper.SessionsFactory;

    public class NpgsqlMigrator
	{
		private readonly ISessionsFactory _sessionsFactory;
		private readonly IMigration[] _migrations;

		public NpgsqlMigrator(ISessionsFactory sessionsFactory, IMigration[] migrations)
		{
			if(sessionsFactory == null)
				throw new ArgumentNullException(nameof(sessionsFactory));
			if (migrations == null)
				throw new ArgumentNullException(nameof(migrations));

			_sessionsFactory = sessionsFactory;
			_migrations = migrations;
		}



		private static void CheckAndCreateVersionTable(ISession session)
		{
			session.Execute(@"
DO $$
BEGIN
IF NOT EXISTS (SELECT 1 FROM pg_class WHERE relname = 'VersionInfo')
THEN
CREATE TABLE public.""VersionInfo""
		    (
		        ""Version"" bigint NOT NULL,
		    ""AppliedOn"" timestamp without time zone,
		    ""Description"" character varying(1024)
		        )
		    WITH(
		        OIDS = FALSE
		    );
		
		    CREATE UNIQUE INDEX ""UC_Version""
		    ON public.""VersionInfo""
		    USING btree
		    (""Version"");
        END IF;
END; $$ 
");
		}

		private static IEnumerable<long> GetAppliedVersions(ISession session)
		{
			return session.Query<long>("select \"Version\" from \"VersionInfo\"").ToArray();
		}

		private static void AddAppliedVersion(ISession session, long version)
		{
			session.Execute(@"
INSERT INTO ""VersionInfo"" (""Version"")
VALUES (@Version)",
				new {Version = version});
		}

		public void Migrate()
		{
			HashSet<long> appliedVersions;
			using (var session = _sessionsFactory.Create())
			{
				CheckAndCreateVersionTable(session);
				appliedVersions = new HashSet<long>(GetAppliedVersions(session));

				session.Commit();
			}

			var newMigrations = _migrations.Where(x => appliedVersions.Contains(x.Version) == false).OrderBy(k => k.Version).ToArray();
			foreach (var newMigration in newMigrations)
				using (var session = _sessionsFactory.Create())
				{
					var commands = newMigration.SqlSource.Split(new[] { "\nGO\r\n" }, StringSplitOptions.RemoveEmptyEntries);
					foreach (var command in commands)
						session.Execute(command, commandTimeout: 0);

					AddAppliedVersion(session, newMigration.Version);

					session.Commit();
				}
		}
	}
}