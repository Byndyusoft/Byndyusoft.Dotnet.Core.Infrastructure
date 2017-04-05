namespace Byndyusoft.Dotnet.Core.Infrastructure.CQRS.Abstractions.Commands
{
    /// <summary>
    /// Interface for synchronous commands
    /// </summary>
    /// <typeparam name="TCommandContext">Command context type</typeparam>
    public interface ICommand<in TCommandContext> where TCommandContext : ICommandContext
    {
        /// <summary>
        /// Method for command execution
        /// </summary>
        /// <param name="commandContext">Information needed for command execution</param>
        void Execute(TCommandContext commandContext);
    }
}