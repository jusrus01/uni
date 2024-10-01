using System;
using System.Collections.Generic;
using System.Linq;

namespace MyCompany.MyClientApp
{
    public enum State
    {
        StartPage,

        ContactsPage,

        Exit
    }

    public class StateManager
    {
        private State _currentState = State.StartPage;

        private readonly Dictionary<State, CommandHandlerType[]> _allowedStates = new Dictionary<State, CommandHandlerType[]>
        {
            {
                State.StartPage,
                new CommandHandlerType[] 
                {
                    CommandHandlerType.GoToContacts,

                    CommandHandlerType.Clear,
                    CommandHandlerType.Help,
                    CommandHandlerType.Exit
                }
            },
            {
                State.ContactsPage,
                new CommandHandlerType[]
                {
                    CommandHandlerType.GoToStart,

                    CommandHandlerType.CreateContact,
                    CommandHandlerType.ViewContacts,
                    CommandHandlerType.RemoveContact,
                    CommandHandlerType.UpdateContact,

                    CommandHandlerType.Clear,
                    CommandHandlerType.Help,
                    CommandHandlerType.Exit
                }
            }
        };

        public bool IsAllowed(CommandHandlerType type)
        {
            if (_allowedStates.TryGetValue(_currentState, out var validHandlers))
            {
                return validHandlers.Contains(type);
            }

            return false;
        }

        public void ChangeState(State state)
        {
            _currentState = state;
        }

        public State GetState()
        {
            return _currentState;
        }

        public List<CommandHandlerType> GetAllowedCurrentStateHandlers()
        {
            if (_allowedStates.TryGetValue(_currentState, out var states))
            {
                return states?.ToList();
            }

            return new List<CommandHandlerType>();
        }
    }

    public class Controller
    {
        private const string WelcomeText = "Welcome to SIM simulation demo.";

        private readonly List<ICommandHandler> _handlers;
        private readonly StateManager _stateManager;
        private readonly ContactManagerProxy _contactManagerService;

        public Controller(ContactManagerProxy contactManagerService)
        {
            _contactManagerService = contactManagerService;

            _stateManager = new StateManager();

            _handlers = new List<ICommandHandler>
            {
                new GoToContactsHandler(_stateManager),
                new GoToStartHandler(_stateManager),
                new CreateContactHandler(_contactManagerService),
                new ViewContactsHandler(_contactManagerService),
                new RemoveContactHandler(_contactManagerService),
                new UpdateContact(_contactManagerService),

                new ExitHandler(_stateManager),

                new ClearHandler()
            };

            var help = new HelpHandler(_stateManager, _handlers);
            _handlers.Add(help);
        }

        public void Run()
        {
            Console.WriteLine(WelcomeText);
            ShowCommandsHelpText();

            while (_stateManager.GetState() != State.Exit)
            {
                Console.Write("Enter command: ");
                var input = Console.ReadLine();

                Process(input);
            }

            Console.WriteLine("Exiting...");
        }

        private void ShowCommandsHelpText()
        {
            var help = _handlers?.FirstOrDefault(handler => handler.Type == CommandHandlerType.Help);
            help?.Process(null);
        }

        private void Process(string input)
        {
            var allowedHandlers = _stateManager.GetAllowedCurrentStateHandlers() ?? new List<CommandHandlerType>();
            var handler = _handlers?.FirstOrDefault(i => i.CanHandle(input) && allowedHandlers.Contains(i.Type));
            if (handler == null)
            {
                Console.WriteLine("Could not find command. Try again...");
                return;
            }

            if (!_stateManager.IsAllowed(handler.Type))
            {
                Console.WriteLine("Try again... This command is not available. Type 'help' to show the list of commands.");
                return;
            }

            if (!handler.Process(input))
            {
                Console.WriteLine("Something went wrong while processing command...");
            }
        }
    }
}