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

            //************TESTING********************
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

            //************TESTING********************
            //TestBrokerDataMerging(outputPort);
            //***************************************** 
            
            //Stop application
            Console.WriteLine("Press any key to close..");
            Console.Read();
            broker.Stop();
        }

        public static void TestBrokerDataMerging(Broker.Port port)
        {
            //Test input merging
            Item item;

            for (int i = 0; i < 50; i++)
            {
                item = port.GetItem();
                if (item.Type == ItemTypes.WordAndEyes)
                {
                    WordAndEyes<String> wordAndEyes = (WordAndEyes<String>)item.Value;
                    Queue<Eyes> listEyes = wordAndEyes.Eyes;
                    Word<String> word = wordAndEyes.Word;

                    Console.WriteLine("------------------------------------");

                    if (word.Value != null)
                        Console.WriteLine(word.Timestamp + " Word: " + word.Value + " Duration: " + word.Duration);
                    else
                        Console.WriteLine(word.Timestamp + " **Delay**" + word.Value + " Duration: " + word.Duration);

                    foreach (Eyes eyes in listEyes)
                    {
                        Console.WriteLine(eyes.Timestamp + " EyeLeft:" + eyes.LeftEye.PupilSize +
                            " EyeRight:" + eyes.RightEye.PupilSize);
                    }
                    Console.WriteLine("End: " + (word.Timestamp + word.Duration - 1));
                }
            }
        }
    }
}
