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
            //Broker.Port inputPortEyeTribe = new Broker.PortBlockingInputEyeTribe();
            Broker.NetworkDispatcherRsvpClient dispatcher = new Broker.NetworkDispatcherRsvpClient();
            Broker.Network network = new Broker.Network(dispatcher , "0.0.0.0" , "api/" , 8181);
            Broker.Port inputPortSpritz = new Broker.PortNonBlockingInputSpritz(network);
            //Create Outputs
            Broker.Port outputPort = new Broker.PortBlockingOutputCore();

            //************TESTING**********************
            Broker.Port inputPortEyeTribe = new PortBlockingInputTest();
            //Broker.Port inputPortSpritz = new PortNonBlockingInputTest();
            //Broker.Port outputPort = new PortBlockingOutputTest();
            //*****************************************

            //Create Broker
            Broker.Broker broker = new Broker.BrokerEyeTribeSpritz<String>();
            broker.AddInput(inputPortEyeTribe);
            broker.AddInput(inputPortSpritz);
            broker.AddOutput(outputPort);
            Console.WriteLine(broker.Start());
            

            Misc.Utils.FileManager<String> fm = new Misc.Utils.FileManager<string>("test");
            Broker.Port dataCleanerOutputPort = new Broker.PortBlockingOutputCore();
            dataCleanerOutputPort.Start();
            //Create data cleaning executor
            Executor dataCleaner = new DataCleaningExecutor(fm, outputPort, dataCleanerOutputPort);
            dataCleaner.startInBackground();


            //Create core
//            Core.Filter core = new Core.Filter(outputPort);

            //************TESTING**********************
            //TestBrokerDataMerging(outputPort);
            //***************************************** 
            
            //Stop application
            Console.WriteLine("Press any key to close..");
            Console.Read();
            dataCleaner.stop();
            Console.WriteLine("Calling borker stop");

            broker.Stop();


            Console.Read();
        }

        public static void TestBrokerDataMerging(Broker.Port port)
        {
            //Used for testing. Reads items from the broker output and prints them into a log file
            Misc.Utils.FileManager<String> fw = new Misc.Utils.FileManager<String>("test");
            int i=0;
            while(i < PortNonBlockingInputTest.WORD_COUNT  *  PortNonBlockingInputTest.NUMBER_TRIALS)
            {
                Item item = port.GetItem();
                fw.AddToFile(item);
                if (item.Type == ItemTypes.DisplayItemAndEyes)
                    i++;
            }
            //Create CSV and JSON files
            fw.SaveLog();
        }
    }
}
