
using System;
using System.Runtime.Remoting.Channels;
using SmartCard.Runtime.Remoting.Channels.APDU;

// make sure you add the reference to your server stub dll or interface
// The stub file is automatically generated for you, under [Server Project Output]\Stub).
using MyCompany.MyOnCardApp;

namespace MyCompany.MyClientApp
{
    /// <summary>
    /// Summary description for MyClient.
    /// </summary>
    public class MyClient
    {
        private const string URL = "apdu://selfdiscover/SMS.uri";
        private const bool UseProxy = true;

        public static void Main()
        {
            if (UseProxy)
            {
                ContactManagerProxy managerProxyTest = new ContactManagerProxy(new ContactManagerService2());
                Controller controllerTest = new Controller(managerProxyTest);
                controllerTest.Run();
                return;
            }

            APDUClientChannel channel = new APDUClientChannel();
            ChannelServices.RegisterChannel(channel, false);

            ContactManagerService contactManagerService = (ContactManagerService)Activator.GetObject(typeof(ContactManagerService), URL);
            ContactManagerProxy managerProxy = new ContactManagerProxy(contactManagerService);
            Controller controller = new Controller(managerProxy);
            controller.Run();

            ChannelServices.UnregisterChannel(channel);
        }
    }
}

