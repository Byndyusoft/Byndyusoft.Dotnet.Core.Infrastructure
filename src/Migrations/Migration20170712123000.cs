namespace Byndyusoft.Dotnet.Core.Samples.Migrations
{
    using FluentMigrator;
    using JetBrains.Annotations;

    [UsedImplicitly]
    [Migration(20170808183900)]
    public class Migration20170808183900 : ForwardOnlyMigration
    {
        public override void Up()
        {
            Execute.Sql("select 1");
        }
    }
}
