﻿using ICAPR_SVP.DataCleaning;
using ICAPR_SVP.Misc;
using ICAPR_SVP.Misc.Executors;
using System;
using TETCSharpClient;

namespace ICAPR_SVP
{
    public class Program
    {
        static void Main(string[] args)
        {
            Misc.Utils.Utils.launchEyeTribeServer();
            GazeManager.Instance.Activate(GazeManager.ApiVersion.VERSION_1_0,GazeManager.ClientMode.Push);
            //Create EyeTribe input ports
            Misc.Port inputPortEyeTribe = new Network.PortBlockingInputEyeTribe();
            Misc.Port inputPortEyeTribeCalib = new Network.PortBlockingInputEyeTribe();
            Run(inputPortEyeTribe,inputPortEyeTribeCalib);
        }

        public static void Run(Port inputPortEyeTribe,Port inputPortEyeTribeCalib)
        {
            //Create data cleaning executor
            Misc.Port dataCleanerOutputPort = new Misc.PortBlockingDefaultImpl();
            ExecutorMultiThreadFilters dataCleaner = new ExecutorMultiThreadFilters(inputPortEyeTribe,dataCleanerOutputPort);
            //Add filters
            dataCleaner.AddFilter(new FilterBlinkDetection("Blinks"));
            dataCleaner.AddFilter(new FilterSlopeOutliersDetection("Outliers"));
            dataCleaner.AddFilter(new FilterScaling("Scaling"));
            dataCleaner.AddFilter(new FilterDenoise("Denoise"));

            //Create svp client network
            Network.NetworkDispatcherSVPClient dispatcher = new Network.NetworkDispatcherSVPClient();
            Network.Network network = new Network.Network(dispatcher,Config.Network.NET_LOCAL_HOST,
                Config.Network.NET_SERVER_URL,Config.Network.NET_SERVER_PORT);

            //Create svp client calibrator
            Misc.Calibration.Calibrator.CalibrationCallbacks calibrationCallbacks =
                new Network.NetworkCalibratorCallbacks(network);
            Misc.Calibration.Calibrator calibrator = new Misc.Calibration.CalibratorSystem(
                inputPortEyeTribeCalib,calibrationCallbacks);

            //Create svp client input port 
            Misc.Port inputPortSVP = new Network.PortNonBlockingInputSVP(network,calibrator);
            //Create broker output port
            Misc.Port brokerOutputPort = new Misc.PortBlockingDefaultImpl();

            //Create Broker
            Misc.Executors.ExecutorSingleThread broker = new Misc.Executors.BrokerEyeTribeSVP<String>();
            broker.AddInput(dataCleanerOutputPort);
            broker.AddInput(inputPortSVP);
            broker.AddOutput(brokerOutputPort);

            //Create file manager
            Misc.Utils.FileManager<String> fm = new Misc.Utils.FileManager<string>(brokerOutputPort);

            //Start ports
            inputPortEyeTribe.Start();
            dataCleanerOutputPort.Start();
            inputPortSVP.Start();
            brokerOutputPort.Start();

            //Start services
            dataCleaner.startInBackground();
            broker.Start();
            fm.Start();

            //Stop services
            Console.WriteLine("Press any key to stop the server..");
            Console.Read();
            Console.WriteLine("Stopping..");
            dataCleaner.stop();
            broker.Stop();
            fm.Stop();

            //Stop ports
            inputPortEyeTribe.Stop();
            inputPortEyeTribeCalib.Stop();
            dataCleanerOutputPort.Stop();
            inputPortSVP.Stop();
            brokerOutputPort.Stop();
            GazeManager.Instance.Deactivate();
            Console.ReadLine();
            Console.WriteLine("Server stopped");
            Console.ReadLine();
        }
    }
}
