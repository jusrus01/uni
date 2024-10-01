namespace MyCompany.MyClientApp
{
    public class ExitHandler : ICommandHandler
    {
        public CommandHandlerType Type => CommandHandlerType.Exit;
        public string HelpText => "Type 'exit' or 'q' to exit program'";

        private readonly StateManager _manager;

        public ExitHandler(StateManager manager)
        {
            _manager = manager;
        }

        public bool CanHandle(string input)
        {
            input = input?.Trim();
            return input == "exit" || input == "q";
        }

        public bool Process(string input)
        {
            _manager.ChangeState(State.Exit);
            return true;
        }
    }
}
