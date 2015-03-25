using System;

using ICAPR_SVP.Misc;
using ICAPR_SVP.Core;
using ICAPR_SVP.Test.MockupImplementations;
using System.Collections.Generic;
using System.Threading;

namespace ICAPR_SVP
{
    class Testing
    {
        static void Main(string[] args)
        {
            //************TESTING**********************
            Broker.Port inputPortEyeTribe = new PortBlockingInputTest();
            Broker.Port inputPortEyeTribeCalib = new PortBlockingInputTest();
            //Broker.Port inputPortSVP = new PortNonBlockingInputTest();
            //Broker.Port outputPort = new PortBlockingOutputTest();
            //*****************************************

            //Create EyeTribe input ports
            //Broker.Port inputPortEyeTribe = new Broker.PortBlockingInputEyeTribe();
            //Broker.Port inputPortEyeTribeCalib = new Broker.PortBlockingInputEyeTribe();

            //Create svp client network
            Broker.NetworkDispatcherSVPClient dispatcher = new Broker.NetworkDispatcherSVPClient();
            Broker.Network network = new Broker.Network(dispatcher,Config.NET_LOCAL_HOST,
                Config.NET_SERVER_URL,Config.NET_SERVER_PORT);

            //Create svp client calibrator
            Broker.Calibration.Calibrator.CalibrationCallbacks calibrationCallbacks =
                new Broker.NetworkCalibratorCallbacks(network);
            Broker.Calibration.Calibrator calibrator = new Broker.Calibration.CalibratorSystem(
                inputPortEyeTribeCalib,calibrationCallbacks);

            //Create svp client input port 
            Broker.Port inputPortSVP = new Broker.PortNonBlockingInputSVP(network,calibrator);

            //Create broker output port
            Broker.Port brokerOutputPort = new Broker.PortBlockingOutput();

            //Create Broker
            Broker.Broker broker = new Broker.BrokerEyeTribeSVP<String>();
            broker.AddInput(inputPortEyeTribe);
            broker.AddInput(inputPortSVP);
            broker.AddOutput(brokerOutputPort);
            broker.Start();

            //Create file manager
            Misc.Utils.FileManager<String> fm = new Misc.Utils.FileManager<string>();

            //Create data cleaning executor
            Broker.Port dataCleanerOutputPort = new Broker.PortBlockingOutput();
            Executor dataCleaner = new ExecutorDataCleaning(fm,brokerOutputPort,dataCleanerOutputPort);
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
            Console.WriteLine("Press any key to stop the server..");
            Console.Read();
            dataCleaner.stop();
            broker.Stop();

            Console.WriteLine("Server stopped");
            Console.Read();
        }
    }
}
