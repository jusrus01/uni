using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using SmartCard.Runtime.Remoting.Channels.APDU;

namespace MyCompany.MyOnCardApp
{
    /// <summary>
    /// Summary description for MyServer.
    /// https://www.codeproject.com/Articles/263118/Building-Your-Own-Security-Application-with-the-Ge
    /// </summary>
    public class MyServer
    {
        /// <summary>
        /// specify the exposed remote object URI.
        /// </summary>
        private const string REMOTE_OBJECT_URI = "SMS.uri";

        /// <summary>
        /// Register the server onto the card.
        /// </summary>
        /// <returns></returns>
        public static int Main()
        {
            // Register the channel the server will be listening to.
            ChannelServices.RegisterChannel(new APDUServerChannel());

            // Register this application as a server            
            RemotingConfiguration.RegisterWellKnownServiceType(typeof(ContactManagerService), REMOTE_OBJECT_URI, WellKnownObjectMode.Singleton);

            return 0;
        }
    }
}

