using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    }
}
