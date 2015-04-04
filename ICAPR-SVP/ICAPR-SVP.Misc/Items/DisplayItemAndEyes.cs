using Newtonsoft.Json;
using System.Collections.Generic;

namespace ICAPR_SVP.Misc
{
    public class DisplayItemAndEyes<T>
    {
        [JsonProperty(PropertyName = "item")]
        public DisplayItem<T> DisplayItem
        {
            get;
            private set;
        }   //Null when idle time
        [JsonProperty(PropertyName = "eyes")]
        public Queue<Eyes> Eyes
        {
            get;
            private set;
        }
        [JsonProperty(PropertyName = "summary")]
        public SummaryItem SummaryItem
        {
            get;
            set;
        }

        public DisplayItemAndEyes(Queue<Eyes> eyes,DisplayItem<T> word)
        {
            DisplayItem = word;
            Eyes = eyes;
        }
    }
}
