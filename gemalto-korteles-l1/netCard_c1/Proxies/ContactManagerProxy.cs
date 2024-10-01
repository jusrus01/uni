using MyCompany.MyOnCardApp;

namespace MyCompany.MyClientApp
{
    public class ContactManagerProxy
    {
        private readonly ContactManagerService _contactManagerService;

        public ContactManagerProxy(ContactManagerService contactManagerService)
        {
            _contactManagerService = contactManagerService;
        }

        public bool UpdateContact(string name, string newName, string number)
        {
            return _contactManagerService.UpdateContact(name, newName, number);
        }

        public bool RemoveContact(string name)
        {
            return _contactManagerService.RemoveContact(name);
        }

        public bool CreateContact(string name, string number)
        {
            return _contactManagerService.CreateContact(name, number);
        }

        public string[] ReadSavedContacts(int from)
        {
            return _contactManagerService.ReadSavedContacts(from);
        }
    }
}