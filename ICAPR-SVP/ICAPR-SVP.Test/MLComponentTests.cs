using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ICAPR_SVP.MachineLearning;
using ICAPR_SVP.Misc.Items;
using System.Collections.Generic;

namespace ICAPR_SVP.Test
{
    [TestClass]
    public class MLComponentTests : MachineLearning.MLComponent
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
            double[][] result = component.getSplitBothEyes(eyes,0,60,60);

            for(int i = 0;i < 60;i++)
            {
                Assert.AreEqual(
                    eyes[i].LeftEyeProcessed.PupilSize,
                    result[0][i],
                    "Left eye copy does not match"
                    );
                Assert.AreEqual(
                    eyes[i].RightEyeProcessed.PupilSize,
                    result[1][i],
                    "Left eye copy does not match"
                    );
            }

            result = component.getSplitBothEyes(eyes,60,60,15);

            for(int i = 0;i < 15;i++)
            {
                Assert.AreEqual(
                    eyes[i+60].LeftEyeProcessed.PupilSize,
                    result[0][i],
                    "Left eye copy does not match"
                    );
                Assert.AreEqual(
                    eyes[i+60].RightEyeProcessed.PupilSize,
                    result[1][i],
                    "Left eye copy does not match"
                    );
            }
        }

        [TestMethod]
        public void testGetArraysForClassification()
        {

            int iterations = eyes.Count / Misc.Config.EyeTribe.SAMPLING_FREQUENCY;
            int modulus = eyes.Count % Misc.Config.EyeTribe.SAMPLING_FREQUENCY;

            int size = (modulus > 0) ? iterations + 1 : iterations;

            for (int i = 0; i < size; i++)
            {
                double[][] classificationArray = component.getArraysForClassification(eyes, Misc.Config.EyeTribe.SAMPLING_FREQUENCY, i);
                int itemsToCompare = Misc.Config.EyeTribe.SAMPLING_FREQUENCY;

                if (modulus > 0 && i == size - 1)
                    itemsToCompare = modulus;

                for( int j = 0 ; j < itemsToCompare ; j++ ){
                    int start_index = i * Misc.Config.EyeTribe.SAMPLING_FREQUENCY + j;
                    Assert.AreEqual(
                        eyes[start_index].LeftEyeProcessed.PupilSize,
                        classificationArray[0][j],
                        "Left array was not created succussfuly"
                        );

                    Assert.AreEqual(
                        eyes[start_index].RightEyeProcessed.PupilSize,
                        classificationArray[1][j],
                        "Right array was not created succussfuly"
                        );
                }
            }
        }

        [TestMethod]
        public void testAddMissingPadding()
        {

            double[][] array = new double[2][];
            array[0] = new double[Misc.Config.EyeTribe.SAMPLING_FREQUENCY];
            array[1] = new double[Misc.Config.EyeTribe.SAMPLING_FREQUENCY];

            for (int i = 0; i < 10; i++)
            {
                array[0][i] = -1;
                array[1][i] = -1;
            }

            Random r = new Random();

            for (int i = 10; i < array[0].Length; i++)
            {
                array[0][i] = r.NextDouble();
                array[1][i] = r.NextDouble();
            }

            component.addMissingPadding(array, 10);

            for (int i = 0; i < 10; i++)
            {
                Assert.AreEqual(
                    -1,
                    array[0][i],
                    "First part of the array was modified");
                Assert.AreEqual(
                    -1,
                    array[1][i],
                    "First part of the array was modified");
            }

            for (int i = 10; i < Misc.Config.EyeTribe.SAMPLING_FREQUENCY; i++)
            {
                Assert.AreEqual(
                    Misc.Config.Calibration.CALIB_DEFAULT_AVG_PUPIL_SIZE,
                    array[0][i],
                    "Last part of the array was modified");
                Assert.AreEqual(
                    Misc.Config.Calibration.CALIB_DEFAULT_AVG_PUPIL_SIZE,
                    array[1][i],
                    "Last part of the array was modified");

            }


        }

        private void setUpEyesArray()
        {
            eyes = new List<Misc.Eyes>(75);
            Random random = new Random();
            for(int i = 0;i < 75;i++)
            {
                Misc.Eye left = new Misc.Eye();
                Misc.Eye right = new Misc.Eye();

                left.PupilSize = random.NextDouble();
                right.PupilSize = random.NextDouble();

                Misc.Eyes item = new Misc.Eyes(0,null,null);

                item.LeftEyeProcessed = left;
                item.RightEyeProcessed = right;

                eyes.Add(item);
            }
        }
    }
}
