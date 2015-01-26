using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ICAPR_RSVP.Broker;
using ICAPR_RSVP.Misc;
using System.Threading;

namespace ICAPR_RSVP.Test.MockupImplementations
{
    public class PortNonBlockingInputTest : PortNonBlocking
    {
        public static readonly int COUNT = 10000;
        private Thread _workerThread;
        private bool _isRunning;

        #region Properties
        public override bool IsRunning { get { return _isRunning; } }
        #endregion

        public PortNonBlockingInputTest()
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
            Misc.Word<String> word;

            for (int i = 0; i < COUNT; i++)
            {
                timestamp += rnd.Next(1, 1000);
                word = new Word<String>(timestamp, 5000, "test");
                base.PushItem(new Bundle<Word<String>>(ItemTypes.Word, word));
            }
        }
    }
}
