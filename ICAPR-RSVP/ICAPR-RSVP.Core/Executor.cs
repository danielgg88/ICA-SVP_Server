using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using ICAPR_RSVP.Misc;

namespace ICAPR_RSVP.Core
{
    public abstract class Executor
    {

        protected LinkedList<Filter> Filters { get; set; }

        private volatile Boolean isRunning = false;
        
        private volatile Boolean isPaused = false;

        private volatile Boolean keepRunning = false;

        private volatile Thread thread;


        public Executor()
        {
            Filters = new LinkedList<Filter>();
        }


        #region Public methods

        public void AddFilter(Filter filter)
        {
            Filters.AddLast(filter);
        }

        public void removeFilter(Filter filter)
        {
            Filters.Remove(filter);
        }


        public void startInBackground()
        {
            thread = new Thread(run);
            thread.Start();
        }
        public Boolean isRunningg()
        {
            return isRunning;
        }

        public void stop()
        {
            if (isRunning && thread.IsAlive)
            {
                keepRunning = false;
                thread.Interrupt();
                thread.Join();
            }
        }

        public void pause()
        {
            this.thread.Interrupt();
            isPaused = true;
        }

        public void resume()
        {
            isPaused = false;
        }

        #endregion

        #region Protected methods

        private void run()
        {
            isRunning = true;
            this.keepRunning = true;

            while (keepRunning)
            {
                try
                {
                    while (isPaused)
                        Thread.Sleep(1000);

                    loop();
                }
                catch (ThreadInterruptedException e)
                {
                    Console.WriteLine("Data cleaning executor was interrupted...calling on stop");
                }
            }
            isRunning = false;
            keepRunning = false;
            onStop();

            Console.WriteLine("Data cleaner is exiting...");
        }

        protected abstract void loop();

        protected abstract void onStop();
        #endregion

    }
}
