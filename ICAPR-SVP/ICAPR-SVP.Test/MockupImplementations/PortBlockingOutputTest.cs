using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ICAPR_SVP.Broker;

namespace ICAPR_SVP.Test.MockupImplementations
{
    public class PortBlockingOutputTest : PortBlocking
    {
        private bool _isRunning;

        #region Properties
        public override bool IsRunning
        {
            get
            {
                return _isRunning;
            }
        }
        #endregion

        public PortBlockingOutputTest()
            : base()
        {
            //Do nothing...
        }

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
