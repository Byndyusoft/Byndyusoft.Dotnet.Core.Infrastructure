// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    using Byndyusoft.Dotnet.Core.Infrastructure.CQRS.Abstractions.Commands;
    using Byndyusoft.Dotnet.Core.Infrastructure.CQRS.Abstractions.Queries;
    using Byndyusoft.Dotnet.Core.Infrastructure.CQRS.Implementations.Commands;
    using Byndyusoft.Dotnet.Core.Infrastructure.CQRS.Implementations.Queries;
    using Extensions;

    public static class ServiceCollectionCqrsExtensions
    {
        public static IServiceCollection AddCqrs(this IServiceCollection services)
        {
            services.TryAddSingleton<IQueriesFactory, QueriesFactory>();
            services.TryAddSingleton<IQueriesDispatcher, QueriesDispatcher>();

            services.TryAddSingleton<ICommandsFactory, CommandsFactory>();
            services.TryAddSingleton<ICommandsDispatcher, ICommandsDispatcher>();

            return services;
        }
    }
}