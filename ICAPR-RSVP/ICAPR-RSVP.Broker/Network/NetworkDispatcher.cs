using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICAPR_RSVP.Broker.Calibration;

namespace ICAPR_RSVP.Broker
{
    public interface NetworkDispatcher
    {
        void init(Port outputPort, Calibrator calibrator);
        String dispatchMessage(String msg);
    }
}
