using Newtonsoft.Json;

namespace ICAPR_SVP.Misc
{
    public class SummaryItem
    {
        [JsonProperty(PropertyName = "ica")]
        public string Ica
        {
            get;
            private set;
        }

        public SummaryItem(string ica)
        {
            Ica = ica;
        }
    }
}
