using MyCompany.MyOnCardApp;

namespace MyCompany.MyClientApp
{
    public class ContactManagerProxy
    {
        private readonly ContactManagerService2 _proxy;
        private readonly ContactManagerService _contactManagerService;

        public ContactManagerProxy(ContactManagerService2 proxy)
        {
            _proxy = proxy;
        }

        public ContactManagerProxy(ContactManagerService contactManagerService)
        {
            _contactManagerService = contactManagerService;
        }

        public bool UpdateContact(string name, string newName, string number)
        {
            if (_proxy != null)
            {
                return _proxy.UpdateContact(name, newName, number);
            }

            return _contactManagerService.UpdateContact(name, newName, number);
        }

        public bool RemoveContact(string name)
        {
            if (_proxy != null)
            {
                return _proxy.RemoveContact(name);
            }

            return _contactManagerService.RemoveContact(name);
        }

        public bool CreateContact(string name, string number)
        {
            if (_proxy != null)
            {
                return _proxy.CreateContact(name, number);
            }

            return _contactManagerService.CreateContact(name, number);
        }

        public string[] ReadSavedContacts(int from)
        {
            if (_proxy != null)
            {
                return _proxy.ReadSavedContacts(from);
            }

            return _contactManagerService.ReadSavedContacts(from);
        }
    }
}
