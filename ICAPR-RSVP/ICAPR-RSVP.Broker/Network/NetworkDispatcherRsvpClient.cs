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
    public class NetworkDispatcherRsvpClient : NetworkDispatcher
    {
        private Port _outputPort;

        public void init(Port outputPort)
        {
            _outputPort = outputPort;
        }

        public String dispatchMessage(String msg)
        {
            Newtonsoft.Json.Linq.JObject json = Newtonsoft.Json.Linq.JObject.Parse(msg);
            String type = (String)json["type"];

            //If result is different from null the server will send it back to the client
            String result = null;

            switch (type)
            {
                case NetworkConstants.TYPE_STREAM:
                    DisplayItem<String> displayItem = JsonConvert.DeserializeObject<DisplayItem<String>>(json["content"].ToString());
                    Console.WriteLine(displayItem.Value + "  " + displayItem.Timestamp + "  " + displayItem.Duration);
                    Bundle<DisplayItem<String>> bundle = new Bundle<DisplayItem<String>>(ItemTypes.DisplayItem, displayItem);
                    _outputPort.PushItem(bundle);
                    break;

                case NetworkConstants.TYPE_TRIAL_CONFIG:
                    ExperimentConfig config = JsonConvert.DeserializeObject<ExperimentConfig>(json["content"].ToString());
                    Console.WriteLine("Received trial config : " + config.Trial);
                    Bundle<ExperimentConfig> newBundle = new Bundle<ExperimentConfig>(ItemTypes.Config, config);
                    _outputPort.PushItem(newBundle);
                    break;

                case NetworkConstants.TYPE_TRIALS:
                    Console.WriteLine("Received request for all trials");
                    FileManager<String> fm = new FileManager<string>("");
                    String trials = fm.getJsonFileList();
                    String wrapper = "{\"type\":\"" + NetworkConstants.TYPE_TRIALS + "\" , \"trials\": " + trials + "}";
                    result = wrapper;
                    break;

                case NetworkConstants.TYPE_SINGLE_TRIAL:
                    Console.WriteLine("Received request for a single trial");
                    FileManager<String> fmm = new FileManager<string>("");
                    String response = fmm.getJsonFile((String)json["content"]);
                    String wrapperResponse = "{\"type\":\"" + NetworkConstants.TYPE_SINGLE_TRIAL + "\" , \"trial\": " + response + "}";
                    return wrapperResponse;

                default:
                    break;
            }
            return result;
        }
    }
}