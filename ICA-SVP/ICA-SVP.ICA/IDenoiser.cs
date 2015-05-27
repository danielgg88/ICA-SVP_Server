using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICA_SVP.Misc;

namespace ICA_SVP.ICA
{
    public interface IDenoiser
    {
        double[][] denoiseEyes(Queue<Eyes> eyes_queue,double[][] eyes_array);
    }
}
