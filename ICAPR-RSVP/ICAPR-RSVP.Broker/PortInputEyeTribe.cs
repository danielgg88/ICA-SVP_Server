using TETCSharpClient;
using TETCSharpClient.Data;

using ICAPR_RSVP.Misc;

namespace ICAPR_RSVP.Broker
{
    public class PortInputEyeTribe : Port, IConnectionStateListener, IGazeListener
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

        public PortInputEyeTribe() : base(){
               //Do nothing
        }

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
                //Get pupils size from EyeDroid
                Eyes eyes = new Eyes();
                eyes.Timestamp = gazeData.TimeStamp;
                eyes.PupilSizeLeft = gazeData.LeftEye.PupilSize;
                eyes.PupilSizeRight = gazeData.RightEye.PupilSize;
                //Create a new item and push into the port queue
                Bundle<Eyes> bundle = new Bundle<Eyes>(ItemTypes.EyesData, eyes);
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
