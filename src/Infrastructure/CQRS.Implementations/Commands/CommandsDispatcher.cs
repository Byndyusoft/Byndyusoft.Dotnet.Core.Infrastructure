namespace Byndyusoft.Dotnet.Core.Infrastructure.CQRS.Implementations.Commands
{
    using System;
    using System.Threading.Tasks;
    using Abstractions.Commands;
    using CommandsFactory;

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
            if (commandsFactory == null)
                throw new ArgumentNullException(nameof(commandsFactory));

            _commandsFactory = commandsFactory;
        }

        /// <summary>
        /// Method for synchronous commands execution
        /// </summary>
        /// <typeparam name="TCommandContext">Command context type</typeparam>
        /// <param name="commandContext">Information needed for commands execution</param>
        public void Execute<TCommandContext>(TCommandContext commandContext) where TCommandContext : ICommandContext
        {
            _commandsFactory.CreateCommand<TCommandContext>().Execute(commandContext);
        }

        /// <summary>
        /// Method for asynchronous commands execution
        /// </summary>
        /// <typeparam name="TCommandContext">Command context type</typeparam>
        /// <param name="commandContext">Information needed for commands execution</param>
        public Task ExecuteAsync<TCommandContext>(TCommandContext commandContext) where TCommandContext : ICommandContext
        {
            return _commandsFactory.CreateAsyncCommand<TCommandContext>().Execute(commandContext);
        }
    }
}