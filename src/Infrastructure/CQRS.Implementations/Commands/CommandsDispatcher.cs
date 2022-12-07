namespace Byndyusoft.Dotnet.Core.Infrastructure.CQRS.Implementations.Commands
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Abstractions.Commands;

    /// <summary>
    /// Commands dispatcher standard implementation
    /// </summary>
    public class CommandsDispatcher : ICommandsDispatcher
    {
        private readonly ICommandsFactory _commandsFactory;

        /// <summary>
        /// Constructor for commands dispatcher
        /// </summary>
        /// <param name="commandsFactory">Commands factory</param>
        public CommandsDispatcher(ICommandsFactory commandsFactory)
        {
            _commandsFactory = commandsFactory ?? throw new ArgumentNullException(nameof(commandsFactory));
        }
        
        /// <summary>
        /// Method for asynchronous commands execution
        /// </summary>
        /// <typeparam name="TCommandContext">Command context type</typeparam>
        /// <param name="commandContext">Information needed for commands execution</param>
        public Task ExecuteAsync<TCommandContext>(TCommandContext commandContext) 
            where TCommandContext : ICommandContext
        {
            return ExecuteAsync(commandContext, CancellationToken.None);
        }

        /// <summary>
        /// Method for asynchronous commands execution
        /// </summary>
        /// <typeparam name="TCommandContext">Command context type</typeparam>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <param name="commandContext">Information needed for commands execution</param>
        public Task ExecuteAsync<TCommandContext>(TCommandContext commandContext, CancellationToken cancellationToken) 
            where TCommandContext : ICommandContext
        {
            return _commandsFactory.CreateCommand<TCommandContext>().Execute(commandContext, cancellationToken);
        }
    }
}