namespace Byndyusoft.Dotnet.Core.Infrastructure.CQRS.Implementations.Commands.CommandsFactory
{
    using System;
    using Abstractions.Commands;
    using Autofac;

    /// <summary>
    /// Commands factory implementation for Autofac
    /// </summary>
    public class AutofacCommandsFactory : ICommandsFactory
    {
        private readonly IComponentContext _componentContext;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="componentContext">Autofac components context</param>
        public AutofacCommandsFactory(IComponentContext componentContext)
        {
            if (componentContext == null)
                throw new ArgumentNullException(nameof(componentContext));

            _componentContext = componentContext;
        }

        /// <summary>
        /// Method for synchronous commands creation
        /// </summary>
        /// <typeparam name="TCommandContext">Command context type</typeparam>
        /// <returns>Command instance</returns>
        public ICommand<TCommandContext> CreateCommand<TCommandContext>() where TCommandContext : ICommandContext
        {
            return _componentContext.Resolve<ICommand<TCommandContext>>();
        }

        /// <summary>
        /// Method for asynchronous commands creation
        /// </summary>
        /// <typeparam name="TCommandContext">Command context type</typeparam>
        /// <returns>Command instance</returns>
        public IAsyncCommand<TCommandContext> CreateAsyncCommand<TCommandContext>() where TCommandContext : ICommandContext
        {
            return _componentContext.Resolve<IAsyncCommand<TCommandContext>>();
        }
    }
}