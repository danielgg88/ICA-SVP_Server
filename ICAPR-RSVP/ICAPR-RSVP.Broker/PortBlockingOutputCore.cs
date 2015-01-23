namespace ICAPR_RSVP.Broker
{
    public class PortBlockingOutputCore : PortBlocking
    {
        private bool _isRunning;

        #region Properties
        public override bool IsRunning
        {
            get { return _isRunning; }
        }
        #endregion

        #region Public methods
        public PortBlockingOutputCore() : base()
        {
               //Do nothing
        }
        #endregion

        #region Protected methods
        protected override void OnStart() { _isRunning = true; }
        protected override void OnStop() { _isRunning = false; }
        #endregion
    }
}
