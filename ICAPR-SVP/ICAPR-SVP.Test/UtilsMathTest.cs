using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ICAPR_SVP.Test
{

    [TestClass]
    public class UtilsMathTest
    {

        private double[] data;

        [TestInitialize]
        public void Initialize()
        {
            data = new double[6];
            data[0] = 34;
            data[1] = 43;
            data[2] = 81;
            data[3] = 106;
            data[4] = 106;
            data[5] = 115;
        }

        [TestMethod]
        public void testGetCSum()
        {
            double[] sum = ICAPR_SVP.Misc.Utils.UtilsMath.GetCSum(data);
            double sumM = 0;
            for(int i = 0;i < data.Length;i++)
            {
                sumM += data[i];
                Assert.AreEqual(sum[i],sumM,0.0001);
            }
        }

        [TestMethod]
        public void testCSumMovingAverage()
        {
            double[] sum = ICAPR_SVP.Misc.Utils.UtilsMath.GetCSum(data);
            double result = ICAPR_SVP.Misc.Utils.UtilsMath.getMean(sum,3,4);
            Assert.AreEqual(97.66666,result,0.0001);

            result = ICAPR_SVP.Misc.Utils.UtilsMath.getMean(sum,3,5);
            Assert.AreEqual(109,result);
        }

        [TestMethod]
        public void testGetVariance()
        {
            double[] sum = ICAPR_SVP.Misc.Utils.UtilsMath.GetCSum(data);
            double result = ICAPR_SVP.Misc.Utils.UtilsMath.getMean(sum,3,5);

            double var = ICAPR_SVP.Misc.Utils.UtilsMath.getVariance(data,result,3,3);

            Assert.AreEqual(27,var);
        }


    }
}
