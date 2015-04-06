using Newtonsoft.Json;

namespace ICAPR_SVP.Misc
{
    public class SummaryItem
    {
        [JsonProperty(PropertyName = "ica")]
        public int[] Ica
        {
            get;
            set;
        }

        public SummaryItem(int[] ica)
        {
            Ica = ica;
        }
    }
}
