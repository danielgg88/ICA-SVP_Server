using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ICAPR_SVP.Test.MockupImplementations;
using ICAPR_SVP.Misc;
using System.Collections.Generic;

namespace ICAPR_SVP.Test
{
    [TestClass]
    public class BrokerEyeTribeSVPTest
    {
        private Broker.Port inputPortEyeTribe;
        private Broker.Port inputPortSVP;
        private Broker.Port outputPort;
        private Broker.Broker broker;

        [TestInitialize]
        public void Initialize()
        {
            //Create inputs
            inputPortEyeTribe = new PortBlockingInputTest();
            inputPortSVP = new PortNonBlockingInputTest();
            //Create Outputs
            outputPort = new PortBlockingOutputTest();
            //Create Broker
            broker = new Broker.BrokerEyeTribeSVP<String>();
            broker.AddInput(inputPortEyeTribe);
            broker.AddInput(inputPortSVP);
            broker.AddOutput(outputPort);
            broker.Start();
        }

        [TestMethod]
        public void TestBrokerDataMerging()
        {
            //Test input merging
            Item item;
            long wordTimestamp = 0;
            long wordDuration = 0;
            long oldTimestamp = 0;

            for(int i = 0;i < PortNonBlockingInputTest.WORD_COUNT * PortNonBlockingInputTest.NUMBER_TRIALS;i++)
            {
                item = this.outputPort.GetItem();
                if(item.Type == ItemTypes.DisplayItemAndEyes)
                {
                    DisplayItemAndEyes<String> wordAndEyes = (DisplayItemAndEyes<String>)item.Value;
                    Queue<Eyes> listEyes = wordAndEyes.EyesOriginal;
                    DisplayItem<String> word = wordAndEyes.DisplayItem;

                    wordTimestamp = word.Timestamp;
                    wordDuration = word.Duration;
                    Assert.IsTrue(wordTimestamp >= oldTimestamp,"Word timestamps not ordered");
                    oldTimestamp = wordTimestamp;

                    foreach(Eyes eyes in listEyes)
                    {
                        //Assert ordered timestamps
                        Assert.IsTrue(eyes.Timestamp >= oldTimestamp,
                            "Eyes timestamp not greater or equal than previous " + eyes.Timestamp + " " + oldTimestamp);
                        Assert.IsTrue(eyes.Timestamp <= (oldTimestamp + wordDuration - 1),
                            "Eyes timestamp greater than word timing");
                        oldTimestamp = eyes.Timestamp;
                    }
                }
                else
                {
                    Assert.IsTrue(item.Type == ItemTypes.Config || item.Type == ItemTypes.EndOfTrial);
                    i--;
                }
            }
            broker.Stop();
        }
    }
}
