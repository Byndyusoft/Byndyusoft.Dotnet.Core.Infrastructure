namespace Byndyusoft.Dotnet.Core.Samples.Jobs.Consumer.Installers
{
    using System.Data.SqlClient;
    using Autofac;
    using Infrastructure.Dapper.ConnectionsFactory;
    using Infrastructure.Dapper.SessionsFactory;
    using JetBrains.Annotations;

    [UsedImplicitly]
    public class DataAccessInstaller : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SqlConnectionsFactory<SqlConnection>>().As<IDbConnectionsFactory>().SingleInstance();
            builder.RegisterType<SessionsFactory>().As<ISessionsFactory>().SingleInstance();
        }
    }
}