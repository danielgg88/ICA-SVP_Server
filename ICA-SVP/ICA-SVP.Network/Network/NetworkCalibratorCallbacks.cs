using ICA_SVP.Misc.Calibration;
using System;

namespace ICA_SVP.Network
{
    public class NetworkCalibratorCallbacks : Calibrator.CalibrationCallbacks
    {
        public Network Network
        {
            get;
            private set;
        }

        public NetworkCalibratorCallbacks(Network net)
        {
            Network = net;
        }

        #region Calibration
        public void OnStart()
        {
            String msg = "{\"type\":\"calibration\",\"content\":\"" + NetworkConstants.NET_CALIBRATION_STARTED + "\"}";
            Network.sendMessage(msg);
        }

        public void OnStop()
        {
            String msg = "{\"type\":\"calibration\",\"content\":\"" + NetworkConstants.NET_CALIBRATION_FINISHED + "\"}";
            Network.sendMessage(msg);
        }
        #endregion
    }
}
