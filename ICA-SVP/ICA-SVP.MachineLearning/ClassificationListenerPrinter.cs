﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICA_SVP.MachineLearning
{
    public class ClassificationListenerPrinter : IClassificationListener
    {
        public void onClassification(String stimulus,String leftEyeLabel,String rightEyeLabel,int portion)
        {
            Console.WriteLine("WEKA -> " + stimulus + "-> " + leftEyeLabel + ":" + rightEyeLabel + "  ->>>" + portion);
        }
    }
}
