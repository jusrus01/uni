using System;
using System.Collections.Generic;

namespace MyCompany.MyClientApp
{
    public class ViewContactsHandler : SimpleCommandBase
    {
        public override string Command => "view";
        public override CommandHandlerType Type => CommandHandlerType.ViewContacts;
        public override string HelpTextWithoutCommand => "see view all saved contacts";

     
        private readonly ContactManagerProxy _contactManagerService;

        public ViewContactsHandler(ContactManagerProxy contactManagerService)
        {
            _contactManagerService = contactManagerService;
        }

        public override bool Process(string input)
        {
            var readContacts = new List<string>();
            int from = 0;

            do
            {
                var contactsReponse = _contactManagerService.ReadSavedContacts(from);
                if (contactsReponse == null)
                {
                    break;
                }

                foreach (var resp in contactsReponse)
                {
                    if (int.TryParse(resp, out from))
                    {
                        break;
                    }

                    readContacts.Add(resp);
                }
            } while (from != 0);

            Console.WriteLine("------------ CONTACTS ------------");

            if (readContacts.Count == 0)
            {
                Console.WriteLine("No contacts found.");
            }
            else
            {
                for (int i = 0; i < readContacts.Count; i++)
                {
                    var splits = readContacts[i]?.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);

                    Console.WriteLine($"[ {i} ]: Name '{splits[0]}', Phone number '{splits[1]}'");
                }
            }

            Console.WriteLine("------------ END ------------");

            return true;
        }
    }
}