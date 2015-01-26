using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using ICAPR_RSVP.Broker;
using ICAPR_RSVP.Misc;


namespace ICAPR_RSVP.Test.MockupImplementations
{
    //Test simulating EyeTribe data input
    public class PortBlockingInputTest : PortBlocking
    {
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
            _workerThread.Join();
            this._isRunning = false;
        }
        #endregion

        private void DoWork()
        {
            //Main testing method
            Random rnd = new Random();
            int timestamp = 0;
            Misc.Eyes eyes;
            Misc.Eye eye = new Eye();
            eye.PupilSize = 5;

            for (int i = 0; i < 1000000; i++)
            {
                timestamp += rnd.Next(1, 1000);
                eyes = new Eyes(timestamp, eye, eye);
                base.PushItem(new Bundle<Eyes>(ItemTypes.Eyes, eyes));
            }
        }
    }
}
