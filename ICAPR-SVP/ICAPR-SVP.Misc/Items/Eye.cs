using Newtonsoft.Json;

namespace ICAPR_SVP.Misc
{
    public class Eye
    {
        [JsonProperty(PropertyName = "diameter")]
        public double PupilSize
        {
            get;
            set;
        }

        [JsonIgnore]
        public CleaningFlags CleaningFlag
        {
            get;
            set;
        }

        public enum CleaningFlags
        {
            Raw,
            Ok,
            PossibleBlink,
            Blink,
            Error
        }

        public Eye()
        {
            CleaningFlag = CleaningFlags.Raw;
        }
    }
}
