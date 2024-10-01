namespace MyCompany.MyClientApp
{
    public class UpdateContact : ICommandHandler
    {
        public CommandHandlerType Type => CommandHandlerType.UpdateContact;
        public string HelpText => "To update contact use 'update <firstName> <lastName> [<newFirstName>|-] [<newLastName>|-] [<newNumber>|-]' command";

        private const int UpdateArgsCount = 6;
        private const string UpdateArgsSeparator = " ";
        private readonly ContactManagerProxy _contactManagerService;

        public UpdateContact(ContactManagerProxy contactManagerService)
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

            var splits = input.Split(new string[] { " " }, System.StringSplitOptions.RemoveEmptyEntries);
            return splits.Length == UpdateArgsCount && splits[0] == "update";
        }

        public bool Process(string input)
        {
            if (input == null)
            {
                return false;
            }

            input = input.Trim();

            var splits = input.Split(new string[] { UpdateArgsSeparator }, System.StringSplitOptions.RemoveEmptyEntries);
            if (splits.Length != UpdateArgsCount)
            {
                return false;
            }
            
            var firstName = splits[1];
            var lastName = splits[2];
            var newFirstName = splits[3];
            var newLastName = splits[4];
            var newNumber = splits[5];

            if (newFirstName == "-")
            {
                newFirstName = firstName;
            }

            if (newLastName == "-")
            {
                newLastName = lastName;
            }

            if (newNumber == "-")
            {
                newNumber = null;
            }

            return _contactManagerService.UpdateContact($"{firstName} {lastName}", $"{newFirstName} {newLastName}", newNumber);
        }
    }
}
