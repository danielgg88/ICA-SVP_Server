using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ICAPR_RSVP.Test.MockupImplementations;
using ICAPR_RSVP.Misc;
using System.Collections.Generic;

namespace ICAPR_RSVP.Test
{
    [TestClass]
    public class BrokerEyeTribeSpritzTest
    {
        private Broker.Port inputPortEyeTribe;
        private Broker.Port inputPortSpritz;
        private Broker.Port outputPort;
        private Broker.Broker broker;

        [TestInitialize]
        public void Initialize()
        {
            //Create inputs
            inputPortEyeTribe = new PortBlockingInputTest();
            inputPortSpritz = new PortNonBlockingInputTest();
            //Create Outputs
            outputPort = new PortBlockingOutputTest();
            //Create Broker
            broker = new Broker.BrokerEyeTribeSpritz<String>();
            broker.AddInput(inputPortEyeTribe);
            broker.AddInput(inputPortSpritz);
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

            for (int i = 0; i < PortNonBlockingInputTest.COUNT; i++)
            {
                item = this.outputPort.GetItem();
                if (item.Type == ItemTypes.WordAndEyes)
                {
                    DisplayItemAndEyes<String> wordAndEyes = (DisplayItemAndEyes<String>)item.Value;
                    Queue<Eyes> listEyes = wordAndEyes.Eyes;
                    DisplayItem<String> word = wordAndEyes.Word;

                    wordTimestamp = word.Timestamp;
                    wordDuration = word.Duration;
                    Assert.IsTrue(wordTimestamp >= oldTimestamp, "Word timestamps not ordered");
                    oldTimestamp = wordTimestamp;

                    foreach (Eyes eyes in listEyes)
                    {
                        //Assert ordered timestamps
                        Assert.IsTrue(eyes.Timestamp >= oldTimestamp,
                            "Eyes timestamp not greater or equal than previous");
                        Assert.IsTrue(eyes.Timestamp <= (oldTimestamp + wordDuration - 1), 
                            "Eyes timestamp greater than word timing");
                        oldTimestamp = eyes.Timestamp;
                    }
                }
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            broker.Stop();
        }
    }
}
