using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ICA_SVP.MachineLearning;
using ICA_SVP.Misc.Items;
using System.Collections.Generic;

namespace ICA_SVP.Test
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
                    eyes[i + 60].LeftEyeProcessed.PupilSize,
                    result[0][i],
                    "Left eye copy does not match"
                    );
                Assert.AreEqual(
                    eyes[i + 60].RightEyeProcessed.PupilSize,
                    result[1][i],
                    "Left eye copy does not match"
                    );
            }
        }

        [TestMethod]
        public void testComputeExtraClassificationAttributes()
        {
            Misc.SummaryItem summary = new Misc.SummaryItem();
            summary.SampleAverage[0] = new double[2];
            summary.SampleAverage[1] = new double[2];

            summary.SampleAverageDifference[0] = new double[2];
            summary.SampleAverageDifference[1] = new double[2];

            int[][] ica = new int[2][];
            ica[0] = new int[2];
            ica[1] = new int[2];
            ica[0][0] = 0;
            ica[0][1] = 1;
            ica[1][0] = 0;
            ica[1][1] = 1;

            summary.Ica = ica;

            double[][] samples = new double[2][];

            samples[0] = new double[60];
            samples[1] = new double[60];

            for(int i = 0;i < 60;i++)
            {
                samples[0][i] = i;
                samples[1][i] = i;
            }

            double[][] result = component.computeExtraClassificationAttributes(summary,samples,0);

            Assert.AreEqual(ica[0][0],result[0][0],"Ica is not equal");
            Assert.AreEqual(ica[1][0],result[1][0],"Ica is not equal");

            Assert.AreEqual(summary.SampleAverage[0][0],result[0][1],"Avg is not equal");
            Assert.AreEqual(summary.SampleAverage[1][0],result[1][1],"Avg is not equal");

            Assert.AreEqual(summary.SampleAverageDifference[0][0],result[0][2],"Avg diff is not equal");
            Assert.AreEqual(summary.SampleAverageDifference[1][0],result[1][2],"Avg diff is not equal");

            Assert.AreEqual(29.5,result[0][1]);
            Assert.AreEqual(29.5,result[1][1]);

            Assert.AreEqual(-30,result[0][2]);
            Assert.AreEqual(-30,result[1][2]);

            for(int i = 0;i < 15;i++)
            {
                samples[0][i] = i + 60;
                samples[1][i] = i + 60;
            }

            for(int i = 0;i < 45;i++)
            {
                samples[0][i + 15] = Misc.Config.Calibration.CALIB_DEFAULT_AVG_PUPIL_SIZE;
                samples[1][i + 15] = Misc.Config.Calibration.CALIB_DEFAULT_AVG_PUPIL_SIZE;
            }

            result = component.computeExtraClassificationAttributes(summary,samples,1);

            Assert.AreEqual(ica[0][1],result[0][0],"Ica is not equal");
            Assert.AreEqual(ica[1][1],result[1][0],"Ica is not equal");

            Assert.AreEqual(summary.SampleAverage[0][1],result[0][1],"Avg is not equal");
            Assert.AreEqual(summary.SampleAverage[1][1],result[1][1],"Avg is not equal");

            Assert.AreEqual(summary.SampleAverageDifference[0][1],result[0][2],"Avg diff is not equal");
            Assert.AreEqual(summary.SampleAverageDifference[1][1],result[1][2],"Avg diff is not equal");

            Assert.AreEqual(20.125,result[0][1]);
            Assert.AreEqual(20.125,result[1][1]);

            Assert.AreEqual(31.25,result[0][2]);
            Assert.AreEqual(31.25,result[1][2]);


        }

        private void verifyArrays(int i)
        {

        }

        [TestMethod]
        public void testGetArraysForClassification()
        {

            int iterations = eyes.Count / Misc.Config.EyeTribe.SAMPLING_FREQUENCY;
            int modulus = eyes.Count % Misc.Config.EyeTribe.SAMPLING_FREQUENCY;

            int size = (modulus > 0) ? iterations + 1 : iterations;

            for(int i = 0;i < size;i++)
            {
                double[][] classificationArray = component.getArraysForClassification(eyes,Misc.Config.EyeTribe.SAMPLING_FREQUENCY,i);
                int itemsToCompare = Misc.Config.EyeTribe.SAMPLING_FREQUENCY;

                if(modulus > 0 && i == size - 1)
                    itemsToCompare = modulus;

                for(int j = 0;j < itemsToCompare;j++)
                {
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

            for(int i = 0;i < 10;i++)
            {
                array[0][i] = -1;
                array[1][i] = -1;
            }

            Random r = new Random();

            for(int i = 10;i < array[0].Length;i++)
            {
                array[0][i] = r.NextDouble();
                array[1][i] = r.NextDouble();
            }

            component.addMissingPadding(array,10);

            for(int i = 0;i < 10;i++)
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

            for(int i = 10;i < Misc.Config.EyeTribe.SAMPLING_FREQUENCY;i++)
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
