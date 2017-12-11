namespace Byndyusoft.Dotnet.Core.Samples.Web.Application.Infrastructure.Installers
{
    using Autofac;
    using Core.Infrastructure.Dapper.ConnectionsFactory;
    using Core.Infrastructure.Dapper.SessionsFactory;
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