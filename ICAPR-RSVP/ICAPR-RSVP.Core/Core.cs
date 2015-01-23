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

        public void Test()
        {
            //Testing function
            Item item;
            for (int i = 0; i < 50; i++)
            {
                item = this._inputPort.GetItem();
                if (item.Type == ItemTypes.Eyes)
                {
                    Eyes eyes = (Eyes)item.Value;
                    Console.WriteLine("Timestamp:" + eyes.Timestamp +
                        " Left: " + eyes.LeftEye.PupilSize + " Right: " + eyes.RightEye.PupilSize);
                }
            }
        }
    }
}
