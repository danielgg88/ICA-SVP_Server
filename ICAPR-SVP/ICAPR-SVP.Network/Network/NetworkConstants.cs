using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICAPR_SVP.Network
{
    static class NetworkConstants
    {
        public const String TYPE_STREAM = "stream";
        public const String TYPE_TRIALS = "trials";
        public const String TYPE_SINGLE_TRIAL = "trial";
        public const String TYPE_TRIAL_CONFIG = "config";
        public const String NET_TYPE_SERVICE_STARTED = "serviceStarted";
        public const String NET_TYPE_SERVICE_STOPPED = "serviceStopped";
        public const String NET_TYPE_SERVICE_PAUSED = "servicePaused";
        public const String NET_TYPE_SERVICE_RESUMED = "serviceResumed";
        public const String NET_TYPE_CALIBRATION = "calibration";
        public const String NET_CALIBRATION_STARTED = "calibrationStarted";
        public const String NET_CALIBRATION_FINISHED = "calibrationFinished";
    }
}
