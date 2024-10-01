namespace MyCompany.MyClientApp
{
    public class GoToStartHandler : ICommandHandler
    {
        public CommandHandlerType Type => CommandHandlerType.GoToStart;
        public string HelpText => "To go back to the start page use 'back' command";

        private readonly StateManager _manager;

        public GoToStartHandler(StateManager manager)
        {
            _manager = manager;
        }

        public bool CanHandle(string input) => input?.Trim() == "back";

        public bool Process(string input)
        {
            _manager.ChangeState(State.StartPage);
            return true;
        }
    }
}