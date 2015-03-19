using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ICAPR_RSVP.Misc
{
    public class DisplayItemAndEyes<T>
    {
        [JsonProperty(PropertyName = "item")]
        public DisplayItem<T> DisplayItem { get; private set; }   //Null when idle time
        [JsonProperty(PropertyName = "eyes_original")]
        public Queue<Eyes> EyesOriginal { get; private set;}
        [JsonProperty(PropertyName = "eyes_processed")]
        public Queue<Eyes> EyesProcessed { get; set; }
        [JsonProperty(PropertyName = "summary")]
        public SummaryItem SummaryItem { get; set; }

        public DisplayItemAndEyes(Queue<Eyes> eyes, DisplayItem<T> word)
        {
            DisplayItem = word;
            EyesOriginal = eyes;
        }
    }
}
