using System;

using ICAPR_RSVP.Misc;
using ICAPR_RSVP.Core;

namespace ICAPR_RSVP
{
    class Program
    {
        static void Main(string[] args)
        {
            //Create inputs
            Broker.Port inputPortEyeTribe = new Broker.PortBlockingInputEyeTribe();
            Broker.Port inputPortSpritz = new Broker.PortNonBlockingInputSpritz();
            //Create Outputs
            Broker.Port outputPort = new Broker.PortBlockingOutputCore();
            //Create Broker
            Broker.Broker broker = new Broker.BrokerEyeTribeSpritz();
            broker.AddInput(inputPortEyeTribe);
            broker.AddInput(inputPortSpritz);
            broker.AddOutput(outputPort);
            broker.Start();
            //Create core
            Core.Core core = new Core.Core(outputPort);
            core.Test();
            //Stop application
            Console.WriteLine("Press a key to close");
            Console.Read();
            broker.Stop();
        }
    }
}
