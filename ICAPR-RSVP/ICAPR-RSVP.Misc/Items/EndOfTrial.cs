using Newtonsoft.Json;

namespace ICAPR_RSVP.Misc
{
    public class EndOfTrial
    {
        [JsonProperty(PropertyName = "timestamp")]
        public long Timestamp { get; private set; }
    }
}
