using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICAPR_RSVP.Misc.Utils
{
    public class Utils
    {
        public static long MilliTimeStamp()
        {
            DateTime d1 = new DateTime(1970, 1, 1);
            DateTime d2 = DateTime.UtcNow;
            TimeSpan ts = new TimeSpan(d2.Ticks - d1.Ticks);

            return (long)ts.TotalMilliseconds;
        }
    }
}
