using System;

namespace ICAPR_RSVP.Broker
{
    public class PortNonBlockingInputSpritz: PortNonBlocking
    {
        private Network _network;//Network server for clients

        public PortNonBlockingInputSpritz(String host, String path, int port)
        {
            //Create a network server
            _network = new Network(this, host, path, port);
        }

        #region Properties
        public override bool IsRunning
        {
            get
            {
                //Is newtork server running?
                return _network.IsConnected;
            }
        }
        #endregion

        #region Protected methods
        protected override void OnStart()
        {
            //Start network server
            _network.startNetwork();
        }

        protected override void OnStop()
        {
            //Stop network server
            _network.stopNetwork();
        }
        #endregion
    }
}
