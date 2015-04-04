using System.Collections.Generic;
using System.Threading;


namespace ICAPR_SVP.Misc.Executors
{
    public abstract class ExecutorSingleThread
    {
        protected readonly List<Port> _listInputPort;   //Input ports list
        protected readonly List<Port> _listOutputPort;  //Output ports list

        private Thread _workerThread;       //Main broker thread
        private volatile bool _shouldStop;  //Broker thread flag

        protected ExecutorSingleThread()
        {
            this._listInputPort = new List<Port>();
            this._listOutputPort = new List<Port>();
        }

        #region Public methods

        public bool AddInput(Port port)
        {
            //Add port to input list
            return AddPort(port,this._listInputPort);
        }

        public bool AddOutput(Port port)
        {
            //Add port to output list
            return AddPort(port,this._listOutputPort);
        }

        public void Start()
        {
            _shouldStop = false;
            _workerThread = new Thread(DoWork);
            _workerThread.Start();
        }

        public void DoWork()
        {
            //Broker main execution loop
            while(!_shouldStop)
            {
                Run();
            }
        }

        public void Stop()
        {
            //Stop all ports
            _shouldStop = true;
            _workerThread.Join();
        }
        #endregion Private methods

        #region Private methods

        private bool AddPort(Port port,List<Port> list)
        {
            //Add port to list
            if(port != null)
            {
                list.Add(port);
                return true;
            }
            else
                return false;
        }
        #endregion

        #region Protected
        protected void sendToOutput(Item item)
        {
            if(item != null)
            {
                foreach(Port output in this._listOutputPort)
                {
                    output.PushItem(item);
                }
            }
        }
        #endregion

        #region Abstract methods
        protected abstract void Run();
        #endregion
    }
}
