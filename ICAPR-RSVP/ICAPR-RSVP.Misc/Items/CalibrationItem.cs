using Newtonsoft.Json;

namespace ICAPR_RSVP.Misc
{
    public class CalibrationItem
    {
        [JsonProperty(PropertyName = "calibration_type")]
        public Types CalibrationType { get; private set; }

        public enum Types
        {
            EyeTribe,
            System
        }
    }
}
