using System;
using System.Text;

namespace MyCompany.MyOnCardApp
{
    public class ContactManagerService : MarshalByRefObject
    {
        private delegate bool ProcessContact(string contact);

        private const string ContactsFileName = "contacts.txt";
        private const char Separator = ':';

        public bool UpdateContact(string name, string newName, string number)
        {
            if (newName == null)
            {
                return false;
            }

            if (number == null)
            {
                number = GetOldNumberByName(name);
                if (number == null)
                {
                    return false;
                }
            }

            if (!RemoveContact(name))
            {
                return false;
            }

            return CreateContact(newName, number);
        }

        public bool CreateContact(string name, string number)
        {
            if (!IsUniqueName(name))
            {
                return false;
            }

            string line = name + Separator + number;

            return Write(line);
        }

        public string[] ReadSavedContacts(int from)
        {
            return PubStorage.ReadLinesFromLinePosition(ContactsFileName, from);
        }

        public bool RemoveContact(string name)
        {
            if (name == null)
            {
                return false;
            }

            return PubStorage.RemoveLineStartsWith(ContactsFileName, name + Separator);
        }

        private string GetOldNumberByName(string name)
        {
            string oldNumber = null;

            bool ProcessContact(string contact)
            {
                if (contact == null)
                {
                    // stop iteration
                    return false;
                }

                var nameOnly = GetNameOnlyFromLine(contact);
                if (string.Compare(name, nameOnly, true) == 0)
                {
                    oldNumber = contact.Substring(contact.IndexOf(Separator) + 1);

                    // stop iteration
                    return false;
                }

                // continue iteration
                return true;
            }

            ForEachContact(ProcessContact);
            return oldNumber;
        }

        private static bool IsUniqueName(string name)
        {
            bool isUnique = true;

            bool ProcessContact(string contact)
            {
                if (contact == null)
                {
                    isUnique = false;

                    // stop iteration
                    return false;
                }

                var nameOnly = GetNameOnlyFromLine(contact);
                if (string.Compare(name, nameOnly, true) == 0)
                {
                    isUnique = false;

                    // stop iteration
                    return false;
                }

                // continue iteration
                return true;
            }

            ForEachContact(ProcessContact);
            return isUnique;
        }

        private static string GetNameOnlyFromLine(string resp)
        {
            if (resp == null)
            {
                return "";
            }

            var separatorIndex = resp.IndexOf(Separator);
            if (separatorIndex >= 0)
            {
                return resp.Substring(0, separatorIndex);
            }

            return resp;
        }

        private static bool IsPositiveNumberOrZero(string terminationSign, out int from)
        {
            from = 0;

            if (terminationSign == null)
            {
                return false;
            }
            
            foreach (char c in terminationSign)
            {
                if (c < '0' || c > '9')
                {
                    return false;
                }
            }

            from = int.Parse(terminationSign);
            return true;
        }

        private static bool Write(string content)
        {
            if (content == null)
            {
                return false;
            }

            return PubStorage.AppendLineToFileEnd(ContactsFileName, Encoding.ASCII.GetBytes(content));
        }

        private static void ForEachContact(ProcessContact processDelegate)
        {
            int from = 0;
            do
            {
                var readContactsChunck = PubStorage.ReadLinesFromLinePosition(ContactsFileName, from);
                if (readContactsChunck == null)
                {
                    break;
                }

                foreach (var contact in readContactsChunck)
                {
                    if (IsPositiveNumberOrZero(contact, out from))
                    {
                        break;
                    }

                    if (!processDelegate(contact))
                    {
                        return;
                    }
                }
            } while (from != 0);
        }
    }
}