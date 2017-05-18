namespace Byndyusoft.Dotnet.Core.Samples.Web.Application.Infrastructure
{
    public class CommandLineArgumentsProvider
    {
        public CommandLineArgumentsProvider(string[] args)
        {
            Arguments = args;
        }

        public string[] Arguments { get; }
    }
}