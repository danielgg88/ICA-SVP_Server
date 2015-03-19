using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ICAPR_RSVP.Broker;
using ICAPR_RSVP.Misc;
using ICAPR_RSVP.Misc.Utils;
using System.Threading;

namespace ICAPR_RSVP.Test.MockupImplementations
{
    public class PortNonBlockingInputTest : PortNonBlocking
    {
        public static readonly int NUMBER_TRIALS = 1;
        public static readonly int WORD_COUNT = 10;
        public static readonly int SLEEP = 10;
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
            this._isRunning = false;
            _workerThread.Join();
        }
        #endregion

        private void DoWork()
        {
            //Main testing method
            Random rnd = new Random();
            Misc.DisplayItem<String> word;
            long timestamp = 0;
            int j = 0;
            while( _isRunning && j++ < NUMBER_TRIALS)
            {
                ExperimentConfig trial = new ExperimentConfig();
                trial.Trial = "trial " + j;
                trial.FileName = "file_name";
                trial.ItemTime = "item_time";
                trial.DelayTime = "delay_time";
                trial.UserName = "name";
                trial.UserAge = "age";
                trial.FontSize = "font_size";
                trial.FontColor = "font_color";
                trial.BackgroundColor = "app_bg";
                trial.BackgroundModality = "bg_modality";
                trial.GazeWindow = "window";
                trial.GazePadding = "padding";
                trial.CalibrationBgColor = "cal_bg";
                trial.SaveLog = true;

                base.PushItem(new Bundle<ExperimentConfig>(ItemTypes.Config, trial));

                int i = 0;
                while( _isRunning && i++ < WORD_COUNT)
                {
                    timestamp = Utils.MilliTimetamp();
                    word = new DisplayItem<String>(timestamp, SLEEP, "test");
                    base.PushItem(new Bundle<DisplayItem<String>>(ItemTypes.DisplayItem, word));
                    Thread.Sleep(SLEEP);
                }
            }
        }
    }
}
