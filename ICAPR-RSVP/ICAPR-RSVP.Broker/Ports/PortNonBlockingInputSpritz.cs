using System;

namespace ICAPR_RSVP.Broker
{
    public class PortNonBlockingInputSpritz: PortNonBlocking
    {
        private Network _network;

        public PortNonBlockingInputSpritz(String host, String path, int port)
        {
            _network = new Network(this, host, path, port);
        }

        #region Properties
        public override bool IsRunning
        {
            get
            {
                return _network.IsConnected;
            }
        }
        #endregion

        #region Protected methods
        protected override void OnStart()
        {
            _network.startNetwork();
        }

        protected override void OnStop()
        {
            _network.stopNetwork();
        }
        #endregion
    }
}
