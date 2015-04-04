using ICAPR_SVP.Misc;
using ICAPR_SVP.Misc.Calibration;
using Newtonsoft.Json;
using System;

namespace ICAPR_SVP.Network
{
    public class NetworkDispatcherSVPClient : NetworkDispatcher
    {
        private Port _outputPort;
        private Calibrator _calibrator;

        public void init(Port outputPort,Calibrator calibrator)
        {
            _outputPort = outputPort;
            _calibrator = calibrator;
        }

        public String dispatchMessage(String msg)
        {
            Newtonsoft.Json.Linq.JObject json = Newtonsoft.Json.Linq.JObject.Parse(msg);
            String type = (String)json["type"];

            //If result is different from null the server will send it back to the client
            String result = null;

            switch(type)
            {
                case NetworkConstants.TYPE_STREAM:
                    DisplayItem<String> displayItem = JsonConvert.DeserializeObject<DisplayItem<String>>(json["content"].ToString());
                    Console.WriteLine("Network: receieved -> item: " + displayItem.Timestamp + "\t" + displayItem.Duration + "\t" + displayItem.Value);
                    Bundle<DisplayItem<String>> bundle = new Bundle<DisplayItem<String>>(ItemTypes.DisplayItem,displayItem);
                    _outputPort.PushItem(bundle);
                    break;

                case NetworkConstants.TYPE_TRIAL_CONFIG:
                    ExperimentConfig config = JsonConvert.DeserializeObject<ExperimentConfig>(json["content"].ToString());
                    Console.WriteLine("Network: receieved -> trial: " + config.Trial);
                    Bundle<ExperimentConfig> newBundle = new Bundle<ExperimentConfig>(ItemTypes.Config,config);
                    _outputPort.PushItem(newBundle);
                    break;

                case NetworkConstants.TYPE_TRIALS:
                    Console.WriteLine("Network: receieved -> get: all");
                    String trials = Misc.Utils.FileManager<String>.getJsonFileList();
                    String wrapper = "{\"type\":\"" + NetworkConstants.TYPE_TRIALS + "\" , \"trials\": " + trials + "}";
                    result = wrapper;
                    break;

                case NetworkConstants.TYPE_SINGLE_TRIAL:
                    String file_name = (String)json["content"];
                    Console.WriteLine("Network: receieved -> get: " + file_name);

                    String response = Misc.Utils.FileManager<String>.getJsonFile(file_name);
                    String wrapperResponse = "{\"type\":\"" + NetworkConstants.TYPE_SINGLE_TRIAL + "\" , \"trial\": " + response + "}";
                    return wrapperResponse;

                case NetworkConstants.NET_TYPE_SERVICE_STOPPED:
                    Console.WriteLine("Network: receieved -> experiment: stopped");
                    EndOfTrial endOfTrial = JsonConvert.DeserializeObject<EndOfTrial>(json["content"].ToString());
                    Bundle<EndOfTrial> bb = new Bundle<EndOfTrial>(ItemTypes.EndOfTrial,endOfTrial);
                    _outputPort.PushItem(bb);
                    break;

                case NetworkConstants.NET_TYPE_CALIBRATION:
                    Console.WriteLine("Network: receieved -> calibration: start");
                    CalibrationItem calibrationItem = JsonConvert.DeserializeObject<CalibrationItem>(json["content"].ToString());
                    if(calibrationItem.CalibrationType == CalibrationItem.Types.EyeTribe)
                        Misc.Utils.Utils.launchEyeTribeCalibration();
                    else if(calibrationItem.CalibrationType == CalibrationItem.Types.System)
                        _calibrator.Start();
                    break;

                default:
                    break;
            }
            return result;
        }
    }
}