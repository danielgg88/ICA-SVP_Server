using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using ICAPR_SVP;

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

        public static double linear(double x,double x0,double x1,double y0,double y1)
        {
            //InterpolatedPoint = (x,?)
            //Linear interpolation between two points < P1 = (x0,y0) P2 = (x1,y1)>  
            if((x1 - x0) == 0)
                return (y0 + y1) / 2;
            return y0 + (x - x0) * (y1 - y0) / (x1 - x0);
        }

        public static void launchEyeTribeServer()
        {
            Process.Start(Config.EyeTribe.EYETRIBE_SERVER_EXE);
            Thread.Sleep(3000);
        }
    }
}
