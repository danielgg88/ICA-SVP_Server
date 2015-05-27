
using ICA_SVP.Test.MockupImplementations;

namespace ICA_SVP
{
    public class Testing
    {
        static void Main(string[] args)
        {
            //Create EyeTribe input ports
            Misc.Config.EyeTribe.ALLOW_CURSOR_CONTROL = false;
            Misc.Port inputPortEyeTribe = new PortBlockingEyeTribeTest(true);
            Misc.Port inputPortEyeTribeCalib = new PortBlockingEyeTribeTest(false);
            Program.Run(inputPortEyeTribe,inputPortEyeTribeCalib);
        }
    }
}
