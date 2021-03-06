﻿using ICA_SVP.Misc;
using ICA_SVP.Misc.Executors;
using System.Collections.Generic;

namespace ICA_SVP.ICA
{
    public class ExecutorICA : ExecutorSingleThread
    {
        // 0 = LEFT
        // 1 = RIGT
        protected IDenoiser denoiser;
        protected double[][] avgCalibrationArray;

        public ExecutorICA()
            : base()
        {
            denoiser = new DenoiserMatlab();
            avgCalibrationArray = new double[2][];
            avgCalibrationArray[0] = new double[Config.ICA.AVG_MOVING_WINDOW_SIZE];
            avgCalibrationArray[1] = new double[Config.ICA.AVG_MOVING_WINDOW_SIZE];
        }

        #region Protected methods

        protected override void Run()
        {
            Item item = this._listInputPort[0].GetItem();
            if(item.Type == ItemTypes.DisplayItemAndEyes)
            {
                populatevgCalibrationArray();
                DisplayItemAndEyes<string> displayItem = (DisplayItemAndEyes<string>)item.Value;

                //Create arrays from eyes
                double[][] eyes_original = new double[2][];
                double[][] eyes_processed = new double[2][];
                initializeDoubleArray(eyes_original,displayItem.Eyes.Count);
                initializeDoubleArray(eyes_processed,displayItem.Eyes.Count);
                createEyeArraysAndSummary(displayItem,eyes_original,eyes_processed);

                //Compute statistical approach
                int[][] binaryEyes = createStatBinaryArray(eyes_original,this.avgCalibrationArray);
                //Compute wavelet transformation (denoise eye data)
                double[][] denoisedEyes = denoiser.denoiseEyes(displayItem.Eyes,eyes_processed);
                //Compute ICA
                computeICAforItem(displayItem,binaryEyes,denoisedEyes,Misc.Config.EyeTribe.SAMPLING_FREQUENCY);
            }
            this._listOutputPort[0].PushItem(item);
        }

        protected void populatevgCalibrationArray()
        {
            //Populate array with Avg values to enable calculating the moving average from the first item
            if(avgCalibrationArray[0][0] != Misc.Calibration.Calibrator.AvgPupilSize[0] &&
                avgCalibrationArray[1][0] != Misc.Calibration.Calibrator.AvgPupilSize[1])
            {
                for(int i = 0;i < avgCalibrationArray[0].Length;i++)
                {
                    avgCalibrationArray[0][i] = Misc.Calibration.Calibrator.AvgPupilSize[0];
                    avgCalibrationArray[1][i] = Misc.Calibration.Calibrator.AvgPupilSize[1];
                }
            }
        }

        protected void computeICAforItem(DisplayItemAndEyes<string> items,int[][] binaryEyes,double[][] denoisedEyes,int SAMPLE_FREQ)
        {
            List<Eyes> eyes = new List<Eyes>(items.Eyes);
            int iterations = eyes.Count / SAMPLE_FREQ;
            int modulus = eyes.Count % SAMPLE_FREQ;

            int size = (modulus > 0) ? iterations + 1 : iterations;
            int[][] ICA = new int[2][];
            ICA[0] = new int[size];
            ICA[1] = new int[size];

            //Compute ICA per iteration (1 second at the time)
            for(int i = 0;i < iterations;i++)
            {
                int index_start = i * SAMPLE_FREQ;
                int window = SAMPLE_FREQ;
                ICA[0][i] = computeICA(binaryEyes[0],denoisedEyes[0],index_start,window);
                ICA[1][i] = computeICA(binaryEyes[1],denoisedEyes[1],index_start,window);
            }

            //Compute ICA for remaining samples (not a complete second)
            if(modulus > 0)
            {
                int index_start = iterations * SAMPLE_FREQ;
                ICA[0][size - 1] = computeICA(binaryEyes[0],denoisedEyes[0],index_start,modulus);
                ICA[1][size - 1] = computeICA(binaryEyes[1],denoisedEyes[1],index_start,modulus);
            }
            items.SummaryItem.Ica = ICA;
        }

        protected int computeICA(int[] binaryEyes,double[] denoisedEyes,int startIndex,int window)
        {
            //Compare binary array and densoised signal. When binary value = 1 -> Check for long values in denoised signal.
            int counter = 0;
            for(int j = startIndex;j < startIndex + window;j++)
            {
                if(binaryEyes[j] == 1 && denoisedEyes[j] > Misc.Config.ICA.SIGNAL_THRESHOLD_LARGE)
                    counter++;
            }
            return counter;
        }

        protected void setupArraysForAvgMovingWindow
            (double[][] eyes_array,int[][] binaryEyes,double[][] avg_and_data,double[][] avg_array,double[][] avg_and_data_sum)
        {
            int eyesSize = eyes_array[0].Length;
            //Initialize binary arrays
            binaryEyes[0] = new int[eyesSize];
            binaryEyes[1] = new int[eyesSize];
            //Concat avg array and eyes data
            avg_and_data[0] = new double[eyesSize + Misc.Config.ICA.AVG_MOVING_WINDOW_SIZE];
            avg_and_data[1] = new double[eyesSize + Misc.Config.ICA.AVG_MOVING_WINDOW_SIZE];
            //Left eye
            avg_array[0].CopyTo(avg_and_data[0],0);
            eyes_array[0].CopyTo(avg_and_data[0],avg_array[0].Length);
            //Right eye
            avg_array[1].CopyTo(avg_and_data[1],0);
            eyes_array[1].CopyTo(avg_and_data[1],avg_array[1].Length);
            //Initialize avg_and_data_sum
            avg_and_data_sum[0] = Misc.Utils.UtilsMath.GetCSum(avg_and_data[0]);
            avg_and_data_sum[1] = Misc.Utils.UtilsMath.GetCSum(avg_and_data[1]);
        }

        protected void computeStdDevBothEyes(double[][] avg_and_data,double[][] avg_and_data_sum,double[] std,int index_end)
        {
            //Compute standard deviation
            std[0] = Misc.Utils.UtilsMath.getStdDev(avg_and_data[0],avg_and_data_sum[0],index_end,Misc.Config.ICA.AVG_MOVING_WINDOW_SIZE);
            std[1] = Misc.Utils.UtilsMath.getStdDev(avg_and_data[1],avg_and_data_sum[1],index_end,Misc.Config.ICA.AVG_MOVING_WINDOW_SIZE);
        }

        protected int[][] createStatBinaryArray(double[][] eyes_array,double[][] avg_array)
        {
            //Initialize arrays
            int[][] binaryEyes = new int[2][];
            double[][] avg_and_data = new double[2][];
            double[][] avg_and_data_sum = new double[2][];
            setupArraysForAvgMovingWindow(eyes_array,binaryEyes,avg_and_data,avg_array,avg_and_data_sum);

            //calculate mean , std, and binary value only for the first item
            double[] std = new double[2];

            //Compute mean and stdDev for both eyes first time
            computeStdDevBothEyes(avg_and_data,avg_and_data_sum,std,Misc.Config.ICA.AVG_MOVING_WINDOW_SIZE);

            //Set 0 to first item
            binaryEyes[0][0] = 0;
            binaryEyes[1][0] = 0;

            //iterate though all of them and compute the binary value for each
            for(int i = Misc.Config.ICA.AVG_MOVING_WINDOW_SIZE + 1;i < avg_and_data[0].Length;i++)
            {
                //Get binary value for LEFT pupil size
                int binary_tmp = getBinaryValue(avg_and_data[0][i - 1],avg_and_data[0][i],std[0]);
                binaryEyes[0][i - Misc.Config.ICA.AVG_MOVING_WINDOW_SIZE] = binary_tmp;
                //Get binary value for RIGHT pupil size
                binary_tmp = getBinaryValue(avg_and_data[1][i - 1],avg_and_data[1][i],std[1]);
                binaryEyes[1][i - Misc.Config.ICA.AVG_MOVING_WINDOW_SIZE] = binary_tmp;

                computeStdDevBothEyes(avg_and_data,avg_and_data_sum,std,i);
            }
            return binaryEyes;
        }

        protected int getBinaryValue(double pupilSize_previuos,double pupilSize_current,double std)
        {
            //Defections from the last observation to the current observation are examined in terms of the size of the 
            //defection and whether it is unusual. A comparison is made on the basis of the standard deviation of the 
            //previous N observations.
            if(pupilSize_current > pupilSize_previuos + std)
                return 1;
            else if(pupilSize_current < pupilSize_previuos - std)
                return -1;
            else
                return 0;
        }

        protected void createEyeArraysAndSummary(DisplayItemAndEyes<string> items,
            double[][] eyes_original,double[][] eyes_processed)
        {
            int i = 0;
            foreach(Eyes eyes in items.Eyes)
            {
                eyes_original[0][i] = eyes.LeftEye.PupilSize;
                eyes_original[1][i] = eyes.RightEye.PupilSize;
                eyes_processed[0][i] = eyes.LeftEyeProcessed.PupilSize;
                eyes_processed[1][i++] = eyes.RightEyeProcessed.PupilSize;

                AddBlinksAndErrorToSummary(items.SummaryItem,eyes);
            }
        }

        #endregion

        #region Public methods

        public static void initializeDoubleArray(double[][] array,int size)
        {
            if(array.Length == 2)
            {
                array[0] = new double[size];
                array[1] = new double[size];
            }
        }

        #endregion

        #region Private methods

        private void AddBlinksAndErrorToSummary(SummaryItem summaryItem,Eyes eyes)
        {
            if(eyes.LeftEye.CleaningFlag == Eye.CleaningFlags.Error)
                summaryItem.ErrorSamples[0]++;
            else if(eyes.LeftEye.CleaningFlag == Eye.CleaningFlags.Blink)
                summaryItem.BlinkSamples[0]++;

            if(eyes.RightEye.CleaningFlag == Eye.CleaningFlags.Error)
                summaryItem.ErrorSamples[1]++;
            else if(eyes.RightEye.CleaningFlag == Eye.CleaningFlags.Blink)
                summaryItem.BlinkSamples[1]++;
        }

        #endregion
    }
}
