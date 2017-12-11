namespace Byndyusoft.Dotnet.Core.Samples.Jobs.Consumer.Installers
{
    using System.Reflection;
    using Autofac;
    using JetBrains.Annotations;
    using Workers;
    using Module = Autofac.Module;

    [UsedImplicitly]
    public class ServicesInstaller : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var workers = typeof(RabbitWorkerBase).GetTypeInfo().Assembly;

            builder.RegisterAssemblyTypes(workers)
                .AsSelf();
        }
    }
}