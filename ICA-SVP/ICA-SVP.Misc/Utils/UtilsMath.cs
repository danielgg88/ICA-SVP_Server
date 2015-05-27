using System;

namespace ICA_SVP.Misc.Utils
{
    public class UtilsMath
    {
        public static double linear(double x,double x0,double x1,double y0,double y1)
        {
            //InterpolatedPoint = (x,?)
            //Linear interpolation between two points < P1 = (x0,y0) P2 = (x1,y1)>  
            if((x1 - x0) == 0)
                return (y0 + y1) / 2;
            return y0 + (x - x0) * (y1 - y0) / (x1 - x0);
        }

        public static double[] GetCSum(double[] data)
        {
            double[] csum = new double[data.Length];
            double cursum = 0;
            for(int i = 0;i < data.Length;i++)
            {
                cursum += data[i];
                csum[i] = cursum;
            }
            return csum;
        }

        public static double getMean(double[] csum,int window,int index_end)
        {
            if(window == 0 || index_end < window)
                return -1;
            return (csum[index_end] - csum[index_end - window]) / window;
        }

        public static double getVariance(double[] data,double mean,int startingPointer,int window)
        {
            double temp = 0;
            for(int i = startingPointer;i < startingPointer + window;i++)
            {
                double a = data[i];
                temp += (mean - a) * (mean - a);
            }
            return temp / (window - 1);
        }

        public static double getStdDev(double[] data,double[] sum,int index_end,int window)
        {
            double mean = UtilsMath.getMean(sum,window,index_end);
            return Math.Sqrt(getVariance(data,mean,index_end - window + 1,window));
        }

        public static double computeAverage(double[] inputArray,int startIndex,int endIndex)
        {
            double sum = 0;
            for(int i = startIndex;i < endIndex;i++)
            {
                sum += inputArray[i];
            }
            return sum / (endIndex - startIndex);
        }
    }

}
