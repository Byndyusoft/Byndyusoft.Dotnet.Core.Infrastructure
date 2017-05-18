namespace Byndyusoft.Dotnet.Core.Samples.Web.Domain.CommandsContexts.Values
{
    using Infrastructure.CQRS.Abstractions.Commands;

    public class SetValueCommandContext : ICommandContext
    {
        public int Id { get; }
        public string Value { get; }

        public SetValueCommandContext(int id, string value)
        {
            Id = id;
            Value = value;
        }
    }
}