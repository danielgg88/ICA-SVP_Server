using System;

using ICAPR_RSVP.Misc;
using ICAPR_RSVP.Core;
using ICAPR_RSVP.Test.MockupImplementations;

namespace ICAPR_RSVP
{
    class Program
    {
        static void Main(string[] args)
        {
            //Create inputs
            //Broker.Port inputPortEyeTribe = new Broker.PortBlockingInputEyeTribe();
            //Broker.Port inputPortSpritz = new Broker.PortNonBlockingInputSpritz();
            //Create Outputs
            //Broker.Port outputPort = new Broker.PortBlockingOutputCore();

            //TESTING
            Broker.Port inputPortEyeTribe = new PortBlockingInputTest();
            Broker.Port inputPortSpritz = new PortNonBlockingInputTest();
            //Create Outputs
            Broker.Port outputPort = new PortBlockingOutputTest();

            //Create Broker
            Broker.Broker broker = new Broker.BrokerEyeTribeSpritz<String>();
            broker.AddInput(inputPortEyeTribe);
            broker.AddInput(inputPortSpritz);
            broker.AddOutput(outputPort);
            broker.Start();
            //Create core
            Core.Core core = new Core.Core(outputPort);
            //core.TestBrokerDataMerging();
            //Stop application
            Console.WriteLine("Press any key to close..");
            Console.Read();
            broker.Stop();
        }
    }
}
