using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICAPR_RSVP.Broker
{
    //TODO Missing implementation
    public class PortNonBlockingInputSpritz: PortNonBlocking
    {
        private bool _isRunning;

        #region Properties
        public override bool IsRunning { get { return _isRunning;  } }
        #endregion

        public PortNonBlockingInputSpritz() : base(){ /*Do nothing*/ }

        #region Protected methods
        protected override void OnStart()
        {
            this._isRunning = true;
        }

        protected override void OnStop()
        {
            this._isRunning = false;
        }
        #endregion
    }
}
