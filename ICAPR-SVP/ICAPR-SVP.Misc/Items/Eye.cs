using Newtonsoft.Json;
using System;

namespace ICAPR_SVP.Misc
{
    public class Eye
    {
        [JsonIgnore]
        private double _pupilSize;

        [JsonProperty(PropertyName = "diameter")]
        public double PupilSize
        {
            get
            {
                return _pupilSize;
            }
            set
            {
                _pupilSize = Math.Round(value, 4);
            }
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
