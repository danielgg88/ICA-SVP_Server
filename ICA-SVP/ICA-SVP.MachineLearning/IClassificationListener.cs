﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICA_SVP.MachineLearning
{
    public interface IClassificationListener
    {
        void onClassification(String stimulus,String leftEyeLabel,String rightEyeLabel,int portion);
    }
}
