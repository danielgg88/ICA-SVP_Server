using System.Collections.Generic;

namespace ICAPR_SVP.Misc.Calibration
{
    public class CalibratorSystem : Calibrator
    {
        #region Public
        public CalibratorSystem(Port port,CalibrationCallbacks callbacks)
            : base(port,callbacks)
        {
        }
        #endregion

        #region Protected
        protected override double[] ComputeAvgPupilSize(List<Item> items)
        {
            //Calculate the average pupil size (both eyes)
            List<Eyes> eyesData = CleanEyeData(items);
            double sum_left = 0;
            double sum_right = 0;
            foreach(Eyes eyeData in eyesData)
            {
                sum_left += eyeData.LeftEye.PupilSize;
                sum_right += eyeData.RightEye.PupilSize;
            }
            double[] result = { sum_left / eyesData.Count,sum_right / eyesData.Count };
            return result;
        }
        #endregion

        #region Private
        private List<Eyes> CleanEyeData(List<Item> items)
        {
            //Clean sampled puil size data before calculating the baseline pupil size
            List<Eyes> eyesData = new List<Eyes>();

            //TODO Clean
            foreach(Item item in items)
            {
                if(item.Type == ItemTypes.Eyes)
                {
                    eyesData.Add((Eyes)item.Value);
                }
            }
            return eyesData;
        }
        #endregion
    }
}
