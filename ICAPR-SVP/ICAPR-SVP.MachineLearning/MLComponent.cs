using ICAPR_SVP.Misc;
using ICAPR_SVP.Misc.Executors;
using System;
using System.Collections.Generic;

namespace ICAPR_SVP.MachineLearning
{
    public class MLComponent : ExecutorSingleThread
    {
        private ClassifierWrapper _classiferWrapper;

        private IClassificationListener _classificationListener;

        private const int LEFT = 0;
        private const int RIGHT = 1;

        public MLComponent()
        {

        }

        public MLComponent(String pathToExternalModel)
        {
            this._classiferWrapper = new ClassifierWrapper();
            this._classiferWrapper.loadExternalModel(pathToExternalModel);
            this._classiferWrapper.setUpDatasetManually(Misc.Config.WEKA.LABELS,Misc.Config.WEKA.ATTRIBUTES);
        }

        public void setClassificationListener(IClassificationListener listener)
        {
            this._classificationListener = listener;
        }

        public IClassificationListener getClassificationListener()
        {
            return this._classificationListener;
        }

        protected override void Run()
        {
            Item item = this._listInputPort[0].GetItem();
            if(item.Type == ItemTypes.DisplayItemAndEyes)
            {
                DisplayItemAndEyes<string> displayItem = (DisplayItemAndEyes<string>)item.Value;
                displayItem.SummaryItem.Classification =
                    classifySample(displayItem,Misc.Config.EyeTribe.SAMPLING_FREQUENCY,
                    Misc.Config.WEKA.ATTRIBUTE_START_INDEX);
            }

            this._listOutputPort[0].PushItem(item);
        }
        protected String[][] classifySample(DisplayItemAndEyes<string> items,int SAMPLE_FREQ,int attributesStartIndex)
        {
            List<Eyes> eyes = new List<Eyes>(items.Eyes);

            int iterations = eyes.Count / SAMPLE_FREQ;
            int modulus = eyes.Count % SAMPLE_FREQ;
            int size = (modulus > 0) ? iterations + 1 : iterations;

            String[][] classification = new String[2][];
            classification[LEFT] = new String[size];
            classification[RIGHT] = new String[size];

            items.SummaryItem.SampleAverage[LEFT] = new double[size];
            items.SummaryItem.SampleAverage[RIGHT] = new double[size];

            items.SummaryItem.SampleAverageDifference[LEFT] = new double[size];
            items.SummaryItem.SampleAverageDifference[RIGHT] = new double[size];

            for (int i = 0; i < size; i++)
            {
                double[][] sampleArrayForClassification = getArraysForClassification(eyes, SAMPLE_FREQ, i);
                double[][] finalArrayForClassification = computeExtraClassificationAttributes(items.SummaryItem, sampleArrayForClassification, i);
                classifySingleItem(classification, items, finalArrayForClassification, attributesStartIndex, i);
            }

            return classification;
        }


        protected double[][] computeExtraClassificationAttributes(SummaryItem item, double [][] inputArray, int iteration)
        {
            //1st attribute
            int leftIca = item.Ica[LEFT][iteration];
            int rightIca = item.Ica[RIGHT][iteration];

            int splitIndex = Config.EyeTribe.SAMPLING_FREQUENCY / 2;

            double left_eye_left_half_avg = Misc.Utils.UtilsMath.computeAverage(inputArray[LEFT], 0, splitIndex);
            double left_eye_right_half_avg = Misc.Utils.UtilsMath.computeAverage(inputArray[LEFT], splitIndex, Config.EyeTribe.SAMPLING_FREQUENCY);

            double right_eye_left_half_avg = Misc.Utils.UtilsMath.computeAverage(inputArray[RIGHT], 0, splitIndex);
            double right_eye_right_half_avg = Misc.Utils.UtilsMath.computeAverage(inputArray[RIGHT], splitIndex, Config.EyeTribe.SAMPLING_FREQUENCY);

            //2nd attribute
            double leftSampleAverage = (left_eye_left_half_avg + left_eye_right_half_avg) / 2;
            double rightSampleAverage = (right_eye_left_half_avg + right_eye_right_half_avg) / 2;
            item.SampleAverage[LEFT][iteration] = leftSampleAverage;
            item.SampleAverage[RIGHT][iteration] = rightSampleAverage;

            //3rd attribute
            double leftAvgDifference = left_eye_left_half_avg - left_eye_right_half_avg;
            double rightAvgDifference = right_eye_left_half_avg - right_eye_right_half_avg;
            item.SampleAverageDifference[LEFT][iteration] = leftAvgDifference;
            item.SampleAverageDifference[RIGHT][iteration] = rightAvgDifference;

            double[][] classificationArray = new double[2][];
            classificationArray[LEFT] = new double[Config.WEKA.ATTRIBUTES.Length];
            classificationArray[RIGHT] = new double[Config.WEKA.ATTRIBUTES.Length];

            classificationArray[LEFT][0] = leftIca;
            classificationArray[RIGHT][0] = rightIca;

            classificationArray[LEFT][1] = leftSampleAverage;
            classificationArray[RIGHT][1] = rightSampleAverage;

            classificationArray[LEFT][2] = leftAvgDifference;
            classificationArray[RIGHT][2] = rightAvgDifference;

            inputArray[LEFT].CopyTo(classificationArray[LEFT], 3);
            inputArray[RIGHT].CopyTo(classificationArray[RIGHT], 3);

            return classificationArray;
        }

        protected double[][] getArraysForClassification(List<Eyes> eyes, int SAMPLE_FREQ, int iteration)
        {
            int iterations = eyes.Count / SAMPLE_FREQ;
            int modulus = eyes.Count % SAMPLE_FREQ;

            int size = (modulus > 0) ? iterations + 1 : iterations;

            int index_start = iteration * SAMPLE_FREQ;
            double[][] splitItem;

            if (modulus != 0 && iteration == size - 1){
                splitItem = getSplitBothEyes(eyes, index_start, SAMPLE_FREQ, modulus);
                addMissingPadding(splitItem, modulus);
            }
            else
                splitItem = getSplitBothEyes(eyes, index_start, SAMPLE_FREQ, SAMPLE_FREQ);

            return splitItem;
        } 

        /*
         * The array with the eyes to add the padding
         * and the start index. Start padding from start index
         * until array.Length
         */
        protected void addMissingPadding(double[][] array, int startIndex)
        {
            for (int i = startIndex; i < array[LEFT].Length; i++)
            {
                array[LEFT][i] = Misc.Calibration.Calibrator.AvgPupilSize[LEFT];
                array[RIGHT][i] = Misc.Calibration.Calibrator.AvgPupilSize[RIGHT];
            }
        }

        protected void classifySingleItem(String[][] classification,DisplayItemAndEyes<String> displayItem,double[][] splitItems,int attributeStartIndex,int round)
        {
            classification[LEFT][round] = _classiferWrapper.getClassificationLabel(splitItems[LEFT],attributeStartIndex);
            classification[RIGHT][round] = _classiferWrapper.getClassificationLabel(splitItems[RIGHT],attributeStartIndex);

            if (this._classificationListener != null)
                _classificationListener.onClassification(displayItem.DisplayItem.Value, classification[LEFT][round], classification[RIGHT][round], round);
        }

        /*
         * Makes a deep copy from the eyes list in a temporary
         * double array. Both eyes are copied in the same for loop.
         * 
         * Provide the startindex to specify which portion of the array to copy.
         */
        protected double[][] getSplitBothEyes(List<Eyes> eyes,int startIndex,int SAMPLE_FREQ,int window)
        {
            double[][] splitItems = new double[2][];
            splitItems[LEFT] = new double[SAMPLE_FREQ];
            splitItems[RIGHT] = new double[SAMPLE_FREQ];

            for(int i = 0;i < window;i++)
            {
                splitItems[LEFT][i] = eyes[startIndex + i].LeftEyeProcessed.PupilSize;
                splitItems[RIGHT][i] = eyes[startIndex + i].RightEyeProcessed.PupilSize;

            }
            return splitItems;
        }


    }
}
