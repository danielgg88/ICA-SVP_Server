using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using ICAPR_SVP.Misc;

namespace ICAPR_SVP.DataCleaning
{
    public class DataCleaningExecutor
    {
        private volatile Boolean _isRunning = false;
        private volatile Boolean _isPaused = false;
        private volatile Boolean _keepRunning = false;
        private volatile List<Thread> _threads;

        protected LinkedList<Filter> Filters
        {
            get;
            private set;
        }
        public Port InputPort
        {
            get;
            private set;
        }
        public Port OutputPort
        {
            get;
            private set;
        }

        public DataCleaningExecutor(Port input,Port output)
        {
            Filters = new LinkedList<Filter>();
            _threads = new List<Thread>();
            InputPort = input;
            OutputPort = output;
        }

        #region Public methods

        public Boolean isRunningg()
        {
            return _isRunning;
        }

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
            _keepRunning = true;
            setup();
            foreach(Filter filter in Filters)
            {
                Thread thread = new Thread(() => run(filter));
                _threads.Add(thread);
                thread.Start();
            }
            _isRunning = true;
        }

        public void stop()
        {
            if(_isRunning)
            {
                _keepRunning = false;

                foreach(Thread thread in _threads)
                {
                    if(thread.IsAlive)
                    {
                        thread.Interrupt();
                        thread.Join();
                    }
                }

                //Call stop in all filters and stop their ports
                foreach(Filter filter in Filters)
                {
                    filter.Stop();
                    filter.InputPort.Stop();
                }
                Filters.Last().OutputPort.Stop();
            }
        }

        public void pause()
        {
            _isPaused = true;
        }

        public void resume()
        {
            _isPaused = false;
        }
        #endregion

        #region Private methods
        private void setup()
        {
            Filter filter;
            Port port = null;
            for(int i = 0;i < Filters.Count;i++)
            {
                filter = Filters.ElementAt(i);
                if(i == 0)
                    filter.InputPort = this.InputPort;
                else
                    filter.InputPort = port;

                if(i == Filters.Count - 1)
                    filter.OutputPort = this.OutputPort;
                else
                {
                    port = new PortBlockingDefaultImpl();
                    port.Start();
                    filter.OutputPort = port;
                }
            }
        }
        private void run(Filter filter)
        {
            while(_keepRunning)
            {
                try
                {
                    while(!_isPaused)
                        filter.Execute();
                }
                catch(ThreadInterruptedException)
                {
                }
            }
            _isRunning = false;
        }
        #endregion
    }
}
