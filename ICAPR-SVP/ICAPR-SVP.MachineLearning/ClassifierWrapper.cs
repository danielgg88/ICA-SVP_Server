using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using weka.classifiers;
using weka.core;

namespace ICAPR_SVP.MachineLearning
{
    class ClassifierWrapper
    {

        private Classifier _classifier;
        private String[] _labels;
        private String[] _attributes;
        Instances dataset;

        public ClassifierWrapper(Classifier classifier)
        {
            this._classifier = classifier;
        }

        public void init(String[] _labels, String[] _attributes)
        {

            try{


                dataset = new Instances(new java.io.FileReader(Program.DESKTOP + @"\data.arff"));
                // Assign the prediction attribute to the dataset. This attribute will
                // be used to make a prediction.
                dataset.setClassIndex(dataset.numAttributes() - 2);
                Console.WriteLine(dataset.numAttributes() + " attributes");
                Console.WriteLine("Classifier initialized successfuly.");

                           }
            catch (Exception ea)
            {
                Console.WriteLine("Could not create classifier...exiting");
                Console.WriteLine(ea);
            }
        }

        public String classify(String timestamp , double[] values)
        {

            String returnString = "yoyo";
            Instance inst = new Instance(1.0, values);
            
            
            inst.setDataset(dataset);
            dataset.add(inst);

            //int index = 0;

            for (int i = 0; i < dataset.numInstances() -1; i++)
            {
                weka.core.Instance currentInst = dataset.instance(i);
                double predictedClass = _classifier.classifyInstance(currentInst);
                //System.Console.WriteLine("Class number: " + index + " predicted as: " + predictedClass);
                //System.Console.WriteLine("Class number: " + index + " predicted as: " + dataset.classAttribute().value((int)predictedClass));
                /*System.Console.WriteLine("ID: " + dataset.instance(i).value(0));
                System.Console.WriteLine(", actual: " + dataset.classAttribute().value((int)dataset.instance(i).classValue()));
                System.Console.WriteLine(", predicted: " + dataset.classAttribute().value((int)dataset.instance(i).classValue()));
                System.Console.WriteLine("ReaL: "+dataset.classAttribute().value((int)predictedClass));
                */

                double[] preds = _classifier.distributionForInstance(dataset.instance(i));
                returnString = dataset.classAttribute().value((int)predictedClass);

            }
            dataset.delete(0);
            return returnString;   
        }
    }
}
