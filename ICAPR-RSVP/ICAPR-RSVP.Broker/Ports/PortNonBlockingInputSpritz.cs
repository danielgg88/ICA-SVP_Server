using System;
using ICAPR_RSVP.Broker.Calibration;

namespace ICAPR_RSVP.Broker
{
    public class PortNonBlockingInputSpritz: PortNonBlocking 
    {
        private Network _network;//Network server for clients
        private Calibrator _calibrator;

        public PortNonBlockingInputSpritz(Network network, Calibrator calibrator)
        {
            //Create a network server
            _network = network;
            _calibrator = calibrator;
        }

        #region Properties
        public override bool IsRunning
        {
            get
            {
                //Is newtork server running?
                //return _network.IsConnected;
                //The network connection is asynchronous this will always return false;
                return true;
            }
        }
        #endregion

        #region Protected methods
        protected override void OnStart()
        {
            //Start network server
            _network.startNetwork(this, _calibrator);
        }

        protected override void OnStop()
        {
            //Stop network server
            _network.stopNetwork();
        }
        #endregion

    }
}
