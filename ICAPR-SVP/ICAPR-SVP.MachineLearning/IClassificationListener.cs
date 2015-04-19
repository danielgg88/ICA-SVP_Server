using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICAPR_SVP.MachineLearning
{
    interface IClassificationListener
    {
        void onClassification(String stimulus, String leftEyeLabel, String rightEyeLabel, int portion);
    }
}
