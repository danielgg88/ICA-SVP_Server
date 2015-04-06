using ICAPR_SVP.Misc.Executors;
using ICAPR_SVP.Misc;
using System.Collections.Generic;

namespace ICAPR_SVP.ICA
{
    public class ICAExecutor : ExecutorSingleThread
    {
        private Denoiser denoiser;
        private double[] avg_left_array;
        private double[] avg_right_array;
        private double[] denoisedLeftEye;
        private double[] denoisedRightEye;
        private int[] binaryLeftEye;
        private int[] binaryRightEye;

        public ICAExecutor()
            : base()
        {
            denoiser = new Denoiser();
            avg_left_array = new double[Config.ICA.AVG_MOVING_WINDOW_SIZE - 1];
            avg_right_array = new double[Config.ICA.AVG_MOVING_WINDOW_SIZE - 1];
            for(int i = 0 ; i < avg_left_array.Length ; i++)
            {
                avg_left_array[i] = Misc.Calibration.Calibrator.AvgPupilSize[0];
                avg_right_array[i] = Misc.Calibration.Calibrator.AvgPupilSize[1];
            }
        }

        protected override void Run()
        {
            Item item = this._listInputPort[0].GetItem();
            if (item.Type == ItemTypes.DisplayItemAndEyes)
            {
                this.denoise(((DisplayItemAndEyes<string>)item.Value));
                this.computeMovingAvgWindow(((DisplayItemAndEyes<string>)item.Value));
                this.computeICA(((DisplayItemAndEyes<string>)item.Value));
            }

            this._listOutputPort[0].PushItem(item);
        }

        private void computeICA(DisplayItemAndEyes<string> items)
        {
            List<Eyes> eyes = new List<Eyes>(items.Eyes);
            int iterations = eyes.Count / Misc.Config.EyeTribe.SAMPLING_FREQUENCY;
            int modulus = eyes.Count % Misc.Config.EyeTribe.SAMPLING_FREQUENCY;

            int icaSize = (modulus > 0) ? iterations + 1 : iterations;

            int[] ICA = new int[icaSize];

            for (int i = 0; i < iterations; i++)
            {
                ICA[i] = computeICA(i * Misc.Config.EyeTribe.SAMPLING_FREQUENCY, Misc.Config.EyeTribe.SAMPLING_FREQUENCY, true);
                ICA[i] += computeICA(i * Misc.Config.EyeTribe.SAMPLING_FREQUENCY, Misc.Config.EyeTribe.SAMPLING_FREQUENCY, false);
                ICA[i] /= 2;
            }

            if (modulus > 0)
            {
                ICA[iterations] = computeICA(iterations * Misc.Config.EyeTribe.SAMPLING_FREQUENCY, modulus, true);
                ICA[iterations] += computeICA(iterations * Misc.Config.EyeTribe.SAMPLING_FREQUENCY, modulus, false);
                ICA[iterations] /= 2;
            }

            items.SummaryItem = new SummaryItem(ICA);
        }

        private int computeICA(int startIndex, int endIndex, bool left)
        {
            double[] denoisedArray = (left) ? denoisedLeftEye : denoisedRightEye;
            int[] binaryArray = (left) ? binaryLeftEye : binaryRightEye;
            int counter = 0;
            for (int j = startIndex; j < startIndex + endIndex; j++)
            {
                if (binaryArray[j] == 1 && denoisedArray[j] > Misc.Config.ICA.SIGNAL_THRESHOLD_LARGE)
                    counter++;
            }
            return counter;
        }


        private void computeMovingAvgWindow(DisplayItemAndEyes<string> items)
        {
            double[] leftArray_tmp = new double[items.Eyes.Count];
            double[] rightArray_tmp = new double[items.Eyes.Count];
            this.createEyeArrays(items, leftArray_tmp, rightArray_tmp, false);

            binaryLeftEye = new int[items.Eyes.Count];
            binaryRightEye = new int[items.Eyes.Count];

            computeAvgWindowForSingleEye(items, leftArray_tmp, avg_left_array, true);
            computeAvgWindowForSingleEye(items, rightArray_tmp, avg_right_array, false);
        }

        private void computeAvgWindowForSingleEye(DisplayItemAndEyes<string> item, double[] inputData, double[] avg_array, bool left)
        {
            double[] avg_and_data = new double[item.Eyes.Count + Misc.Config.ICA.AVG_MOVING_WINDOW_SIZE - 1];
            avg_array.CopyTo(avg_and_data, 0);

            inputData.CopyTo(avg_and_data, avg_array.Length);

            double[] array_avg_data_sum = Misc.Utils.UtilsMath.GetCSum(avg_and_data);

            //calculate mean , std, and binary value only for the first item
            double mean = Misc.Utils.UtilsMath.CSumMovingAverage(array_avg_data_sum,
                Misc.Config.ICA.AVG_MOVING_WINDOW_SIZE, Misc.Config.ICA.AVG_MOVING_WINDOW_SIZE);

            double std = Misc.Utils.UtilsMath.getStdDev(avg_and_data, mean, 0);

            List<Eyes> eyes = new List<Eyes>(item.Eyes);
            if (left)
                binaryLeftEye[0] = 0;
            else
                binaryRightEye[0] = 0;

            //iterate though all of them and compute the binary value for each
            for (int i = Misc.Config.ICA.AVG_MOVING_WINDOW_SIZE + 1; i < avg_and_data.Length; i++)
            {
                mean = Misc.Utils.UtilsMath.CSumMovingAverage(array_avg_data_sum,
                    Misc.Config.ICA.AVG_MOVING_WINDOW_SIZE, i);

                int binary = 0;
                if (avg_and_data[i] > avg_and_data[i-1] + std)
                    binary = 1;
                else if (avg_and_data[i] < avg_and_data[i-1] - std)
                    binary = -1;
                else
                    binary = 0;

                if (left)
                    binaryLeftEye[i - Misc.Config.ICA.AVG_MOVING_WINDOW_SIZE] = binary;
                else
                    binaryRightEye[i - Misc.Config.ICA.AVG_MOVING_WINDOW_SIZE] = binary;


                std = Misc.Utils.UtilsMath.getStdDev(avg_and_data, mean, i - Misc.Config.ICA.AVG_MOVING_WINDOW_SIZE);
            }
        }


        private void denoise(DisplayItemAndEyes<string> items)
        {
            double[] leftArray = new double[items.Eyes.Count];
            double[] rightArray = new double[items.Eyes.Count];

            this.createEyeArrays(items, leftArray, rightArray, true);

            denoisedLeftEye = denoiser.denoise(leftArray);
            denoisedRightEye = denoiser.denoise(rightArray);

            int i = 0;
            foreach (Eyes eyes in items.Eyes)
            {
                eyes.LeftEyeProcessed.PupilSize = denoisedLeftEye[i];
                eyes.RightEyeProcessed.PupilSize = denoisedRightEye[i++];
            }
        }


        private void createEyeArrays(DisplayItemAndEyes<string> items, double[] leftArray, double[] rightArray, bool processed)
        {
            int i = 0;
            foreach (Eyes eyes in items.Eyes)
            {
                leftArray[i] = (processed)? eyes.LeftEyeProcessed.PupilSize : eyes.LeftEye.PupilSize;
                rightArray[i++] = (processed) ? eyes.RightEyeProcessed.PupilSize : eyes.RightEye.PupilSize;
            }
        }
    }
}
