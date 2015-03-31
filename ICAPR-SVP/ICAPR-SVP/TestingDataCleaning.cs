using System;
using System.IO;
using ICAPR_SVP.Misc;
using ICAPR_SVP.Core;
using ICAPR_SVP.Test.MockupImplementations;
using System.Collections.Generic;
using System.Threading;
using Newtonsoft.Json;
using ICAPR_SVP.Misc.Items;

namespace ICAPR_SVP
{
    class TestingDataCleaning
    {
        static void Main(string[] args)
        {
            //Create file manager
            Misc.Utils.FileManager<String> fm = new Misc.Utils.FileManager<string>();
            //Load log file
            String json = fm.getJsonFile("test_20150325093316620");
            Trial<String> trial = JsonConvert.DeserializeObject<Trial<String>>(json);

            //Add ports
            Misc.Port inputPort = new Misc.PortBlockingOutput();
            Misc.Port outputPort = new Misc.PortBlockingOutput();
            inputPort.Start();
            outputPort.Start();
            Executor dataCleaner = new ExecutorDataCleaning(fm,inputPort,outputPort);
            dataCleaner.startInBackground();

            //Push items
            foreach(DisplayItemAndEyes<String> item in trial.ExperimentData)
            {
                inputPort.PushItem(new Bundle<DisplayItemAndEyes<String>>(ItemTypes.DisplayItemAndEyes,item));
            }

            //Pull items
            for(int i = 0;i < trial.ExperimentData.Count;i++)
            {
                DisplayItemAndEyes<String> item = (DisplayItemAndEyes<String>)outputPort.GetItem().Value;
            }

            //Stop cleaner
            dataCleaner.stop();
            Console.WriteLine("Cleaner stopped");
            Console.Read();
        }
    }
}
