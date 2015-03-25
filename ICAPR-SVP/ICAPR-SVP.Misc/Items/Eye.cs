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
    }
}
