namespace Byndyusoft.Dotnet.Core.Infrastructure.CQRS.Abstractions.Commands
{
    using System.Threading.Tasks;

    /// <summary>
    /// Interface for asynchronous commands
    /// </summary>
    /// <typeparam name="TCommandContext">Command context type</typeparam>
    public interface IAsyncCommand<in TCommandContext> where TCommandContext : ICommandContext
    {
        /// <summary>
        /// Method for command execution
        /// </summary>
        /// <param name="commandContext">Information needed for command execution</param>
        Task Execute(TCommandContext commandContext);
    }
}