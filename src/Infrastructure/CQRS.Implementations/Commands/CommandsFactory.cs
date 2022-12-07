namespace Byndyusoft.Dotnet.Core.Infrastructure.CQRS.Implementations.Commands
{
    using System;
    using Byndyusoft.Dotnet.Core.Infrastructure.CQRS.Abstractions.Commands;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    ///     Default queries factory
    /// </summary>
    public class CommandsFactory : ICommandsFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public CommandsFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public ICommand<TCommandContext> CreateCommand<TCommandContext>()
            where TCommandContext : ICommandContext
        {
            return _serviceProvider.GetRequiredService<ICommand<TCommandContext>>();
        }
    }
}