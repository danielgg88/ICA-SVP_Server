using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using ICAPR_SVP.Broker;
using ICAPR_SVP.Misc;
using ICAPR_SVP.Misc.Utils;

namespace ICAPR_SVP.Test.MockupImplementations
{
    //Test simulating EyeTribe data input
    public class PortBlockingEyeTribeTest : PortBlocking
    {
        public static readonly int SLEEP = 5;
        private Thread _workerThread;
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

        public PortBlockingEyeTribeTest()
            : base()
        {

        }

        #region Protected methods
        protected override void OnStart()
        {
            _workerThread = new Thread(DoWork);
            _workerThread.Start();
            this._isRunning = true;
        }

        protected override void OnStop()
        {
            this._isRunning = false;
            _workerThread.Join();
        }
        #endregion

        private void DoWork()
        {
            //Main testing method
            Random rnd = new Random();
            long timestamp = 0;
            Misc.Eyes eyes;

            while(this._isRunning)
            {
                Misc.Eye eyeRight = new Eye();
                eyeRight.PupilSize = rnd.Next(5,7);

                Misc.Eye eyeLeft = new Eye();
                eyeLeft.PupilSize = rnd.Next(5,7);

                timestamp = Utils.MilliTimestamp();
                eyes = new Eyes(timestamp,eyeRight,eyeLeft);
                base.PushItem(new Bundle<Eyes>(ItemTypes.Eyes,eyes));
                Thread.Sleep(SLEEP);
            }
        }
    }
}
