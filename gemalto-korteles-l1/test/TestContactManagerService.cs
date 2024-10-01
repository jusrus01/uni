using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyCompany.MyOnCardApp;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace test
{
    [TestClass]
    public class TestContactManagerService
    {
        [TestMethod]
        public void ReadAllContactsReadAllSomeDeletedInsideAndSomeChanged()
        {
            var contractService = new ContactManagerService();

            for (var i = 0; i < 50; i++)
            {
                Assert.IsTrue(contractService.CreateContact(i.ToString(), i.ToString()));
            }

            Assert.IsTrue(contractService.RemoveContact("10"));
            Assert.IsTrue(contractService.CreateContact("10", "10"));

            Assert.IsTrue(contractService.RemoveContact("20"));
            Assert.IsTrue(contractService.CreateContact("20", "20"));

            Assert.IsTrue(contractService.UpdateContact("20", "stop", "stop"));

            List<string> allLinesRead = new List<string>();
            int from = 0;
            do
            {
                var content = contractService.ReadSavedContacts(from);
                foreach (var line in content)
                {
                    if (int.TryParse(line, out from))
                    {
                        break;
                    }

                    allLinesRead.Add(line);
                }
            }
            while (from != 0);

            Assert.AreEqual(50, allLinesRead.Count);
            for (int i = 0; i < 50; i++)
            {
                if (i == 20)
                {
                    continue;
                }

                var once = allLinesRead.SingleOrDefault(j => j == $"{i}:{i}");
                Assert.IsTrue(once != null);
            }

            Assert.IsTrue(allLinesRead.SingleOrDefault(j => j.Contains("stop")) != null);
        }

        [TestMethod]
        public void ReadAllContactsOneDeletedThenTheSameCanBeAddedAndThenUpdated()
        {
            var contractService = new ContactManagerService();

            (string, string) firstContact = ("John Smith", "13123123213");
            (string, string) secondContact = ("John Smith", "1311231223123213");

            Assert.IsTrue(contractService.CreateContact(firstContact.Item1, firstContact.Item2));

            Assert.IsTrue(contractService.RemoveContact(firstContact.Item1));

            Assert.IsTrue(contractService.CreateContact(secondContact.Item1, secondContact.Item2));

            Assert.IsTrue(contractService.UpdateContact(secondContact.Item1, "John S", "7777777777777777777"));

            var content = contractService.ReadSavedContacts(0);
            Assert.IsNotNull(content);
            Assert.AreEqual(11, content.Length);
            Assert.AreEqual("John S:7777777777777777777", content[0]);
            Assert.AreEqual("0", content[1]);
        }

        [TestMethod]
        public void ReadAllContactsOneDeletedThenTheSameCanBeAddedAndThenUpdatedToTheSameNumber()
        {
            var contractService = new ContactManagerService();

            (string, string) firstContact = ("John Smith", "13123123213");
            (string, string) secondContact = ("John Smith", "1311231223123213");

            Assert.IsTrue(contractService.CreateContact(firstContact.Item1, firstContact.Item2));

            Assert.IsTrue(contractService.RemoveContact(firstContact.Item1));

            Assert.IsTrue(contractService.CreateContact(secondContact.Item1, secondContact.Item2));

            Assert.IsTrue(contractService.UpdateContact(secondContact.Item1, "John S", null));

            var content = contractService.ReadSavedContacts(0);
            Assert.IsNotNull(content);
            Assert.AreEqual(11, content.Length);
            Assert.AreEqual("John S:1311231223123213", content[0]);
            Assert.AreEqual("0", content[1]);
        }

        [TestMethod]
        public void ReadAllContactsReadMax11ContentReceivedAndSomeDeleted()
        {
            var contractService = new ContactManagerService();

            for (var i = 0; i < 50; i++)
            {
                Assert.IsTrue(contractService.CreateContact(i.ToString(), i.ToString()));
            }

            Assert.IsTrue(contractService.RemoveContact("10"));
            Assert.IsTrue(contractService.CreateContact("10", "10"));

            Assert.IsTrue(contractService.RemoveContact("20"));
            Assert.IsTrue(contractService.CreateContact("20", "20"));

            var content = contractService.ReadSavedContacts(0);
            Assert.IsNotNull(content);
            Assert.AreEqual(11, content.Length);
            Assert.AreEqual("10", content[10]);
        }

        [TestMethod]
        public void ReadAllContactsReadAllSomeDeletedInside()
        {
            var contractService = new ContactManagerService();

            for (var i = 0; i < 50; i++)
            {
                Assert.IsTrue(contractService.CreateContact(i.ToString(), i.ToString()));
            }

            Assert.IsTrue(contractService.RemoveContact("10"));
            Assert.IsTrue(contractService.CreateContact("10", "10"));

            Assert.IsTrue(contractService.RemoveContact("20"));
            Assert.IsTrue(contractService.CreateContact("20", "20"));

            List<string> allLinesRead = new List<string>();
            int from = 0;
            do
            {
                var content = contractService.ReadSavedContacts(from);
                foreach (var line in content)
                {
                    if (int.TryParse(line, out from))
                    {
                        break;
                    }

                    allLinesRead.Add(line);
                }
            }
            while (from != 0);

            Assert.AreEqual(50, allLinesRead.Count);
            for (int i = 0; i < 50; i++)
            {
                var once = allLinesRead.SingleOrDefault(j => j == $"{i}:{i}");
                Assert.IsTrue(once != null);
            }
        }

        [TestMethod]
        public void ReadAllContactsOneDeletedThenTheSameCanBeAdded()
        {
            var contractService = new ContactManagerService();

            (string, string) firstContact = ("John Smith", "13123123213");
            (string, string) secondContact = ("John Smith", "1311231223123213");

            Assert.IsTrue(contractService.CreateContact(firstContact.Item1, firstContact.Item2));

            Assert.IsTrue(contractService.RemoveContact(firstContact.Item1));

            Assert.IsTrue(contractService.CreateContact(secondContact.Item1, secondContact.Item2));

            var content = contractService.ReadSavedContacts(0);
            Assert.IsNotNull(content);
            Assert.AreEqual(11, content.Length);
            Assert.AreEqual("John Smith:1311231223123213", content[0]);
            Assert.AreEqual("0", content[1]);
        }

        [TestMethod]
        public void ReadAllContactsReadFewContactsBackendOnly()
        {
            var contractService = new ContactManagerService();

            (string, string) firstContact = ("John Smith", "13123123213");
            (string, string) secondContact = ("Anton Smith", "13123123213");

            Assert.IsTrue(contractService.CreateContact(firstContact.Item1, firstContact.Item2));
            Assert.IsTrue(contractService.CreateContact(secondContact.Item1, secondContact.Item2));

            var content = contractService.ReadSavedContacts(0);
            Assert.IsNotNull(content);
            Assert.AreEqual(11, content.Length);
            Assert.AreEqual("John Smith:13123123213", content[0]);
            Assert.AreEqual("Anton Smith:13123123213", content[1]);
            Assert.AreEqual("0", content[2]);
        }

        [TestMethod]
        public void ReadAllContactsReadOnlyUnique()
        {
            var contractService = new ContactManagerService();

            (string, string) firstContact = ("John Smith", "13123123213");
            (string, string) secondContact = ("John Smith", "1311231223123213");

            Assert.IsTrue(contractService.CreateContact(firstContact.Item1, firstContact.Item2));
            Assert.IsFalse(contractService.CreateContact(secondContact.Item1, secondContact.Item2));

            var content = contractService.ReadSavedContacts(0);
            Assert.IsNotNull(content);
            Assert.AreEqual(11, content.Length);
            Assert.AreEqual("John Smith:13123123213", content[0]);
            Assert.AreEqual("0", content[1]);
        }

        [TestMethod]
        public void ReadAllContactsReadMax11ContentReceived()
        {
            var contractService = new ContactManagerService();

            for (var i = 0; i < 50; i++)
            {
                Assert.IsTrue(contractService.CreateContact(i.ToString(), i.ToString()));
            }
      
            var content = contractService.ReadSavedContacts(0);
            Assert.IsNotNull(content);
            Assert.AreEqual(11, content.Length);
            Assert.AreEqual("10", content[10]);
        }

        [TestMethod]
        public void ReadAllContactsReadAll()
        {
            var contractService = new ContactManagerService();

            for (var i = 0; i < 50; i++)
            {
                Assert.IsTrue(contractService.CreateContact(i.ToString(), i.ToString()));
            }

            List<string> allLinesRead = new List<string>();
            int from = 0;
            do
            {
                var content = contractService.ReadSavedContacts(from);
                foreach (var line in content)
                {
                    if (int.TryParse(line, out from))
                    {
                        break;
                    }

                    allLinesRead.Add(line);
                }
            }
            while (from != 0);

            Assert.AreEqual(50, allLinesRead.Count);
            for (int i = 0; i < 50; i++)
            {
                var once = allLinesRead.SingleOrDefault(j => j == $"{i}:{i}");
                Assert.IsTrue(once != null);
            }
        }

        [TestCleanup]
        [TestInitialize]
        public void Cleanup()
        {
            PubStorage.PubAbsolutePath = "c:\\testOnly\\";
            if (!Directory.Exists(PubStorage.PubAbsolutePath))
            {
                Directory.CreateDirectory(PubStorage.PubAbsolutePath);
            }

            var path = "c:\\testOnly\\contacts.txt";
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}