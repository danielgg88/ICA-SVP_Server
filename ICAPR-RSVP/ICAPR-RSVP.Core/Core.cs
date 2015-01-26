using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ICAPR_RSVP.Broker;
using ICAPR_RSVP.Misc;

namespace ICAPR_RSVP.Core
{
    public class Core
    {
        private Broker.Port _inputPort; //Input port

        public Core(Broker.Port port)
        {
            //Set core input port
            this._inputPort = port;
        }

        public void TestBrokerDataMerging()
        {
            //Test input merging
            Item item;

            for (int i = 0; i < 50; i++)
            {
                item = this._inputPort.GetItem();
                if (item.Type == ItemTypes.WordAndEyes)
                {
                    WordAndEyes<String> wordAndEyes = (WordAndEyes<String>)item.Value;
                    Queue<Eyes> listEyes = wordAndEyes.Eyes;
                    Word<String> word = wordAndEyes.Word;
                    Console.WriteLine("------------------------------------");
                    Console.WriteLine(word.Timestamp + " Word: " + word.Value + " Duration: " + word.Duration);

                    foreach (Eyes eyes in listEyes)
                    {
                        Console.WriteLine(eyes.Timestamp + " EyeLeft:" + eyes.LeftEye.PupilSize +
                            " EyeRight:" + eyes.RightEye.PupilSize);
                    }
                }
            }
        }
    }
}
