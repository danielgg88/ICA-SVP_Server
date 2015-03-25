﻿using Newtonsoft.Json;

namespace ICAPR_SVP.Misc
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

        public Eyes(long timestamp,Eye left,Eye right)
        {
            Timestamp = timestamp;
            LeftEye = left;
            RightEye = right;
        }
    }
}