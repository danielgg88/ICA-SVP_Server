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
        public CalibratorSystem(Port port,CalibrationCallbacks callbacks) : base(port,callbacks)
        {
        }
        #endregion

        #region Protected
        protected override double ComputeAvgPupilSize(List<Item> eyeData)
        {
            eyeData = CleanEyeData(eyeData);
            //TODO Calculate avg
            return 0;
        }
        #endregion

        #region Private
        private List<Item> CleanEyeData(List<Item> eyeData)
        {
            //TODO Clean
            return eyeData;
        }
        #endregion
    }
}
