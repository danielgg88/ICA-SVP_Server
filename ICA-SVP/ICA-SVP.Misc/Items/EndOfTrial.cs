using Newtonsoft.Json;

namespace ICA_SVP.Misc
{
    public class EndOfTrial
    {
        [JsonProperty(PropertyName = "timestamp")]
        public long Timestamp
        {
            get;
            private set;
        }
    }
}
