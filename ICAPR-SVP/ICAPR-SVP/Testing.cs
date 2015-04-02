using System;

using ICAPR_SVP.Misc;
using ICAPR_SVP.DataCleaning;
using ICAPR_SVP.Network;
using ICAPR_SVP.Test.MockupImplementations;
using System.Collections.Generic;
using System.Threading;

namespace ICAPR_SVP
{
    public class Testing
    {
        static void Main(string[] args)
        {
            //Create EyeTribe input ports
            Misc.Port inputPortEyeTribe = new PortBlockingEyeTribeTest();
            Misc.Port inputPortEyeTribeCalib = new PortBlockingEyeTribeTest();
            Program.Run(inputPortEyeTribe,inputPortEyeTribeCalib);
        }
    }
}
