using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICAPR_SVP.Broker.Calibration;
using ICAPR_SVP.Misc;

namespace ICAPR_SVP.Network
{
    public interface NetworkDispatcher
    {
        void init(Port outputPort,Calibrator calibrator);
        String dispatchMessage(String msg);
    }
}
