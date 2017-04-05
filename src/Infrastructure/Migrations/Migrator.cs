namespace Byndyusoft.Dotnet.Core.Infrastructure.Migrations
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Dapper.SessionsFactory;

	public class Migrator
	{
		private readonly ISessionsFactory _sessionsFactory;
		private readonly IMigration[] _migrations;

		public Migrator(ISessionsFactory sessionsFactory, IMigration[] migrations)
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
if object_id ('[dbo].[VersionInfo]') is null
	begin
		create table [dbo].[VersionInfo](
			[Version] [bigint] NOT NULL,
			[AppliedOn] [datetime] NOT NULL CONSTRAINT [DF_VersionInfo_AppliedOn] DEFAULT (getutcdate())
		) on [PRIMARY]		

		create unique clustered index [UC_Version] ON [dbo].[VersionInfo]
		(
			[Version] asc
		)with (pad_index = off, statistics_norecompute = off, sort_in_tempdb = off, ignore_dup_key = off, drop_existing = off, online = off, allow_row_locks = on, allow_page_locks = on)
	end");
		}

		private static IEnumerable<long> GetAppliedVersions(ISession session)
		{
			return session.Query<long>("select [Version] from VersionInfo").ToArray();
		}

		private static void AddAppliedVersion(ISession session, long version)
		{
			session.Execute(@"
INSERT INTO [dbo].[VersionInfo] ([Version])
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

			var newMigrations = _migrations.Where(x => appliedVersions.Contains(x.Version) == false).ToArray();
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