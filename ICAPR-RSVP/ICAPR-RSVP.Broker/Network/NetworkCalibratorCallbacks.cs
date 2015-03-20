using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICAPR_RSVP.Broker
{
    public class NetworkCalibratorCallbacks : Calibration.Calibrator.CalibrationCallbacks
    {
        public Network Network { get; private set; }

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
