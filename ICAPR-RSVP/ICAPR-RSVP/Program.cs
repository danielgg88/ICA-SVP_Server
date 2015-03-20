using System;

using ICAPR_RSVP.Misc;
using ICAPR_RSVP.Core;
using ICAPR_RSVP.Test.MockupImplementations;
using System.Collections.Generic;
using System.Threading;

namespace ICAPR_RSVP
{
    class Program
    {
        static void Main(string[] args)
        {
            //************TESTING**********************
            //Broker.Port inputPortEyeTribe = new PortBlockingInputTest();
            //Broker.Port inputPortEyeTribeCalib = new PortBlockingInputTest();
            //Broker.Port inputPortSpritz = new PortNonBlockingInputTest();
            //Broker.Port outputPort = new PortBlockingOutputTest();
            //*****************************************

            //Create inputs
            Broker.Port inputPortEyeTribe = new Broker.PortBlockingInputEyeTribe();
            Broker.Port inputPortEyeTribeCalib = new Broker.PortBlockingInputEyeTribe();

            //Network
            Broker.NetworkDispatcherRsvpClient dispatcher = new Broker.NetworkDispatcherRsvpClient();
            Broker.Network network = new Broker.Network(dispatcher, Config.NET_LOCAL_HOST, Config.NET_SERVER_URL, Config.NET_SERVER_PORT);
            
            //Calibration
            Broker.Calibration.Calibrator.CalibrationCallbacks calibrationCallbacks = new Broker.NetworkCalibratorCallbacks(network);
            Broker.Calibration.Calibrator calibrator = new Broker.Calibration.CalibratorSystem(inputPortEyeTribeCalib, calibrationCallbacks);

            Broker.Port inputPortSpritz = new Broker.PortNonBlockingInputSpritz(network, calibrator);
            
            //Create Outputs
            Broker.Port outputPort = new Broker.PortBlockingOutput();

            //Create Broker
            Broker.Broker broker = new Broker.BrokerEyeTribeRSVP<String>();
            broker.AddInput(inputPortEyeTribe);
            broker.AddInput(inputPortSpritz);
            broker.AddOutput(outputPort);
            Console.WriteLine("Broker started: " + broker.Start());

            //Create file manager
            Misc.Utils.FileManager<String> fm = new Misc.Utils.FileManager<string>("test");

            //Create data cleaning executor
            Broker.Port dataCleanerOutputPort = new Broker.PortBlockingOutput();
            Executor dataCleaner = new DataCleaningExecutor(fm, outputPort, dataCleanerOutputPort);
            dataCleaner.startInBackground();
           
            //************TESTING**********************
            /*
            Thread t = new Thread(() =>
            {
                Misc.Utils.FileManager<String> fw = new Misc.Utils.FileManager<String>("test");
                int i = 0;
                while (i < PortNonBlockingInputTest.WORD_COUNT * PortNonBlockingInputTest.NUMBER_TRIALS)
                {
                    try
                    {
                        Item item = outputPort.GetItem();
                        fw.AddToFile(item);
                        if (item.Type == ItemTypes.DisplayItemAndEyes)
                            i++;
                    }
                    catch (ThreadInterruptedException)
                    {
                        break;
                    }
                }
                //Create CSV and JSON files
                fw.SaveLog();
            });
            t.Start();
            */
            //***************************************** 

            //Stop application
            Console.WriteLine("Press any key to close..");

            Console.Read();
            //t.Interrupt();
            dataCleaner.stop();
            broker.Stop();

            Console.WriteLine("Stopped");
            Console.Read();
        }
    }
}
