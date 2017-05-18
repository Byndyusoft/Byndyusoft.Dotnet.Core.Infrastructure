namespace Byndyusoft.Dotnet.Core.Samples.Web.Application.Infrastructure.Installers
{
    using System.Linq;
    using System.Reflection;
    using Autofac;
    using Core.Infrastructure.CQRS.Abstractions.Queries;
    using Core.Infrastructure.CQRS.Implementations.Queries;
    using Core.Infrastructure.CQRS.Implementations.Queries.QueriesFactory;
    using DataAccess.Values.Queries;
    using JetBrains.Annotations;
    using Module = Autofac.Module;

    [UsedImplicitly]
    public class QueriesInstaller : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AutofacQueriesFactory>().As<IQueriesFactory>().SingleInstance();
            builder.RegisterType<QueriesDispatcher>().As<IQueriesDispatcher>().SingleInstance();

            var queryType = typeof(IQuery<,>);
            var dataAccess = typeof(GetValueQuery).GetTypeInfo().Assembly;
            builder.RegisterAssemblyTypes(dataAccess)
                .Where(x => x.GetInterfaces()
                                .SingleOrDefault(i => i.GetGenericArguments().Length > 0 && i.GetGenericTypeDefinition() == queryType) != null)
                .AsImplementedInterfaces()
                .SingleInstance();
        }
    }
}