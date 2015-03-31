using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICAPR_SVP.Misc;

namespace ICAPR_SVP.Broker.Calibration
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
        protected override double ComputeAvgPupilSize(List<Item> items)
        {
            //Calculate the average pupil size (both eyes)
            List<Eyes> eyesData = CleanEyeData(items);
            double sum = 0;
            foreach(Eyes eyeData in eyesData)
            {
                sum += eyeData.LeftEye.PupilSize;
                sum += eyeData.RightEye.PupilSize;
            }
            return sum / (eyesData.Count * 2);
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
