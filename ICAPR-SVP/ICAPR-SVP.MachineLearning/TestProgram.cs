using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using weka.classifiers;

namespace ICAPR_SVP.MachineLearning
{

    

    class TestProgram
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


        public readonly static double[][] data = new double[30][];

        public static void Main(string[] args)
        {
            //sit
            data[0] = new double[3] { 0.47233468, -3.9176707, -2.6142108 };
            data[1] = new double[3] { 0.046406556, -0.858782, -0.63787985 };
            data[2] = new double[3] { -0.17380759, 3.6097832, 2.7456322 };
            data[3] = new double[3] { -0.16770728, 0.5791203, 0.96054906 };
            data[4] = new double[3] { -0.28900135, 1.795702, 2.417291 };
            data[5] = new double[3] { 0.08864446, -0.3185912, -0.12336447 };
            data[6] = new double[3] { 0.581315, 1.7075713, 0.2560547 };
            data[7] = new double[3] { -0.09121835, 0.34056798, -0.015010262 };
            data[8] = new double[3] { -0.12566063, 0.07287236, 0.026149655 };
            data[9] = new double[3] { -0.029466057, 0.012073129, -0.0016809463 };
            data[10] = new double[3] { -0.07365239, 0.014765322, -0.0039785383 };
            data[11] = new double[3] { 0.007933879, -8.5078477E-4, -0.005431461 };
            data[12] = new double[3] { -0.0186718, 0.01726839, 0.0077036857 };
            data[13] = new double[3] { -0.010668111, 0.01545645, -0.015901947 };
            data[14] = new double[3] { -0.011744166, 0.007223171, -0.020456124 };
            data[15] = new double[3] { -0.024344707, 0.006238878, -0.0073480606 };
            data[16] = new double[3] { -0.0153015135, -0.005868578, 0.013534069 };
            data[17] = new double[3] { -0.020646881, 4.9924257E-4, -0.019600581 };
            data[18] = new double[3] { 0.0033192635, 0.0072121024, -0.006845379 };
            data[19] = new double[3] { 0.0028290749, 0.01952585, 0.0077137947 };
            data[20] = new double[3] { 0.03604231, -0.047949158, 0.031525515 };
            data[21] = new double[3] { 0.031862974, -0.028426379, 0.021313572 };
            data[22] = new double[3] { 0.032048415, 0.014139831, -9.0751646E-4 };
            data[23] = new double[3] { 0.01869123, -0.01156621, -0.0019141197 };
            data[24] = new double[3] { 0.004183507, -0.0015491128, 0.02310648 };
            data[25] = new double[3] { -0.0153109785, 0.003470391, 0.025822926 };
            data[26] = new double[3] { 0.0075129033, 5.116224E-4, -0.009088325 };
            data[27] = new double[3] { -0.031759404, -0.11832555, -0.018378735 };
            data[28] = new double[3] { 0.052361917, -0.07112386, 0.014579868 };
            data[29] = new double[3] { 0.0022031784, -0.055912863, -7.847786E-4 };



            test_withExternalModel();
        }

        public static void test_withExternalModel()
        {
            ClassifierWrapper wrapper = new ClassifierWrapper();

            wrapper.loadExternalModel(DESKTOP + WEKA_MODEL);

            wrapper.setUpDatasetManually(labels, attributes);

            for (int i = 0; i < data.Length; i++)
                Console.WriteLine(wrapper.classify(data[i], 1));

                Console.ReadLine();

        }

        public static void test_withTraining()
        {
            Classifier classifier = new weka.classifiers.lazy.IBk();

            ClassifierWrapper wrapper = new ClassifierWrapper(classifier);

            wrapper.trainClassifierCrossValidation(TestProgram.DESKTOP + @"\data.arff", 10);

            for (int i = 0; i < data.Length; i++)
                Console.WriteLine(wrapper.classify(data[i], 1));

                Console.ReadLine();
        }
    }
}
