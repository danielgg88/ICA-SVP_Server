using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ICAPR_RSVP.Misc;
using ICAPR_RSVP.Misc.Utils;

namespace ICAPR_RSVP.Broker
{




    public class SpritzNetworkDispatcher : INetworkDispather
    {
        public const String TYPE_STREAM = "stream";
        public const String TYPE_TRIALS = "trials";
        public const String TYPE_SINGLE_TRIAL = "trial";
        public const String TYPE_TRIAL_CONFIG = "config";

        public Port CorePort { get; set; }

        public Network CoreNetwork { get; set; }

        public void dispatchMessage(String msg)
        {
            Newtonsoft.Json.Linq.JObject json = Newtonsoft.Json.Linq.JObject.Parse(msg);

            String type = (String)json["type"];

            switch (type)
            {
                case TYPE_STREAM:
                    DisplayItem<String> word = JsonConvert.DeserializeObject<DisplayItem<String>>(json["content"].ToString());
                    Console.WriteLine(word.Value + "  " + word.Timestamp + "  " + word.Duration);
                    Bundle<DisplayItem<String>> bundle = new Bundle<DisplayItem<String>>(ItemTypes.Word, word);
                    CorePort.PushItem(bundle);
                    break;
                case TYPE_TRIAL_CONFIG:
                    ExperimentConfig config = JsonConvert.DeserializeObject<ExperimentConfig>(json["content"].ToString());
                    Console.WriteLine("Received trial config : " + config.Trial);
                    Bundle<ExperimentConfig> newBundle = new Bundle<ExperimentConfig>(ItemTypes.Config, config);
                    CorePort.PushItem(newBundle);
                    break;
                case TYPE_TRIALS:
                    Console.WriteLine("Received request for all trials");
                    FileManager<String> fm = new FileManager<string>("");
                    String trials = fm.getJsonFileList();
                    String wrapper = "{\"type\":\"" + TYPE_TRIALS + "\" , \"trials\": " + trials + "}";
                    Console.WriteLine(wrapper);
                    CoreNetwork.sendMessage(wrapper);
                    break;
                case TYPE_SINGLE_TRIAL:
                    Console.WriteLine("Received request for a single trial");

                    break;

                default:

                    break;
            }

        }

    }

}
