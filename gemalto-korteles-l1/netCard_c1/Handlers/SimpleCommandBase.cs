namespace MyCompany.MyClientApp
{
    public abstract class SimpleCommandBase : ICommandHandler
    {
        protected SimpleCommandBase() { }

        public abstract string Command { get; }

        public abstract CommandHandlerType Type { get; }

        public abstract string HelpTextWithoutCommand { get; }
        public string HelpText => $"Type '{Command}' to {HelpTextWithoutCommand}";

        public bool CanHandle(string input) => Command?.Trim() == input;

        public abstract bool Process(string input);
    }
}
