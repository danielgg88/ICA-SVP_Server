using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICAPR_SVP.Misc;

namespace ICAPR_SVP.ICA
{
    public interface IDenoiser{
        double[][] denoiseEyes(Queue<Eyes> eyes_queue,double[][] eyes_array);
    }
}
