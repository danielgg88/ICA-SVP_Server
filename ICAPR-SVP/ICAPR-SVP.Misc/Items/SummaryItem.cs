using Newtonsoft.Json;

namespace ICAPR_SVP.Misc
{
    public class SummaryItem
    {
        [JsonProperty(PropertyName = "ica")]
        public int[][] Ica
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "error_samples")]
        public int[] ErrorSamples
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "blink_samples")]
        public int[] BlinkSamples
        {
            get;
            set;
        }

        public SummaryItem()
        {
            BlinkSamples = new int[2];
            ErrorSamples = new int[2];
        }
    }
}
