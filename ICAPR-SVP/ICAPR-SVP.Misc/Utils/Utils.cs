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
        public static long MilliTimetamp()
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

        public static void launchEyeTribeCalibration()
        {
            Process.Start(Config.EyeTribe.EYETRIBE_CALIBRATION_EXE);
        }

        public static void launchEyeTribeServer()
        {
            Process.Start(Config.EyeTribe.EYETRIBE_SERVER_EXE);
            Thread.Sleep(3000);
        }
    }
}
