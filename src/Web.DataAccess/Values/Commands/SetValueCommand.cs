namespace Byndyusoft.Dotnet.Core.Samples.Web.DataAccess.Values.Commands
{
    using System;
    using Domain.CommandsContexts.Values;
    using Infrastructure.CQRS.Abstractions.Commands;
    using JetBrains.Annotations;
    using Repository;

    [UsedImplicitly]
    public class SetValueCommand : ICommand<SetValueCommandContext>
    {
        private readonly IValuesRepository _repository;

        public SetValueCommand(IValuesRepository repository)
        {
            if(repository == null)
                throw new ArgumentNullException();

            _repository = repository;
        }

        public void Execute(SetValueCommandContext commandContext)
        {
            _repository.Set(commandContext.Id, commandContext.Value);
        }
    }
}