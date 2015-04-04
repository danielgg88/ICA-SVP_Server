using Newtonsoft.Json;
using System;

namespace ICAPR_SVP.Misc
{
    public class ExperimentConfig
    {
        [JsonProperty(PropertyName = "trial")]
        public String Trial
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "file_name")]
        public String FileName
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "item_time")]
        public String ItemTime
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "delay_time")]
        public String DelayTime
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "user_name")]
        public String UserName
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "user_age")]
        public String UserAge
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "font_size")]
        public String FontSize
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "font_color")]
        public String FontColor
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "app_bg")]
        public String BackgroundColor
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "bg_modality")]
        public String BackgroundModality
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "padding")]
        public String GazePadding
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "window")]
        public String GazeWindow
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "cal_bg")]
        public String CalibrationBgColor
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "save_log")]
        public Boolean SaveLog
        {
            get;
            set;
        }
    }
}
