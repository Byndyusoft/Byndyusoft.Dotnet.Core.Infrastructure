namespace Byndyusoft.Dotnet.Core.Samples.Web.Application.Infrastructure.Installers
{
    using System.Linq;
    using System.Reflection;
    using Autofac;
    using Core.Infrastructure.CQRS.Abstractions.Commands;
    using Core.Infrastructure.CQRS.Implementations.Commands;
    using Core.Infrastructure.CQRS.Implementations.Commands.CommandsFactory;
    using DataAccess.Values.Commands;
    using JetBrains.Annotations;
    using Module = Autofac.Module;

    [UsedImplicitly]
    public class CommandsInstaller : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AutofacCommandsFactory>().As<ICommandsFactory>().SingleInstance();
            builder.RegisterType<CommandsDispatcher>().As<ICommandsDispatcher>().SingleInstance();

            var commandType = typeof(ICommand<>);
            var asyncCommandType = typeof(IAsyncCommand<>);
            var dataAccess = typeof(SetValueCommand).GetTypeInfo().Assembly;
            builder.RegisterAssemblyTypes(dataAccess)
                .Where(x => x.GetInterfaces()
                                .SingleOrDefault(i => i.GetGenericArguments().Length > 0 && (i.GetGenericTypeDefinition() == commandType || i.GetGenericTypeDefinition() == asyncCommandType)) != null)
                .AsImplementedInterfaces()
                .SingleInstance();
        }
    }
}