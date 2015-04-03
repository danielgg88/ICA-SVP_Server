using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICAPR_SVP.Misc;

namespace ICAPR_SVP.DataCleaning
{
    public class FilterSlopeOutliersDetection : Filter
    {

        private Eyes firstPreviousEye;
        private Eyes secondPreviousEye;
        private Eyes currentEye;


        public FilterSlopeOutliersDetection(String name) : base(name)
        {
            
        }

        protected override void OnExecute(Misc.Port input, Misc.Port output)
        {
            Item item = input.GetItem();
            currentEye = (Eyes) item.Value;
            if (firstPreviousEye == null || secondPreviousEye == null)
            {
                swapEyes();
                return;
            }

            //take the avg for the left eye only
            Eye avgNextEye = new Eye();
            avgNextEye.PupilSize = Misc.Calibration.Calibrator.AvgPupilSize[0];
            applyRule(secondPreviousEye.LeftEyeProcessed, firstPreviousEye.LeftEyeProcessed, currentEye.LeftEyeProcessed, avgNextEye);

            //take the avg for the right eye only
            avgNextEye.PupilSize = Misc.Calibration.Calibrator.AvgPupilSize[1];
            applyRule(secondPreviousEye.RightEyeProcessed, firstPreviousEye.RightEyeProcessed, currentEye.RightEyeProcessed, avgNextEye);

            output.PushItem(item);
            swapEyes();
        }

        private void applyRule(Eye secondPreviousEye, Eye firstPreviousEye, Eye currentEye , Eye nextEye)
        {
           if( isOutlier(firstPreviousEye.PupilSize , currentEye.PupilSize ) )
           {
                double sum = secondPreviousEye.PupilSize + 
                    firstPreviousEye.PupilSize +
                    currentEye.PupilSize +
                    nextEye.PupilSize;

                currentEye.PupilSize = Math.Round(sum / 4, 4);
           }
        }

        private bool isOutlier(double previous, double next)
        {
            return Math.Abs(previous - next) > Config.Cleaning.OUTLIER_MAX_CHANGE_RATE_ALLOWED_MM;
        }

        private void swapEyes()
        {
            secondPreviousEye = firstPreviousEye;
            firstPreviousEye = currentEye;
            currentEye = null;
        }

        protected override void OnStop(Misc.Port output)
        {
            throw new NotImplementedException();
        }
    }
}
