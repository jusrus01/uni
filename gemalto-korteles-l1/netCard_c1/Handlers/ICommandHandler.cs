namespace MyCompany.MyClientApp
{
    public interface ICommandHandler
    {
        CommandHandlerType Type { get; }
        string HelpText { get; }

        bool CanHandle(string input);
        bool Process(string input);
    }
}