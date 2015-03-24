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
            Console.WriteLine("Test port closed");
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
                eyeRight.PupilSize = rnd.Next(5, 7);

                Misc.Eye eyeLeft = new Eye();
                eyeLeft.PupilSize = rnd.Next(5, 7);

                timestamp = Utils.MilliTimetamp();
                eyes = new Eyes(timestamp, eyeRight, eyeLeft);
                base.PushItem(new Bundle<Eyes>(ItemTypes.Eyes, eyes));
                Thread.Sleep(SLEEP);
            }
        }
    }
}
