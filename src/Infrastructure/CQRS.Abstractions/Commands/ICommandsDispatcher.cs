namespace Byndyusoft.Dotnet.Core.Infrastructure.CQRS.Abstractions.Commands
{
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Commands dispatcher interface
    /// </summary>
    public interface ICommandsDispatcher
    {
        /// <summary>
        /// Method for asynchronous commands execution
        /// </summary>
        /// <typeparam name="TCommandContext">Command context type</typeparam>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <param name="commandContext">Information needed for commands execution</param>
        Task ExecuteAsync<TCommandContext>(TCommandContext commandContext, CancellationToken cancellationToken) 
            where TCommandContext : ICommandContext;

        /// <summary>
        /// Method for asynchronous commands execution
        /// </summary>
        /// <typeparam name="TCommandContext">Command context type</typeparam>
        /// <param name="commandContext">Information needed for commands execution</param>
        Task ExecuteAsync<TCommandContext>(TCommandContext commandContext)
            where TCommandContext : ICommandContext;
    }
}