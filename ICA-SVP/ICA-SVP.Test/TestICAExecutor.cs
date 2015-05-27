using ICA_SVP.ICA;
using ICA_SVP.Misc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

/*
 *  **IMPORTANT** 
 *  For this test use the following values 
 *  and remember to change the default window in the config file to 2.

 L size	 LP size	 R size	 RP size 
 3.9618	 0.0079	 4.1744	 0.0028
 3.9817	 -0.028	 4.2857	 0.0008
 4.0315	 -0.0092	 4.3017	 0.0225
 3.9371	 0.0585	 4.3555	 0.0699
 4.027	 -0.242	 4.2966	 0.0268
 * 
 */

namespace ICA_SVP.Test
{
    [TestClass]
    public class TestICAExecutor : ExecutorICA
    {

        public const int SIZE = 5;
        public const int WINDOW = Misc.Config.ICA.AVG_MOVING_WINDOW_SIZE;
        public const int AVG_PUPIL_SIZE = 3;

        private Misc.DisplayItemAndEyes<String> displayItem;
        private TestICAExecutor executor;
        double[][] avg = new double[2][];

        public TestICAExecutor()
            : base()
        {
            Assert.AreEqual(2,Config.ICA.AVG_MOVING_WINDOW_SIZE,"Window size is not 2. Test will fail");
        }

        [TestInitialize]
        public void Initialize()
        {
            executor = new TestICAExecutor();
            Misc.DisplayItem<String> word = new DisplayItem<String>(0,0,"test");

            Queue<Eyes> eyes = new Queue<Eyes>();

            Eye leftOriginal_1 = new Eye();
            leftOriginal_1.PupilSize = 3.9618;
            Eye rightOriginal_1 = new Eye();
            rightOriginal_1.PupilSize = 4.1744;
            Eye leftOriginal_2 = new Eye();
            leftOriginal_2.PupilSize = 3.9817;
            Eye rightOriginal_2 = new Eye();
            rightOriginal_2.PupilSize = 4.2857;
            Eye leftOriginal_3 = new Eye();
            leftOriginal_3.PupilSize = 4.0315;
            Eye rightOriginal_3 = new Eye();
            rightOriginal_3.PupilSize = 4.3017;
            Eye leftOriginal_4 = new Eye();
            leftOriginal_4.PupilSize = 3.9371;
            Eye rightOriginal_4 = new Eye();
            rightOriginal_4.PupilSize = 4.3555;
            Eye leftOriginal_5 = new Eye();
            leftOriginal_5.PupilSize = 4.027;
            Eye rightOriginal_5 = new Eye();
            rightOriginal_5.PupilSize = 4.2966;

            Eye leftProcessed_1 = new Eye();
            leftProcessed_1.PupilSize = 0.0079;
            Eye rightProcessed_1 = new Eye();
            rightProcessed_1.PupilSize = 0.0028;
            Eye leftProcessed_2 = new Eye();
            leftProcessed_2.PupilSize = -0.028;
            Eye rightProcessed_2 = new Eye();
            rightProcessed_2.PupilSize = 0.0008;
            Eye leftProcessed_3 = new Eye();
            leftProcessed_3.PupilSize = -0.0092;
            Eye rightProcessed_3 = new Eye();
            rightProcessed_3.PupilSize = 0.0225;
            Eye leftProcessed_4 = new Eye();
            leftProcessed_4.PupilSize = 0.0585;
            Eye rightProcessed_4 = new Eye();
            rightProcessed_4.PupilSize = 0.0699;
            Eye leftProcessed_5 = new Eye();
            leftProcessed_5.PupilSize = -0.242;
            Eye rightProcessed_5 = new Eye();
            rightProcessed_5.PupilSize = 0.0268;

            Eyes eyes1 = new Eyes(0,leftOriginal_1,rightOriginal_1);
            eyes1.LeftEyeProcessed = leftProcessed_1;
            eyes1.RightEyeProcessed = rightProcessed_1;

            Eyes eyes2 = new Eyes(0,leftOriginal_2,rightOriginal_2);
            eyes2.LeftEyeProcessed = leftProcessed_2;
            eyes2.RightEyeProcessed = rightProcessed_2;

            Eyes eyes3 = new Eyes(0,leftOriginal_3,rightOriginal_3);
            eyes3.LeftEyeProcessed = leftProcessed_3;
            eyes3.RightEyeProcessed = rightProcessed_3;

            Eyes eyes4 = new Eyes(0,leftOriginal_4,rightOriginal_4);
            eyes4.LeftEyeProcessed = leftProcessed_4;
            eyes4.RightEyeProcessed = rightProcessed_4;

            Eyes eyes5 = new Eyes(0,leftOriginal_5,rightOriginal_5);
            eyes5.LeftEyeProcessed = leftProcessed_5;
            eyes5.RightEyeProcessed = rightProcessed_5;

            eyes.Enqueue(eyes1);
            eyes.Enqueue(eyes2);
            eyes.Enqueue(eyes3);
            eyes.Enqueue(eyes4);
            eyes.Enqueue(eyes5);

            displayItem = new DisplayItemAndEyes<string>(eyes,word);
            avg[0] = new double[WINDOW];
            avg[1] = new double[WINDOW];

            for(int i = 0;i < WINDOW;i++)
            {
                avg[0][i] = AVG_PUPIL_SIZE;
                avg[1][i] = AVG_PUPIL_SIZE;
            }
        }

        [TestCleanup]
        public void CleanUp()
        {

        }

        [TestMethod]
        public void testGetBinaryValue()
        {
            int result = executor.getBinaryValue(5,7,1);
            Assert.AreEqual(1,result);

            result = executor.getBinaryValue(5,5,5);
            Assert.AreEqual(0,result);

            result = executor.getBinaryValue(5,3,1);
            Assert.AreEqual(-1,result);
        }

        [TestMethod]
        public void testCreateEyeArrays()
        {
            double[][] eyes_original = new double[2][];
            double[][] eyes_processed = new double[2][];
            initializeDoubleArray(eyes_original,displayItem.Eyes.Count);
            initializeDoubleArray(eyes_processed,displayItem.Eyes.Count);
            executor.createEyeArraysAndSummary(displayItem,eyes_original,eyes_processed);

            Assert.AreEqual(SIZE,eyes_processed[0].Length);
            int i = 0;
            foreach(Eyes eye in displayItem.Eyes)
            {
                Assert.AreEqual(eye.LeftEyeProcessed.PupilSize,eyes_processed[0][i]);
                Assert.AreEqual(eye.RightEyeProcessed.PupilSize,eyes_processed[1][i++]);
            }

            Assert.AreEqual(SIZE,eyes_original[0].Length);
            i = 0;
            foreach(Eyes eye in displayItem.Eyes)
            {
                Assert.AreEqual(eye.LeftEye.PupilSize,eyes_original[0][i]);
                Assert.AreEqual(eye.RightEye.PupilSize,eyes_original[1][i++]);
            }
        }

        [TestMethod]
        public void testSetupArraysForAvgMovingWindow()
        {
            int[][] binaryEyes = new int[2][];
            double[][] avg_and_data = new double[2][];
            double[][] avg_and_data_sum = new double[2][];

            double[][] eyes_original = new double[2][];
            double[][] eyes_processed = new double[2][];
            initializeDoubleArray(eyes_original,displayItem.Eyes.Count);
            initializeDoubleArray(eyes_processed,displayItem.Eyes.Count);
            executor.createEyeArraysAndSummary(displayItem,eyes_original,eyes_processed);

            executor.setupArraysForAvgMovingWindow(eyes_original,binaryEyes,avg_and_data,avg,avg_and_data_sum);

            Assert.AreEqual(eyes_original[0].Length,binaryEyes[0].Length,"Binary");
            Assert.AreEqual(eyes_original[0].Length + avg[0].Length,avg_and_data[0].Length,"Avg and data");
            Assert.AreEqual(eyes_original[0].Length + avg[0].Length,avg_and_data_sum[0].Length,"Avg and data sum");
        }


        [TestMethod]
        public void testComputeStdBothEyes()
        {
            int[][] binaryEyes = new int[2][];
            double[][] avg_and_data = new double[2][];
            double[][] avg_and_data_sum = new double[2][];

            double[][] eyes_original = new double[2][];
            double[][] eyes_processed = new double[2][];
            initializeDoubleArray(eyes_original,displayItem.Eyes.Count);
            initializeDoubleArray(eyes_processed,displayItem.Eyes.Count);

            executor.createEyeArraysAndSummary(displayItem,eyes_original,eyes_processed);

            executor.setupArraysForAvgMovingWindow(eyes_original,binaryEyes,avg_and_data,avg,avg_and_data_sum);

            //get for first item after avg padding. index + WINDOW + 1
            double[] std = new double[2];
            executor.computeStdDevBothEyes(avg_and_data,avg_and_data_sum,std,WINDOW);

            Assert.AreEqual(0.6800,std[0],0.001);
            Assert.AreEqual(0.8304,std[1],0.001);

            executor.computeStdDevBothEyes(avg_and_data,avg_and_data_sum,std,WINDOW + 1);

            Assert.AreEqual(0.0140,std[0],0.001);
            Assert.AreEqual(0.0787,std[1],0.001);
        }

        [TestMethod]
        public void testCreateStatBinaryArray()
        {
            double[][] eyes_original = new double[2][];
            double[][] eyes_processed = new double[2][];
            initializeDoubleArray(eyes_original,displayItem.Eyes.Count);
            initializeDoubleArray(eyes_processed,displayItem.Eyes.Count);
            executor.createEyeArraysAndSummary(displayItem,eyes_original,eyes_processed);

            int[][] binary = executor.createStatBinaryArray(eyes_original,avg);

            Assert.AreEqual(0,binary[0][0]);
            Assert.AreEqual(0,binary[0][1]);
            Assert.AreEqual(1,binary[0][2]);
            Assert.AreEqual(-1,binary[0][3]);
            Assert.AreEqual(1,binary[0][4]);
        }

        [TestMethod]
        public void testComputeICA()
        {
            double[][] eyes_original = new double[2][];
            double[][] eyes_processed = new double[2][];
            initializeDoubleArray(eyes_original,displayItem.Eyes.Count);
            initializeDoubleArray(eyes_processed,displayItem.Eyes.Count);
            executor.createEyeArraysAndSummary(displayItem,eyes_original,eyes_processed);

            int[][] binary = executor.createStatBinaryArray(eyes_original,avg);

            double[] denoised = { 32456,434,1.2,333,0 };

            int ICA = executor.computeICA(binary[0],denoised,0,displayItem.Eyes.Count);

            Assert.AreEqual(1,ICA);
        }


        [TestMethod]
        public void testComputeICAForItem()
        {
            double[][] eyes_original = new double[2][];
            double[][] eyes_processed = new double[2][];
            initializeDoubleArray(eyes_original,displayItem.Eyes.Count);
            initializeDoubleArray(eyes_processed,displayItem.Eyes.Count);
            executor.createEyeArraysAndSummary(displayItem,eyes_original,eyes_processed);

            int[][] binary = executor.createStatBinaryArray(eyes_original,avg);

            double[][] denoised = new double[2][]{ 
                new double[]{ 32456, 434, 1.2, 333, 0 },
                new double[]{ 0,0,0,1,0} };

            executor.computeICAforItem(displayItem,binary,denoised,2);

            SummaryItem summary = displayItem.SummaryItem;

            Assert.AreEqual(3,summary.Ica[0].Length);
            Assert.AreEqual(0,summary.Ica[0][0]);
            Assert.AreEqual(1,summary.Ica[0][1]);
            Assert.AreEqual(0,summary.Ica[0][2]);
        }
    }
}
