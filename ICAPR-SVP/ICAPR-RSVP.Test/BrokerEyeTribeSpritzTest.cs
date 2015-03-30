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
                    WordAndEyes<String> wordAndEyes = (WordAndEyes<String>)item.Value;
                    Queue<Eyes> listEyes = wordAndEyes.Eyes;
                    Word<String> word = wordAndEyes.Word;

                    if (word != null)
                    {
                        wordTimestamp = word.Timestamp;
                        wordDuration = word.Duration;
                        Assert.IsTrue(wordTimestamp >= oldTimestamp, "Word timestamps not ordered");
                        oldTimestamp = word.Timestamp;
                    }

                    foreach (Eyes eyes in listEyes)
                    {
                        if (word != null)
                        {
                            //Assert ordered timestamps
                            Assert.IsTrue(
                                eyes.Timestamp >= oldTimestamp
                                && eyes.Timestamp < (wordTimestamp + wordDuration), "Eyes timestamp out of word timing");
                        }
                        else
                        {
                            Assert.IsTrue(eyes.Timestamp >= oldTimestamp, "Eyes timestamp not in increasing order");
                        }
                        
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
