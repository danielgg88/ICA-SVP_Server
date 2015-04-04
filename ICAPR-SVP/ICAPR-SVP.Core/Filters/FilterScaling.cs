using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICAPR_SVP.Misc;
using ICAPR_SVP.Misc.Calibration;

namespace ICAPR_SVP.DataCleaning
{
    public class FilterScaling : Filter
    {
        public FilterScaling(String name) : base(name)
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
            if(item.Type == ItemTypes.Eyes){
                Eyes eyes = (Eyes)item.Value;
                eyes.LeftEyeProcessed = new Eye();
                eyes.RightEyeProcessed = new Eye();
                eyes.LeftEyeProcessed.PupilSize = eyes.LeftEye.PupilSize - Calibrator.AvgPupilSize[0];
                eyes.RightEyeProcessed.PupilSize = eyes.LeftEye.PupilSize - Calibrator.AvgPupilSize[1];
                eyes.LeftEyeProcessed.CleaningFlag = eyes.LeftEye.CleaningFlag;
                eyes.RightEyeProcessed.CleaningFlag = eyes.RightEye.CleaningFlag;
            }
            return item;
        }
        #endregion
    }
}
