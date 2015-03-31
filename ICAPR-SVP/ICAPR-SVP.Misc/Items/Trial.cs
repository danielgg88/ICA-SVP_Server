using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ICAPR_SVP.Misc.Items
{
    public class Trial<T>
    {
        [JsonProperty(PropertyName = "configuration")]
        public ExperimentConfig Config
        {
            get;
            private set;
        }
        [JsonProperty(PropertyName = "data")]
        public List<DisplayItemAndEyes<T>> ExperimentData
        {
            get;
            private set;
        }
        [JsonProperty(PropertyName = "sampling_rate")]
        public int SamplingRate
        {
            get
            {
                return Misc.Config.EyeTribe.SAMPLING_FREQUENCY;
            }
        }

        public Trial(ExperimentConfig config,List<DisplayItemAndEyes<T>> data)
        {
            Config = config;
            ExperimentData = data;
        }
    }
}
