using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICAPR_RSVP.Misc;

namespace ICAPR_RSVP.Broker.Calibration
{
    public abstract class Calibrator
    {
        private List<Item> mEyeData = new List<Item>();

        public Port InputPort { get; private set; }
        public double AvgPupilSize { get; private set; }
        public Boolean IsCalibrated { get; private set; }
        public CalibrationCallbacks Callbacks { get; private set; }

        public interface CalibrationCallbacks
        {
            void OnStart();
            void OnStop();
        }

        #region Public

        public void Start()
        {
            InputPort.Start();
            if (InputPort.IsRunning)
            {
                Callbacks.OnStart();
                IsCalibrated = false;
                new Task(() => Calibrate()).Start();
            }
        }

        public Calibrator(Port port, CalibrationCallbacks callbacks)
        {
            InputPort = port;
            Callbacks = callbacks;
        }

        public void Calibrate(){
            AvgPupilSize = 0;
            for(int i = 0; i < Config.CALIB_TOTAL_SAMPLES ; i++){
                this.mEyeData.Add(this.InputPort.GetItem());
            }
            AvgPupilSize = ComputeAvgPupilSize(mEyeData);
            Stop();
        }

        #endregion

        #region Protected
        protected abstract double ComputeAvgPupilSize(List<Item> eyeData);
        #endregion

        #region private
        private void Stop()
        {
            InputPort.Stop();
            if (!InputPort.IsRunning)
                Callbacks.OnStop();
            IsCalibrated = true;
        }
        #endregion
    }
}
