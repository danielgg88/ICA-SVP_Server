using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICAPR_SVP.MachineLearning
{
    class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello Java, from C#!");
            classifyTest();
        }

        const int percentSplit = 66;
        public static void classifyTest()
        {
            try
            {
                weka.core.Instances insts = new weka.core.Instances(new java.io.FileReader("iris.arff"));
                insts.setClassIndex(insts.numAttributes() - 1);

                weka.classifiers.Classifier cl = new weka.classifiers.trees.J48();
                Console.WriteLine("Performing " + percentSplit + "% split evaluation.");

                //randomize the order of the instances in the dataset.
                weka.filters.Filter myRandom = new weka.filters.unsupervised.instance.Randomize();
                myRandom.setInputFormat(insts);
                insts = weka.filters.Filter.useFilter(insts, myRandom);

                int trainSize = insts.numInstances() * percentSplit / 100;
                int testSize = insts.numInstances() - trainSize;
                weka.core.Instances train = new weka.core.Instances(insts, 0, trainSize);

                cl.buildClassifier(train);
                int numCorrect = 0;
                for (int i = trainSize; i < insts.numInstances(); i++)
                {
                    weka.core.Instance currentInst = insts.instance(i);
                    double predictedClass = cl.classifyInstance(currentInst);
                    if (predictedClass == insts.instance(i).classValue())
                        numCorrect++;
                }
                Console.WriteLine(numCorrect + " out of " + testSize + " correct (" +
                           (double)((double)numCorrect / (double)testSize * 100.0) + "%)");

               
            }
            catch (java.lang.Exception ex)
            {
                ex.printStackTrace();
            }

            Console.ReadLine();
        }
    }
}
