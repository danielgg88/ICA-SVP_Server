using ICA_SVP.Misc;
using ICA_SVP.Misc.Calibration;
using System;

namespace ICA_SVP.Network
{
    public interface NetworkDispatcher
    {
        void init(Port outputPort,Calibrator calibrator);
        String dispatchMessage(String msg);
    }
}
