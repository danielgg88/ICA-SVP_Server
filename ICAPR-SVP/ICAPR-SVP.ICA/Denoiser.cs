using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICAPR_SVP.Misc;

namespace ICAPR_SVP.ICA
{

    class Denoiser
    {

        private MLApp.MLApp matlab;

        public Denoiser()
        {
            matlab = new MLApp.MLApp();
            matlab.Visible = 0;
        }

        public double[] denoise(double[] data)
        {
            matlab.PutWorkspaceData("data", "base", data);
            matlab.Execute(@"dwtmode('asym');");
            matlab.Execute(@"[c,l] = wavedec(data," + Config.Matlab.DENOISE_LEVEL + @",'" + Config.Matlab.DENOISE_ALGORITHM + @"');");
            matlab.Execute(@"xd = wden(c, 'minimaxi', 'h', 'sln'," + Config.Matlab.DENOISE_LEVEL + @",'" + Config.Matlab.DENOISE_ALGORITHM + @"');");
            //matlab.Execute(@"plot(xd);");
            object result = null;

            matlab.GetWorkspaceData("xd", "base", out result);

            double[,] resultArray = ((double[,])result);
            double[] returnArray = new double[resultArray.Length];

            for (int i = 0; i < resultArray.Length; i++)
                returnArray[i] = resultArray[0, i];

            return returnArray;
        }


    }
}
