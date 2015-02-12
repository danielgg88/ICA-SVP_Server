using System;

namespace ICAPR_RSVP.Broker
{
    public class PortNonBlockingInputSpritz: PortNonBlocking 
    {
        private Network _network;//Network server for clients

        public PortNonBlockingInputSpritz(Network network)
        {
            //Create a network server
            _network = network;
        }

        #region Properties
        public override bool IsRunning
        {
            get
            {
                //Is newtork server running?
                //return _network.IsConnected;
                //TODO the network connection is asynchronous this will always return false;
                return true;
            }
        }
        #endregion

        #region Protected methods
        protected override void OnStart()
        {
            //Start network server
            _network.startNetwork(this);
        }

        protected override void OnStop()
        {
            //Stop network server
            _network.stopNetwork();
        }
        #endregion

    }
}
