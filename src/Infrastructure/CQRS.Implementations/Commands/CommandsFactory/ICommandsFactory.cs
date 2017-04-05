namespace Byndyusoft.Dotnet.Core.Infrastructure.CQRS.Implementations.Commands.CommandsFactory
{
    using Abstractions.Commands;

    /// <summary>
    /// Commands factory interface
    /// </summary>
    public interface ICommandsFactory
    {
        /// <summary>
        /// Method for synchronous commands creation
        /// </summary>
        /// <typeparam name="TCommandContext">Command context type</typeparam>
        /// <returns>Command instance</returns>
        ICommand<TCommandContext> CreateCommand<TCommandContext>() where TCommandContext : ICommandContext;

        /// <summary>
        /// Method for asynchronous commands creation
        /// </summary>
        /// <typeparam name="TCommandContext">Command context type</typeparam>
        /// <returns>Command instance</returns>
        IAsyncCommand<TCommandContext> CreateAsyncCommand<TCommandContext>() where TCommandContext : ICommandContext;
    }
}