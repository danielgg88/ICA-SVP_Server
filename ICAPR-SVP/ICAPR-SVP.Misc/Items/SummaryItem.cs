using Newtonsoft.Json;
using System;

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

        [JsonProperty(PropertyName = "classification")]
        public String[][] Classification
        {
            get;
            set;
        }

        [JsonIgnore]
        public double[][] SampleAverage
        {
            get;
            set;
        }

        [JsonIgnore]
        public double[][] SampleAverageDifference
        {
            get;
            set;
        }
        
        public SummaryItem()
        {
            BlinkSamples = new int[2];
            ErrorSamples = new int[2];
            SampleAverage = new double[2][];
            SampleAverageDifference = new double[2][];
        }
    }
}
