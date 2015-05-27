using System;

namespace ICA_SVP.Misc.Filters
{
    public abstract class Filter
    {
        public String Name
        {
            get;
            set;
        }
        public Port InputPort
        {
            get;
            set;
        }
        public Port OutputPort
        {
            get;
            set;
        }

        public Filter(String name)
        {
            Name = name;
        }
        public void Execute()
        {
            OnExecute(this.InputPort,this.OutputPort);
        }
        public void Stop()
        {
            OnStop(OutputPort);
        }
        protected abstract void OnExecute(Port input,Port output);
        protected abstract void OnStop(Port output);
    }
}
