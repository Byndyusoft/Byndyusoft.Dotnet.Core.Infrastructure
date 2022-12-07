namespace Byndyusoft.Dotnet.Core.Infrastructure.CQRS.Abstractions.Commands
{
    /// <summary>
    /// Commands factory interface
    /// </summary>
    public interface ICommandsFactory
    {
        /// <summary>
        /// Method for asynchronous commands creation
        /// </summary>
        /// <typeparam name="TCommandContext">Command context type</typeparam>
        /// <returns>Command instance</returns>
        ICommand<TCommandContext> CreateCommand<TCommandContext>() where TCommandContext : ICommandContext;
    }
}