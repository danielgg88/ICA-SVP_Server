using System;

using ICAPR_RSVP.Misc;
using ICAPR_RSVP.Core;
using ICAPR_RSVP.Test.MockupImplementations;
using System.Collections.Generic;

namespace ICAPR_RSVP
{
    class Program
    {
        static void Main(string[] args)
        {
            //Create inputs
            Broker.Port inputPortEyeTribe = new Broker.PortBlockingInputEyeTribe();
            Broker.Port inputPortSpritz = new Broker.PortNonBlockingInputSpritz("0.0.0.0" , "api" , 8181);
            //Create Outputs
            Broker.Port outputPort = new Broker.PortBlockingOutputCore();

            //************TESTING**********************
            //Broker.Port inputPortEyeTribe = new PortBlockingInputTest();
            //Broker.Port inputPortSpritz = new PortNonBlockingInputTest();
            //Broker.Port outputPort = new PortBlockingOutputTest();
            //*****************************************

            //Create Broker
            Broker.Broker broker = new Broker.BrokerEyeTribeSpritz<String>();
            broker.AddInput(inputPortEyeTribe);
            broker.AddInput(inputPortSpritz);
            broker.AddOutput(outputPort);
            broker.Start();
            //Create core
            Core.Core core = new Core.Core(outputPort);

            //************TESTING**********************
            //TestBrokerDataMerging(outputPort);
            //***************************************** 
            
            //Stop application
            Console.WriteLine("Press any key to close..");
            Console.Read();
            broker.Stop();
        }

        public static void TestBrokerDataMerging(Broker.Port port)
        {
            //Used for testing. Reads items from the broker output and prints them into a log file
            Misc.Utils.FileManager<String> fw = new Misc.Utils.FileManager<String>("test");
            for (int i = 0; i < 50; i++)
                fw.AddToFile(port.GetItem());

            //Create CSV and JSON files
            fw.WriteCsvFile();
            String jsonfile = fw.WriteJsonFile();
            
            //Read JSON files
            Console.WriteLine(fw.getJsonFile(""));
            Console.WriteLine(fw.getJsonFileList());
            fw.Clear();
        }
    }
}
