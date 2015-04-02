using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICAPR_SVP.Misc;

namespace ICAPR_SVP.DataCleaning
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
