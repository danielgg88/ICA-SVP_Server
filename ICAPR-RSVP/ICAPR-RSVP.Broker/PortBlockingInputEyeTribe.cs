using System;
using TETCSharpClient;
using TETCSharpClient.Data;

using ICAPR_RSVP.Misc;

namespace ICAPR_RSVP.Broker
{
    public class PortBlockingInputEyeTribe : PortBlocking, IConnectionStateListener, IGazeListener
    {
        #region Properties
        public override bool IsRunning
        {
            get { return GazeManager.Instance.IsActivated; }
        }
        public bool IsCalibrated
        {
            get { return GazeManager.Instance.IsCalibrated; }
        }
        #endregion

        #region Public methods

        public void OnConnectionStateChanged(bool IsActivated)
        {
            // The connection state listener detects when the connection to the EyeTribe server changes
            if (!IsActivated)
                GazeManager.Instance.Deactivate();
        }

        public void OnGazeUpdate(GazeData gazeData)
        {
            //Update receieved from EyeDroid. If system calibrated, store data.
            if (this.IsCalibrated)
            {
                //Get left eye data
                Misc.Eye leftEye = new Misc.Eye();
                leftEye.PupilSize = Math.Round(gazeData.LeftEye.PupilSize, 2); ;
                //Get right eye data
                Misc.Eye rightEye = new Misc.Eye();
                rightEye.PupilSize = Math.Round(gazeData.RightEye.PupilSize, 2);
                //Create a new item and push into the port queue
                Eyes eyes = new Eyes(gazeData.TimeStamp, leftEye, rightEye);
                Bundle<Eyes> bundle = new Bundle<Eyes>(ItemTypes.Eyes, eyes);
                PushItem(bundle);
            }
        }
        #endregion

        #region Protected methods
        protected override void OnStart()
        {
            //Start EyeTribe
            if (!this.IsRunning)
                InitEyeTribeClient();
        }

        protected override void OnStop()
        {
            //Stop EyeTribe
            if (this.IsRunning)
                GazeManager.Instance.Deactivate();
        }
        #endregion

        #region Private methods
        private void InitEyeTribeClient()
        {
            // Activate/connect client
            GazeManager.Instance.Activate(GazeManager.ApiVersion.VERSION_1_0, GazeManager.ClientMode.Push);
            GazeManager.Instance.AddConnectionStateListener(this);
            GazeManager.Instance.AddGazeListener(this);
            // Fetch current status
            OnConnectionStateChanged(GazeManager.Instance.IsActivated);
        }
        #endregion
    }
}
