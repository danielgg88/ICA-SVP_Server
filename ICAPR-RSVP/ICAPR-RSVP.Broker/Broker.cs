using System.Collections.Generic;
using System.Threading;

using ICAPR_RSVP.Misc;

namespace ICAPR_RSVP.Broker
{
    public abstract class Broker
    {
        protected readonly List<Port> _listInputPort;   //Input ports list
        protected readonly List<Port> _listOutputPort;  //Output ports list

        private Thread _workerThread;       //Main broker thread
        private volatile bool _shouldStop;  //Broker thread flag

        protected Broker()
        {
            this._listInputPort = new List<Port>();
            this._listOutputPort = new List<Port>();
        }

        #region Public methods

        public bool AddInput(Port port)
        {
            //Add port to input list
            return AddPort(port, this._listInputPort);
        }

        public bool AddOutput(Port port)
        {
            //Add port to output list
            return AddPort(port, this._listOutputPort);
        }

        public bool Start(){
            bool error = false;
            //Start input ports
            if (StartPorts(this._listInputPort))
                error = true;
            //Start output ports
            if (StartPorts(this._listOutputPort))
                error = true;
            //Execute broker
            if (!error)
            {
                _shouldStop = false;
                _workerThread = new Thread(DoWork);
                _workerThread.Start();
            }
            return !error;
        }

        public void DoWork()
        {
            //Broker main execution loop
            while (!_shouldStop)
            {
                Run();
            }
        }

        public void Stop()
        {
            //Stop all ports
            _shouldStop = true;
            _workerThread.Join();
            StopPorts(this._listInputPort);
            StopPorts(this._listOutputPort);
        }

        public int GetInputIndexByID(int id)
        {
            //Get index in input list by providing port ID
            for (int i = 0; i < _listInputPort.Count; i++)
            {
                if (_listInputPort[i].ID == id)
                    return i;
            }
            return -1;
        }

        #endregion Private methods

        #region Private methods

        private bool AddPort(Port port, List<Port> list)
        {
            //Add port to list
            if (port != null)
            {
                list.Add(port);
                return true;
            }
            else
                return false;
        }

        private bool StartPorts(List<Port> listPort)
        {
            //Start each port in the list
            bool error = false;
            foreach (Port port in listPort)
            {
              port.Start();
              if (!port.IsRunning)
                  error = true;
            }
            return error;
        }

        private void StopPorts(List<Port> listPort)
        {
            //Stop each port in the list
            foreach (Port port in listPort)
            {
                port.Stop();
            }
        }
        #endregion

        #region Protected
        protected void sendToOutput(Item item)
        {
            if (item != null)
            {
                foreach (Port output in this._listOutputPort)
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
