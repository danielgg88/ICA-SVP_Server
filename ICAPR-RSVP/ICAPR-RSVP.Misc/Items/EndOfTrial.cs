using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ICAPR_RSVP.Misc
{
    public class EndOfTrial
    {
        [JsonProperty(PropertyName = "timestamp")]
        public long Timestamp { get; private set; }
    }
}
