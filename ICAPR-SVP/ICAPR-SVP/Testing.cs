using System;

using ICAPR_SVP.Misc;
using ICAPR_SVP.Core;
using ICAPR_SVP.Test.MockupImplementations;
using System.Collections.Generic;
using System.Threading;

namespace ICAPR_SVP
{
    public class Testing
    {
        static void Main(string[] args)
        {
            //Create EyeTribe input ports
            Misc.Port inputPortEyeTribe = new PortBlockingInputTest();
            Misc.Port inputPortEyeTribeCalib = new PortBlockingInputTest();

            //Create data cleaning executor
            Misc.Port dataCleanerOutputPort = new Misc.PortBlockingOutput();
            Executor dataCleaner = new ExecutorDataCleaning(inputPortEyeTribe,dataCleanerOutputPort);

            //Create svp client network
            Broker.NetworkDispatcherSVPClient dispatcher = new Broker.NetworkDispatcherSVPClient();
            Broker.Network network = new Broker.Network(dispatcher,Config.Network.NET_LOCAL_HOST,
                Config.Network.NET_SERVER_URL,Config.Network.NET_SERVER_PORT);

            //Create svp client calibrator
            Broker.Calibration.Calibrator.CalibrationCallbacks calibrationCallbacks =
                new Broker.NetworkCalibratorCallbacks(network);
            Broker.Calibration.Calibrator calibrator = new Broker.Calibration.CalibratorSystem(
                inputPortEyeTribeCalib,calibrationCallbacks);

            //Create svp client input port 
            Misc.Port inputPortSVP = new Broker.PortNonBlockingInputSVP(network,calibrator);
            //Create broker output port
            Misc.Port brokerOutputPort = new Misc.PortBlockingOutput();

            //Create Broker
            Broker.Broker broker = new Broker.BrokerEyeTribeSVP<String>();
            broker.AddInput(dataCleanerOutputPort);
            broker.AddInput(inputPortSVP);
            broker.AddOutput(brokerOutputPort);

            //Create file manager
            Misc.Utils.FileManager<String> fm = new Misc.Utils.FileManager<string>(brokerOutputPort);

            //Start ports
            inputPortEyeTribe.Start();
            inputPortEyeTribeCalib.Start();
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
            dataCleaner.stop();
            broker.Stop();
            fm.Stop();
            //Stop ports
            inputPortEyeTribe.Stop();
            inputPortEyeTribeCalib.Stop();
            dataCleanerOutputPort.Stop();
            inputPortSVP.Stop();
            brokerOutputPort.Stop();

            Console.WriteLine("Server stopped");
            Console.Read();
        }
    }
}
