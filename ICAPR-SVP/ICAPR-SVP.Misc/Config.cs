using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICAPR_SVP.Misc
{
    public class Config
    {
        //network configuration
        public const String NET_LOCAL_HOST = "0.0.0.0";
        public const int NET_SERVER_PORT = 8181;
        public const String NET_SERVER_URL = "api/";
        //eyetribe configuration
        public const int SAMPLING_FREQUENCY = 60;   //Reduce sampling to the given frequency (I.e. 30 samples/seg)
        public const String EYETRIBE_CALIBRATION_EXE = @"..\..\..\libs\EyeTribe\Calibration.exe";
        public const String EYETRIBE_SERVER_EXE = @"..\..\..\libs\EyeTribe\EyeTribe.exe";
        //calibration
        public const int CALIB_TOTAL_SAMPLES = 1000;
    }
}
