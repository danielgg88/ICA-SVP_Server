using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ICAPR_SVP.MachineLearning;
using ICAPR_SVP.Misc.Items;
using System.Collections.Generic;

namespace ICAPR_SVP.Test
{
    [TestClass]
    public class MLComponentTests: MachineLearning.MLComponent
    {
        private List<Misc.Eyes> eyes;
        private MLComponentTests component;

        [TestInitialize]
        public void Initialize()
        {
            setUpEyesArray();
            component = new MLComponentTests();
        }

        [TestMethod]
        public void testGetSplitBothEyes()
        {
            double[][] result = component.getSplitBothEyes(eyes, 0, 60, 60);

            for (int i = 0; i < 60; i++)
            {
                Assert.AreEqual(
                    eyes[i].LeftEye.PupilSize,
                    result[0][i],
                    "Left eye copy does not match"
                    );
                Assert.AreEqual(
                    eyes[i].RightEye.PupilSize,
                    result[1][i],
                    "Left eye copy does not match"
                    );
            }

            result = component.getSplitBothEyes(eyes, 60, 60, 15);

            for (int i = 60; i < 15; i++)
            {
                Assert.AreEqual(
                    eyes[i].LeftEye.PupilSize,
                    result[0][i],
                    "Left eye copy does not match"
                    );
                Assert.AreEqual(
                    eyes[i].RightEye.PupilSize,
                    result[1][i],
                    "Left eye copy does not match"
                    );
            }
        }

        private void setUpEyesArray()
        {
            eyes = new List<Misc.Eyes>(75);
            for (int i = 0; i < 75; i++)
            {
                Misc.Eye left = new Misc.Eye();
                Misc.Eye right = new Misc.Eye();
                Random random = new Random();

                left.PupilSize = random.NextDouble();
                right.PupilSize = random.NextDouble();

                Misc.Eyes item = new Misc.Eyes(0, left, right);

                eyes.Add(item);
            }
        }
    }
}
