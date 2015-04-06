using System;

namespace ICAPR_SVP.Misc
{
    public class Config
    {
        public class Network
        {
            //network configuration
            public const String NET_LOCAL_HOST = "0.0.0.0";
            public const int NET_SERVER_PORT = 8181;
            public const String NET_SERVER_URL = "api/";
        }

        public class EyeTribe
        {
            //eyetribe configuration
            public const int SAMPLING_FREQUENCY = 60;   //Reduce sampling to the given frequency (I.e. 30 samples/seg)
            public const String EYETRIBE_CALIBRATION_EXE = @"..\..\..\libs\EyeTribe\Calibration.exe";
            public const String EYETRIBE_SERVER_EXE = @"..\..\..\libs\EyeTribe\EyeTribe.exe";
        }

        public class Calibration
        {
            //calibration
            public const int CALIB_TOTAL_SAMPLES = 60;
            public const double CALIB_DEFAULT_AVG_PUPIL_SIZE = 4.5; //mm
            public const int TEST_MAX_PUPIL_SIZE = 7; //mm
        }

        public class Cleaning
        {
            public const double BLINK_DIAMETER_THRESHOLD_LOW_MM = 2;       //Max pupil diameter variation from the 
            //baseline before being considered a blink
            public const int BLINK_MIN_CONSEQUENT_SAMPLES = 4; //Min no. of samples to consider a blink appereance
            public const int BLINK_MAX_ALLOWED_SAMPLES = 20;  //Max allowed duration of one blink before the sample 
            //is discarded
            public const int BLINK_MAX_ALLOWED_PERC = 20;     //Percentage (%) of max allowed blink detections in 1 sample
            public const double OUTLIER_MAX_CHANGE_RATE_ALLOWED_MM = 0.1; //Maximum change rate allowed from the two preceding
            //samples.
        }

        public class Matlab
        {
            
            public const String DENOISE_ALGORITHM = "db4";  //daubechies 4
            public const int DENOISE_LEVEL = 5;
        }

        public class ICA
        {
            public const int AVG_MOVING_WINDOW_SIZE = 10;
            public const double SIGNAL_THRESHOLD_LARGE = 0.05;
        }
    }
}
