using Newtonsoft.Json;

namespace ICAPR_RSVP.Misc
{
    public class Eye
    {
        [JsonProperty(PropertyName = "diameter")]
        public double PupilSize{ get; set; }
    }
}
