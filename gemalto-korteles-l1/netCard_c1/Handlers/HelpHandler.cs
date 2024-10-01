using System.Collections.Generic;
using System;
using System.Linq;

namespace MyCompany.MyClientApp
{
    public class HelpHandler : SimpleCommandBase
    {
        private readonly StateManager _manager;
        private readonly List<ICommandHandler> _handlers;

        public override string Command => "help";
        public override CommandHandlerType Type => CommandHandlerType.Help;
        public override string HelpTextWithoutCommand => "view available commands";

        public HelpHandler(StateManager manager, List<ICommandHandler> handlers)
        {
            _manager = manager;
            _handlers = handlers;
        }

        public override bool Process(string input)
        {
            var allowedHandlers = _manager.GetAllowedCurrentStateHandlers() ?? new List<CommandHandlerType>();

            Console.WriteLine("--------------------------------------------------------");

            if (allowedHandlers.Any())
            {
                Console.WriteLine("Overview of all available commands:");
            }

            foreach (var handler in allowedHandlers)
            {
                ShowHelpTextForHandler(handler);
            }

            Console.WriteLine("--------------------------------------------------------");
            return true;
        }

        private void ShowHelpTextForHandler(CommandHandlerType handlerType)
        {
            var handler = _handlers?.FirstOrDefault(i => i.Type == handlerType);
            if (handler != null)
            {
                Console.WriteLine($"- {handler.HelpText}");
            }
        }
    }
}
