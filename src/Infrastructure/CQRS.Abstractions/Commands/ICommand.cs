namespace Byndyusoft.Dotnet.Core.Infrastructure.CQRS.Abstractions.Commands
{
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Interface for asynchronous commands
    /// </summary>
    /// <typeparam name="TCommandContext">Command context type</typeparam>
    public interface ICommand<in TCommandContext> where TCommandContext : ICommandContext
    {
        /// <summary>
        /// Method for command execution
        /// </summary>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <param name="commandContext">Information needed for command execution</param>
        Task Execute(TCommandContext commandContext, CancellationToken cancellationToken);
    }
}