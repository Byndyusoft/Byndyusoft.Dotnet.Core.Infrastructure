namespace Byndyusoft.Dotnet.Core.Samples.Jobs.Consumer.Installers
{
    using Autofac;
    using Infrastructure.Dapper.ConnectionsFactory;
    using Infrastructure.Dapper.SessionsFactory;
    using JetBrains.Annotations;

    [UsedImplicitly]
    public class DataAccessInstaller : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SqlConnectionsFactory>().As<IDbConnectionsFactory>().SingleInstance();
            builder.RegisterType<SessionsFactory>().As<ISessionsFactory>().SingleInstance();
        }
    }
}