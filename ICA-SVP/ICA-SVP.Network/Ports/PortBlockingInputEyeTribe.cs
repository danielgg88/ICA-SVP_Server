using System;
using TETCSharpClient;
using TETCSharpClient.Data;
using System.Diagnostics;
using System.Threading;
using ICA_SVP.Misc;
using ICA_SVP.Misc.Utils;

namespace ICA_SVP.Network
{
    public class PortBlockingInputEyeTribe : PortBlocking,IConnectionStateListener,IGazeListener
    {

        private bool isRunning;

        public PortBlockingInputEyeTribe()
        {
            this.IsRunning = false;
        }

        #region Properties
        public override bool IsRunning
        {
            get
            {
                return isRunning;
            }
            set
            {
                isRunning = value;
            }
        }
        public bool IsCalibrated
        {
            get
            {
                return GazeManager.Instance.IsCalibrated;
            }
        }
        #endregion

        #region Public methods

        public void OnConnectionStateChanged(bool IsActivated)
        {
            // The connection state listener detects when the connection to the EyeTribe server changes
            if(!IsActivated)
                this.Stop();
        }

        public void OnGazeUpdate(GazeData gazeData)
        {
            //Update receieved from EyeDroid. If system calibrated, store data.
            if(this.IsCalibrated)
            {
                //Get left eye data
                Misc.Eye leftEye = new Misc.Eye();
                leftEye.PupilSize = Misc.Utils.Utils.PixelsToMM(gazeData.LeftEye.PupilSize);
                //Get right eye data
                Misc.Eye rightEye = new Misc.Eye();
                rightEye.PupilSize = Misc.Utils.Utils.PixelsToMM(gazeData.RightEye.PupilSize);
                //Create a new item and push into the port queue
                Eyes eyes = new Eyes(Utils.MilliTimestamp(),leftEye,rightEye);
                //Eyes eyes = new Eyes(Utils.WinMilliTimestampToUnix(gazeData.TimeStamp),leftEye,rightEye);
                Bundle<Eyes> bundle = new Bundle<Eyes>(ItemTypes.Eyes,eyes);
                PushItem(bundle);
            }
        }
        #endregion

        #region Protected methods
        protected override void OnStart()
        {
            if(!this.IsRunning)
            {
                // Activate/connect client
                GazeManager.Instance.AddConnectionStateListener(this);
                GazeManager.Instance.AddGazeListener(this);
                // Fetch current status
                OnConnectionStateChanged(GazeManager.Instance.IsActivated);
                this.IsRunning = true;
            }
        }

        protected override void OnStop()
        {
            GazeManager.Instance.RemoveConnectionStateListener(this);
            GazeManager.Instance.RemoveGazeListener(this);
            this.IsRunning = false;
        }
        #endregion

    }
}
