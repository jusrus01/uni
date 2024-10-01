namespace MyCompany.MyClientApp
{
    public class RemoveContactHandler : ICommandHandler
    {
        public CommandHandlerType Type => CommandHandlerType.RemoveContact;
        public string HelpText => "To remove contact use 'remove <firstName> <lastName>' command";

        private const int RemoveArgsCount = 3;
        private const string RemoveArgsSeparator = " ";

        private readonly ContactManagerProxy _contactManagerService;

        public RemoveContactHandler(ContactManagerProxy contactManagerService)
        {
            _contactManagerService = contactManagerService;
        }

        public bool CanHandle(string input)
        {
            if (input == null)
            {
                return false;
            }

            input = input.Trim();
            var splits = input.Split(new string[] { RemoveArgsSeparator }, System.StringSplitOptions.RemoveEmptyEntries);
            return splits.Length == RemoveArgsCount && splits[0] == "remove";
        }

        public bool Process(string input)
        {
            var splits = input.Split(new string[] { RemoveArgsSeparator }, System.StringSplitOptions.RemoveEmptyEntries);
            if (splits.Length != RemoveArgsCount)
            {
                return false;
            }

            return _contactManagerService.RemoveContact($"{splits[1]} {splits[2]}");
        }
    }
}
