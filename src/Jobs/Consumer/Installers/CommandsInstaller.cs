using IntrospectionExtensions = System.Reflection.IntrospectionExtensions;

namespace Byndyusoft.Dotnet.Core.Samples.Jobs.Consumer.Installers
{
    using System.Linq;
    using Autofac;
    using Infrastructure.CQRS.Abstractions.Commands;
    using Infrastructure.CQRS.Implementations.Commands;
    using Infrastructure.CQRS.Implementations.Commands.CommandsFactory;
    using JetBrains.Annotations;
    using TypeExtensions = System.Reflection.TypeExtensions;
    using Web.DataAccess.Db.Commands;

    [UsedImplicitly]
    public class CommandsInstaller : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AutofacCommandsFactory>().As<ICommandsFactory>().SingleInstance();
            builder.RegisterType<CommandsDispatcher>().As<ICommandsDispatcher>().SingleInstance();

            var commandType = typeof(ICommand<>);
            var asyncCommandType = typeof(IAsyncCommand<>);
            var dataAccess = IntrospectionExtensions.GetTypeInfo(typeof(BaseDbCommand)).Assembly;
            builder.RegisterAssemblyTypes(dataAccess)
                .Where(x => TypeExtensions.GetInterfaces(x)
                                .SingleOrDefault(i => TypeExtensions.GetGenericArguments(i).Length > 0 && (i.GetGenericTypeDefinition() == commandType || i.GetGenericTypeDefinition() == asyncCommandType)) != null)
                .AsImplementedInterfaces()
                .SingleInstance();
        }
    }
}