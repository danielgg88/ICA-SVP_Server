using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICAPR_SVP.Misc.Utils
{
    public class UtilsMath
    {
        public static double linear(double x, double x0, double x1, double y0, double y1)
        {
            //InterpolatedPoint = (x,?)
            //Linear interpolation between two points < P1 = (x0,y0) P2 = (x1,y1)>  
            if ((x1 - x0) == 0)
                return (y0 + y1) / 2;
            return y0 + (x - x0) * (y1 - y0) / (x1 - x0);
        }

        public static double[] GetCSum(double[] data)
        {
            double[] csum = new double[data.Length];
            double cursum = 0;
            for (int i = 0; i < data.Length; i++)
            {
                cursum += data[i];
                csum[i] = cursum;
            }
            return csum;
        }

        public static double CSumMovingAverage(double[] csum, int period, int ii)
        {
            if (period == 0 || ii <= period)
                return -1;
            return (csum[ii] - csum[ii - period])/period;
        }

        public static double getVariance(double[] data, double mean, int startingPointer)
        {
            double temp = 0;
            for (int i = startingPointer; i < Misc.Config.ICA.AVG_MOVING_WINDOW_SIZE; i++)
            {
                double a = data[i];
                temp += (mean - a) * (mean - a);
            }
            return temp / data.Length;
        }

        public static double getStdDev(double[] data, double mean, int startingPointer)
        {
            return Math.Sqrt(getVariance(data, mean, startingPointer));
        }
    }

}
