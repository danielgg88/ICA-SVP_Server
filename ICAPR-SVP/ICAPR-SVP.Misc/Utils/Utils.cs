using System;
using System.Diagnostics;
using System.Threading;

namespace ICAPR_SVP.Misc.Utils
{
    public static class Utils
    {
        public const double PX_MM = 3.779528;

        public static long MilliTimestamp()
        {
            DateTime d1 = new DateTime(1970,1,1);
            DateTime d2 = DateTime.UtcNow;
            TimeSpan ts = new TimeSpan(d2.Ticks - d1.Ticks);

            return (long)ts.TotalMilliseconds;
        }

        public static long WinMilliTimestampToUnix(long timestamp)
        {
            DateTime d1 = new DateTime(1970,1,1);
            return timestamp - (long)d1.Millisecond;
        }

        public static double DateTimeToUnixTimestamp(DateTime dateTime)
        {
            return (dateTime - new DateTime(1970,1,1).ToLocalTime()).TotalSeconds;
        }

        public static double PixelsToMM(double pixels)
        {
            return pixels / PX_MM;
        }

        public static void launchEyeTribeCalibration()
        {
            Process.Start(Config.EyeTribe.EYETRIBE_CALIBRATION_EXE);
        }



        public static void launchEyeTribeServer()
        {
            Process.Start(Config.EyeTribe.EYETRIBE_SERVER_EXE,"--framerate=" + Config.EyeTribe.SAMPLING_FREQUENCY);
            Thread.Sleep(3000);
        }
    }
}
