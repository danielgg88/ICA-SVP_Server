namespace ICAPR_SVP.Misc
{
    public class PortBlockingDefaultImpl : PortBlocking
    {
        private bool _isRunning;

        #region Properties
        public override bool IsRunning
        {
            get
            {
                return _isRunning;
            }
            set
            {
                _isRunning = value;
            }
        }
        #endregion

        #region Protected methods
        protected override void OnStart()
        {
            _isRunning = true;
        }
        protected override void OnStop()
        {
            _isRunning = false;
        }
        #endregion
    }
}
