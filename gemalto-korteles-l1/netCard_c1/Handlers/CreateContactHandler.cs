using System;

namespace MyCompany.MyClientApp
{
    public class CreateContactHandler : ICommandHandler
    {
        private const int CreateArgsCount = 4;
        private const char CreateArgsSeparator = ' ';

        private readonly ContactManagerProxy _contactManagerService;

        public CommandHandlerType Type => CommandHandlerType.CreateContact;
        public string HelpText => "To create a new contact use 'add <firstName> <lastName> <phoneNumber>' command";

        public CreateContactHandler(ContactManagerProxy contactManagerService)
        {
            _contactManagerService = contactManagerService;
        }

        public bool CanHandle(string input) 
        {
            input = input?.ToLowerInvariant()?.Trim();
            if (input == null)
            {
                return false;
            }

            var splits = input.Split(new char[] { CreateArgsSeparator }, StringSplitOptions.RemoveEmptyEntries);
            return splits.Length == CreateArgsCount && splits[0] == "add";
        }

        public bool Process(string input)
        {
            if (input == null)
            {
                return false;
            }

            input = input?.ToLowerInvariant()?.Trim();
            
            var splits = input?.Split(new char[] { CreateArgsSeparator }, StringSplitOptions.RemoveEmptyEntries);
            if ((splits?.Length ?? 0) < CreateArgsCount)
            {
                return false;
            }

            return _contactManagerService.CreateContact($"{splits[1]} {splits[2]}", splits[3]);
        }
    }
}
