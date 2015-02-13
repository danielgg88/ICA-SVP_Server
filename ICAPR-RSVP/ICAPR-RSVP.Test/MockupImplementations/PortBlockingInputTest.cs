using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using ICAPR_RSVP.Broker;
using ICAPR_RSVP.Misc;
using ICAPR_RSVP.Misc.Utils;

namespace ICAPR_RSVP.Test.MockupImplementations
{
    //Test simulating EyeTribe data input
    public class PortBlockingInputTest : PortBlocking
    {
        public static readonly int SLEEP = 5;
        private Thread _workerThread;
        private bool _isRunning;

        #region Properties
        public override bool IsRunning { get { return _isRunning; } }
        #endregion

        public PortBlockingInputTest()
            : base()
        {
            _workerThread = new Thread(DoWork);
        }

        #region Protected methods
        protected override void OnStart()
        {
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
                eyeRight.PupilSize = rnd.Next(5, 10);

                Misc.Eye eyeLeft = new Eye();
                eyeLeft.PupilSize = rnd.Next(5, 10);

                timestamp = Utils.MilliTimeStamp();
                eyes = new Eyes(timestamp, eyeRight, eyeLeft);
                base.PushItem(new Bundle<Eyes>(ItemTypes.Eyes, eyes));
                Thread.Sleep(SLEEP);
            }
        }
    }
}
