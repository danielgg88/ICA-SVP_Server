using ICA_SVP.DataCleaning;
using ICA_SVP.Misc;
using ICA_SVP.Misc.Executors;
using ICA_SVP.Misc.Items;
using Newtonsoft.Json;
using System;

namespace ICA_SVP
{
    public class TestingJSON
    {
        static void Main(string[] args)
        {
            //Load log file
            String json = Misc.Utils.FileManager<string>.getJsonFile("20150403081935668_");
            Trial<String> trial = JsonConvert.DeserializeObject<Trial<String>>(json);

            //Add ports
            Misc.Port inputPort = new Misc.PortBlockingDefaultImpl();
            Misc.Port outputPort = new Misc.PortBlockingDefaultImpl();
            ExecutorMultiThreadFilters dataCleaner = new ExecutorMultiThreadFilters(inputPort,outputPort);
            dataCleaner.AddFilter(new FilterBlinkDetection("Blink detection"));

            //Start ports
            inputPort.Start();
            outputPort.Start();
            //Start services
            dataCleaner.startInBackground();

            //Push items
            foreach(DisplayItemAndEyes<String> item in trial.ExperimentData)
            {
                foreach(Eyes eyes in item.Eyes)
                {
                    inputPort.PushItem(new Bundle<Eyes>(ItemTypes.Eyes,eyes));
                }
            }

            //Pull items
            for(int i = 0;i < trial.ExperimentData.Count;i++)
            {
                Eyes eyes = (Eyes)outputPort.GetItem().Value;
            }

            //Stop services
            dataCleaner.stop();
            //Stop ports
            inputPort.Stop();
            outputPort.Stop();

            Console.WriteLine("Cleaner stopped");
            Console.Read();
        }
    }
}
