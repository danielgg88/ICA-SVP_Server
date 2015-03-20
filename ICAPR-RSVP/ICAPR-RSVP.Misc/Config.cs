using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICAPR_RSVP.Misc
{
    public class Config
    {
        //network configuration
        public  const String NET_LOCAL_HOST = "0.0.0.0";
        public  const int NET_SERVER_PORT = 8181;
        public  const String NET_SERVER_URL = "api/";
        //eyetribe configuration
        public  const int SAMPLING_FREQUENCY = 10;   //Reduce sampling to the given frequency (I.e. 10 samples/seg)
        public  const String EYETRIBE_CALIBRATION_EXE = @"..\..\..\libs\EyeTribe\Calibration.exe";
        public  const String EYETRIBE_SERVER_EXE = @"..\..\..\libs\EyeTribe\EyeTribe.exe";
    }
}
