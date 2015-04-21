using System;
using weka.classifiers;
using weka.core;

namespace ICAPR_SVP.MachineLearning
{
    public class ClassifierWrapper
    {
        private Classifier _classifier;

        Instances dataset;

        /*
         * Used with methods that work with external model.
         * The classifier is created based on the provided model.
         */
        public ClassifierWrapper()
        {
        }

        /*
         * Used with methods that create and train the
         * classifier on the fly. The specific classifier
         * is injected in the constructor.
         */
        public ClassifierWrapper(Classifier classifier)
        {
            this._classifier = classifier;
        }

        #region Train classifier in c#
        public void trainClassifierCrossValidation(String pathToArffFile,int numberOfIterations)
        {
            try
            {
                dataset = new Instances(new java.io.FileReader(pathToArffFile));
                // Assign the prediction attribute to the dataset. This attribute will
                // be used to make a prediction.
                dataset.setClassIndex(dataset.numAttributes() - 1);

                _classifier.buildClassifier(dataset);

                Evaluation eval = new Evaluation(dataset);

                eval.crossValidateModel(_classifier,dataset,numberOfIterations,new java.util.Random(1));

                Console.WriteLine("Classifier was trained successfully...");
            }
            catch(Exception ea)
            {
                Console.WriteLine("Could not train classifier...");
                Console.WriteLine(ea);
            }
        }

        #endregion

        #region Work with external model
        public void loadExternalModel(String pathToModel)
        {
            try
            {
                this._classifier = (Classifier)weka.core.SerializationHelper.read(pathToModel);
                Console.WriteLine("External model was loaded successfuly...");
            }
            catch(Exception e)
            {
                Console.WriteLine("Could not load external model...");
                Console.WriteLine(e);
            }
        }


        public void setUpDatasetManually(String[] _labels,String[] _attributes)
        {
            try
            {
                FastVector attributes = new FastVector();
                for(int i = 0;i < _attributes.Length;i++)
                {
                    attributes.addElement(new weka.core.Attribute(_attributes[i]));
                }

                FastVector labels = new FastVector();
                for(int i = 0;i < _labels.Length;i++)
                {
                    labels.addElement(_labels[i]);
                }

                weka.core.Attribute cls = new weka.core.Attribute("label",labels);
                attributes.addElement(cls);

                dataset = new Instances("TestInstances",attributes,0);
                dataset.setClassIndex(dataset.numAttributes() - 1);
                Console.WriteLine("Dataset was set up successfuly...");
            }
            catch(Exception ea)
            {
                Console.WriteLine("Could not set up dataset...");
                Console.WriteLine(ea);
            }
        }

        #endregion

        /*
         * After the classifier has been set up use this method
         * with all values needed for classification.
         * 
         * Use startIndex to set which specific attributes to use.
         * word,ica.s1,s2..s59 requires a startIndex of 2.
         */
        public Instance classify(double[] values,int startIndex)
        {
            Instance inst = new Instance(dataset.numAttributes());
            inst.setDataset(dataset);

            for(int i = startIndex;i < dataset.numAttributes() - 1;i++)
                inst.setValue(i,values[i - startIndex]);

            double clsLabel = _classifier.classifyInstance(inst);
            inst.setClassValue(clsLabel);

            return inst;
        }

        /*
         * Classify and get the label of the classification.
         * It is assumed that the attribute with the label is at index
         * numAttributes - 1
         */
        public String getClassificationLabel(double[] values,int startIndex)
        {
            Instance instance = classify(values,startIndex);
            return instance.stringValue(dataset.numAttributes() - 1);
        }
    }
}
