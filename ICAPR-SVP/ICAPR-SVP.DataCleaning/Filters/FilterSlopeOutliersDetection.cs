using ICAPR_SVP.Misc;
using ICAPR_SVP.Misc.Filters;
using System;

namespace ICAPR_SVP.DataCleaning
{
    public class FilterSlopeOutliersDetection : Filter
    {

        private Eyes firstPreviousEye;
        private Eyes secondPreviousEye;
        private Eyes currentEye;


        public FilterSlopeOutliersDetection(String name)
            : base(name)
        {

        }

        protected override void OnExecute(Misc.Port input,Misc.Port output)
        {
            //take the avg for the left eye only
            Eye avgLeftEye = new Eye();
            avgLeftEye.PupilSize = Misc.Calibration.Calibrator.AvgPupilSize[0];

            //take the avg for the right eye only
            Eye avgRightEye = new Eye();
            avgRightEye.PupilSize = Misc.Calibration.Calibrator.AvgPupilSize[1];

            Item item = input.GetItem();
            currentEye = (Eyes)item.Value;

            if(firstPreviousEye == null || secondPreviousEye == null)
            {
                checkForFirstOutliers(currentEye,avgLeftEye,avgRightEye);
                swapEyes();
                return;
            }

            applyRule(secondPreviousEye.LeftEye,firstPreviousEye.LeftEye,currentEye.LeftEye,avgLeftEye);

            applyRule(secondPreviousEye.RightEye,firstPreviousEye.RightEye,currentEye.RightEye,avgRightEye);

            output.PushItem(item);
            swapEyes();
        }

        private void applyRule(Eye secondPreviousEye,Eye firstPreviousEye,Eye currentEye,Eye nextEye)
        {
            if(isOutlier(secondPreviousEye.PupilSize,currentEye.PupilSize))
            {
                double sum = secondPreviousEye.PupilSize +
                    firstPreviousEye.PupilSize +
                    currentEye.PupilSize +
                    nextEye.PupilSize;

                currentEye.PupilSize = sum / 4;
                currentEye.CleaningFlag = Eye.CleaningFlags.Error;
            }
        }

        private void checkForFirstOutliers(Eyes eyes,Eye avgLeftEye,Eye avgRightEye)
        {
            if(isOutlier(eyes.LeftEye.PupilSize,avgLeftEye.PupilSize))
                eyes.LeftEye.PupilSize = avgLeftEye.PupilSize;

            if(isOutlier(eyes.RightEye.PupilSize,avgRightEye.PupilSize))
                eyes.RightEye.PupilSize = avgRightEye.PupilSize;

        }

        private bool isOutlier(double previous,double next)
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
