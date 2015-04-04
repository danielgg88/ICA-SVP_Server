using ICAPR_SVP.Misc;
using ICAPR_SVP.Misc.Calibration;
using System;

namespace ICAPR_SVP.Network
{
    public interface NetworkDispatcher
    {
        void init(Port outputPort,Calibrator calibrator);
        String dispatchMessage(String msg);
    }
}
