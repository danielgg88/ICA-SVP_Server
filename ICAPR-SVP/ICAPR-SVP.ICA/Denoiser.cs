using ICAPR_SVP.Misc;
using System.Collections.Generic;

namespace ICAPR_SVP.ICA
{
    public class Denoiser
    {
        private MLApp.MLApp matlab;

        public Denoiser()
        {
            matlab = new MLApp.MLApp();
            matlab.Visible = 0;
        }

        public double[][] denoiseEyes(Queue<Eyes> eyes_queue,double[][] eyes_array)
        {
            double[][] denoisedEye = new double[2][];
            denoisedEye[0] = denoise(eyes_array[0]);
            denoisedEye[1] = denoise(eyes_array[1]);

            int i = 0;
            foreach(Eyes eyes in eyes_queue)
            {
                eyes.LeftEyeProcessed.PupilSize = denoisedEye[0][i];
                eyes.RightEyeProcessed.PupilSize = denoisedEye[1][i++];
            }
            return denoisedEye;
        }

        private double[] denoise(double[] data)
        {
            matlab.PutWorkspaceData("data","base",data);
            matlab.Execute(@"dwtmode('asym');");
            matlab.Execute(@"[c,l] = wavedec(data," + Config.Matlab.DENOISE_LEVEL + @",'" + Config.Matlab.DENOISE_ALGORITHM + @"');");
            matlab.Execute(@"xd = wden(c, 'minimaxi', 'h', 'sln'," + Config.Matlab.DENOISE_LEVEL + @",'" + Config.Matlab.DENOISE_ALGORITHM + @"');");
            //matlab.Execute(@"plot(xd);");

            object result = null;
            matlab.GetWorkspaceData("xd","base",out result);

            double[,] resultArray = ((double[,])result);
            double[] returnArray = new double[resultArray.Length];

            for(int i = 0;i < resultArray.Length;i++)
                returnArray[i] = resultArray[0,i];

            return returnArray;
        }
    }
}
