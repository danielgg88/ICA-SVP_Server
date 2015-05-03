using System;

namespace ICAPR_SVP.Misc
{
    public static class Config
    {
        public static class Network
        {
            //network configuration
            public const String NET_LOCAL_HOST = "0.0.0.0";
            public const int NET_SERVER_PORT = 8184;
            public const String NET_SERVER_URL = "api/";
        }

        public static class EyeTribe
        {
            //eyetribe configuration
            public const int SAMPLING_FREQUENCY = 60;   //Reduce sampling to the given frequency (I.e. 30 samples/seg)
            public const String EYETRIBE_CALIBRATION_EXE = @"..\..\..\libs\EyeTribe\Calibration.exe";
            public const String EYETRIBE_SERVER_EXE = @"..\..\..\libs\EyeTribe\EyeTribe.exe";
            public static bool ALLOW_CURSOR_CONTROL = true;
        }

        public static class Calibration
        {
            //calibration
            public const int CALIB_TOTAL_SAMPLES = 240;
            public const double CALIB_DEFAULT_AVG_PUPIL_SIZE = 4.5; //mm
            public const int TEST_MAX_PUPIL_SIZE = 7; //mm
        }

        public static class Cleaning
        {
            public const double BLINK_DIAMETER_THRESHOLD_LOW_MM = 2;       //Max pupil diameter variation from the 
            //baseline before being considered a blink
            public const int BLINK_MIN_CONSEQUENT_SAMPLES = 4; //Min no. of samples to consider a blink appereance
            public const int BLINK_MAX_ALLOWED_SAMPLES = 16;  //Max allowed duration of one blink before the sample 
            //is discarded
            public const int BLINK_MAX_ALLOWED_PERC = 20;     //Percentage (%) of max allowed blink detections in 1 sample
            public const double OUTLIER_MAX_CHANGE_RATE_ALLOWED_MM = 0.2; //Maximum change rate allowed from the two preceding
            //samples.
        }

        public static class Matlab
        {
            public const String DENOISE_ALGORITHM = "db4";  //daubechies 4
            public const int DENOISE_LEVEL = 4;
            public const bool SHOW_MATLAB_PLOT = false;
        }

        public static class ICA
        {
            public const int AVG_MOVING_WINDOW_SIZE = 2;
            public const double SIGNAL_THRESHOLD_LARGE = 0.05;
        }

        public static class FileManager
        {
            public const String BASE_PATH = @"..\..\..\";
            public const bool CREATE_JSON = true;
            public const bool CREATE_CSV_ORIGINAL_PROCESSED = true;
            public const bool CREATE_CSV_PER_SECOND = true;
        }

        public static class WEKA
        {

            public readonly static String WEKA_EXTERNAL_MODEL = @"..\..\..\output.model";

            /*
             * Currently is not needed. It is used in the code 
             * but it will always be 0.
             */
            public readonly static int ATTRIBUTE_START_INDEX = 0;

            public readonly static String[] LABELS = {
                                           "C_bright",
                                           "C_obscure",
                                           "C_steady",
                                           "I_bright",
                                           "I_obscure",
                                           "I_steady",
                                       };


            /*
             * Include all the attributes that have been used for training
             * except the attribute of the classification label.
             * 
             * This attribute is added in the ClassifierWrapper
             */
            public readonly static String[] ATTRIBUTES = {
                                                                "ica",
                                                                "avg",          //average fo the whole sample (60 frames)
                                                                "split_avg",    //difference of average of the first 30 samples and the average of the other 30
                                                                "s0",
                                                                "s1",
                                                                "s2",
                                                                "s3",
                                                                "s4",
                                                                "s5",
                                                                "s6",
                                                                "s7",
                                                                "s8",
                                                                "s9",
                                                                "s10",
                                                                "s11",
                                                                "s12",
                                                                "s13",
                                                                "s14",
                                                                "s15",
                                                                "s16",
                                                                "s17",
                                                                "s18",
                                                                "s19",
                                                                "s20",
                                                                "s21",
                                                                "s22",
                                                                "s23",
                                                                "s24",
                                                                "s25",
                                                                "s26",
                                                                "s27",
                                                                "s28",
                                                                "s29",
                                                                "s30",
                                                                "s31",
                                                                "s32",
                                                                "s33",
                                                                "s34",
                                                                "s35",
                                                                "s36",
                                                                "s37",
                                                                "s38",
                                                                "s39",
                                                                "s40",
                                                                "s41",
                                                                "s42",
                                                                "s43",
                                                                "s44",
                                                                "s45",
                                                                "s46",
                                                                "s47",
                                                                "s48",
                                                                "s49",
                                                                "s50",
                                                                "s51",
                                                                "s52",
                                                                "s53",
                                                                "s54",
                                                                "s55",
                                                                "s56",
                                                                "s57",
                                                                "s58",
                                                                "s59"
                                                               };


        }
    }
}
