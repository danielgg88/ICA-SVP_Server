using ICAPR_SVP.Misc;
using ICAPR_SVP.Misc.Calibration;
using ICAPR_SVP.Misc.Filters;
using System;

namespace ICAPR_SVP.DataCleaning
{
    public class FilterScaling : Filter
    {
        public FilterScaling(String name)
            : base(name)
        {
        }

        #region Protected methods
        protected override void OnExecute(Port input,Port output)
        {
            output.PushItem(Scale(input.GetItem()));
        }

        protected override void OnStop(Port output)
        {
        }
        #endregion

        #region Private methods
        private Item Scale(Item item)
        {
            if(item.Type == ItemTypes.Eyes)
            {
                Eyes eyes = (Eyes)item.Value;
                eyes.LeftEyeProcessed = new Eye();
                eyes.RightEyeProcessed = new Eye();
                eyes.LeftEyeProcessed.PupilSize = eyes.LeftEye.PupilSize - Calibrator.AvgPupilSize[0];
                eyes.RightEyeProcessed.PupilSize = eyes.RightEye.PupilSize - Calibrator.AvgPupilSize[1];
                eyes.LeftEyeProcessed.CleaningFlag = eyes.LeftEye.CleaningFlag;
                eyes.RightEyeProcessed.CleaningFlag = eyes.RightEye.CleaningFlag;
            }
            return item;
        }
        #endregion
    }
}
