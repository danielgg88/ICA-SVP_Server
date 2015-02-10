using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICAPR_RSVP.Broker
{
    public interface INetworkDispather
    {
        void dispatchMessage(String msg);
    }
}
