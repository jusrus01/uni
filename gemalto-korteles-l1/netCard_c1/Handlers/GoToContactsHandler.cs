namespace MyCompany.MyClientApp
{
    public class GoToContactsHandler : ICommandHandler
    {
        public CommandHandlerType Type => CommandHandlerType.GoToContacts;
        public string HelpText => "To open contacts page use 'contacts' command";

        private readonly StateManager _manager;

        public GoToContactsHandler(StateManager manager)
        {
            _manager = manager;
        }

        public bool CanHandle(string input) => input?.Trim() == "contacts";

        public bool Process(string input)
        {
            _manager.ChangeState(State.ContactsPage);
            return true;
        }
    }
}
