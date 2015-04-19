using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICAPR_SVP.Misc.Executors;
using ICAPR_SVP.Misc;
using ICAPR_SVP.Misc.Utils;

namespace ICAPR_SVP.MachineLearning
{
    class MLComponent: ExecutorSingleThread
    {
        private ClassifierWrapper _classiferWrapper;

        private IClassificationListener _classificationListener;

        private const int LEFT = 0;
        private const int RIGHT = 0;

        public MLComponent(String pathToExternalModel)
        {
            this._classiferWrapper = new ClassifierWrapper();
            this._classiferWrapper.loadExternalModel(pathToExternalModel);
            this._classiferWrapper.setUpDatasetManually(Misc.Config.WEKA.LABELS, Misc.Config.WEKA.ATTRIBUTES);
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
                String[][] classification = classifySample(displayItem, Misc.Config.EyeTribe.SAMPLING_FREQUENCY, Misc.Config.WEKA.ATTRIBUTE_START_INDEX);

                //Decide how to put the classification in the summaryItem
                //TODO
            }

            this._listOutputPort[0].PushItem(item);
        }
        protected String[][] classifySample(DisplayItemAndEyes<string> items, int SAMPLE_FREQ, int attributesStartIndex)
        {
            List<Eyes> eyes = new List<Eyes>(items.Eyes);
            int iterations = eyes.Count / SAMPLE_FREQ;
            int modulus = eyes.Count % SAMPLE_FREQ;

            int size = (modulus > 0) ? iterations + 1 : iterations;
            String[][] classification = new String[2][];
            classification[LEFT] = new String[size];
            classification[RIGHT] = new String[size];

            for(int i = 0;i < iterations;i++)
            {
                int index_start = i * SAMPLE_FREQ;
                int window = SAMPLE_FREQ;

                double[][] splitItem = getSplitBothEyes(eyes, index_start, SAMPLE_FREQ, window);
                classification[LEFT][i] = _classiferWrapper.getClassificationLabel(splitItem[LEFT], attributesStartIndex);
                classification[RIGHT][i] = _classiferWrapper.getClassificationLabel(splitItem[RIGHT], attributesStartIndex);

                if (this._classificationListener != null)
                    _classificationListener.onClassification(items.DisplayItem.Value, classification[LEFT][i], classification[RIGHT][i], i);
            }

            if(modulus > 0)
            {
                int index_start = iterations * SAMPLE_FREQ;
                int window = eyes.Count - index_start;
                double[][] splitItem = getSplitBothEyes(eyes, index_start, SAMPLE_FREQ, window);
                //fill the rest of the array
                for (int i = 0; i < SAMPLE_FREQ - window; i++)
                {
                    int index = index_start + window + i; 
                    splitItem[LEFT][i] = Misc.Calibration.Calibrator.AvgPupilSize[LEFT];
                    splitItem[RIGHT][i] = Misc.Calibration.Calibrator.AvgPupilSize[RIGHT]; 
                }

                classification[LEFT][size -1] = _classiferWrapper.getClassificationLabel(splitItem[LEFT], attributesStartIndex);
                classification[RIGHT][size -1] = _classiferWrapper.getClassificationLabel(splitItem[RIGHT], attributesStartIndex);

                if (this._classificationListener != null)
                    _classificationListener.onClassification(items.DisplayItem.Value, classification[LEFT][size-1], classification[RIGHT][size-1], size-1);
            }

            return classification;
        }

        /*
         * Makes a deep copy from the eyes list in a temporary
         * double array. Both eyes are copied in the same for loop.
         * 
         * Provide the startindex to specify which portion of the array to copy.
         *
         * 
         */ 
        private double[][] getSplitBothEyes(List<Eyes> eyes, int startIndex, int SAMPLE_FREQ, int window)
        {
            double[][] splitItems = new double[2][];
            splitItems[LEFT] = new double[SAMPLE_FREQ];
            splitItems[RIGHT] = new double[SAMPLE_FREQ];

            for (int i = 0; i < window; i++)
            {
                splitItems[LEFT][i] = eyes[startIndex +i].LeftEye.PupilSize;
                splitItems[RIGHT][i] = eyes[startIndex +i].RightEye.PupilSize;

            }
            return splitItems;
        }

        
    }
}
