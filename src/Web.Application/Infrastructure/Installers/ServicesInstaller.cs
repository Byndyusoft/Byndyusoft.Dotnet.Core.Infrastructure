namespace Byndyusoft.Dotnet.Core.Samples.Web.Application.Infrastructure.Installers
{
    using Autofac;
    using DataAccess.Values.Repository;
    using JetBrains.Annotations;

    [UsedImplicitly]
    public class ServicesInstaller : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<InMemoryRepository>().As<IValuesRepository>().SingleInstance();
        }
    }
}