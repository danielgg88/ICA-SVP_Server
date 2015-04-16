using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using weka.classifiers;

namespace ICAPR_SVP.MachineLearning
{

    

    class Program
    {
        public const String WEKA_MODEL = @"\output.model";       //Folder to save logs
        public readonly static  String DESKTOP                       //Base path (Desktop)
                = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

        public readonly static String[] attributes = {
                                               "timestamp",
                                               "x",
                                               "y",
                                               "z"
                                               

                                           };

        public readonly static String[] labels = {
                                           "sit",
                                           "stairs",
                                           "walking",
                                       };


        public static void Main(string[] args)
        {
            classifyTest();
        }

        public static void classifyTest()
        {
            Classifier classifier = (Classifier)weka.core.SerializationHelper.read(DESKTOP + WEKA_MODEL);

            ClassifierWrapper wrapper = new ClassifierWrapper(classifier);

            wrapper.init(labels, attributes);

            String result = wrapper.classify(
                "2014-05-04T18:24:39",
                new double[] { -0.010668111, 0.01545645, -0.015901947 });

            Console.WriteLine(result);

            Console.ReadLine();
        }
    }
}
