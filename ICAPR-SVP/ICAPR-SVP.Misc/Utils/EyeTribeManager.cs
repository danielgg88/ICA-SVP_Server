using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using TETControls.Cursor;
using TETCSharpClient;

namespace ICAPR_SVP.Misc.Utils
{
    public static class EyeTribeManager
    {
        static CursorControl cursorControl;

        public static void enableCursorControl(bool enabled)
        {
            cursorControl.Enabled = enabled;
        }

        public static void launchEyeTribeCalibration()
        {
            Process.Start(Config.EyeTribe.EYETRIBE_CALIBRATION_EXE);
        }

        public static void launchEyeTribeServer()
        {
            Process.Start(Config.EyeTribe.EYETRIBE_SERVER_EXE,"--framerate=" + Config.EyeTribe.SAMPLING_FREQUENCY);
            cursorControl = new CursorControl(Screen.PrimaryScreen,false,true);

            if(Screen.AllScreens.Length > 1)
                cursorControl = new CursorControl(Screen.AllScreens[1], false, true);
            else
                cursorControl = new CursorControl(Screen.PrimaryScreen, false, true);

            GazeManager.Instance.AddGazeListener(cursorControl);
            Thread.Sleep(3000);
        }
    }
}
