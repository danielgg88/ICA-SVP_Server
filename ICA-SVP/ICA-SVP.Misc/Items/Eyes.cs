using Newtonsoft.Json;

namespace ICA_SVP.Misc
{
    public class Eyes
    {
        [JsonProperty(PropertyName = "timestamp")]
        public long Timestamp
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "left_eye")]
        public Eye LeftEye
        {
            get;
            private set;
        }
        [JsonProperty(PropertyName = "right_eye")]
        public Eye RightEye
        {
            get;
            private set;
        }

        [JsonProperty(PropertyName = "left_eye_processed")]
        public Eye LeftEyeProcessed
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "right_eye_processed")]
        public Eye RightEyeProcessed
        {
            get;
            set;
        }

        public Eyes(long timestamp,Eye left,Eye right)
        {
            Timestamp = timestamp;
            LeftEye = left;
            RightEye = right;
        }
    }
}
