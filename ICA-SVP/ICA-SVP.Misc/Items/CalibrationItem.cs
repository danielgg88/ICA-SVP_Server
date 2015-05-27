using Newtonsoft.Json;

namespace ICA_SVP.Misc
{
    public class CalibrationItem
    {
        [JsonProperty(PropertyName = "calibration_type")]
        public Types CalibrationType
        {
            get;
            private set;
        }

        public enum Types
        {
            EyeTribe,
            System
        }
    }
}
