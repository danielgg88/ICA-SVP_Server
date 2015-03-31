using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICAPR_SVP.Broker;
using ICAPR_SVP.Misc;

namespace ICAPR_SVP.Core
{
    public class ExecutorDataCleaning : Executor
    {
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

        public ExecutorDataCleaning(Port input,Port output)
            : base()
        {
            this.InputPort = input;
            this.OutputPort = output;
        }

        protected override void init()
        {
            if(OutputPort != null)
                OutputPort.Start();
        }

        protected override void loop()
        {
            Item item = InputPort.GetItem();
            Item result = null;
            foreach(Filter filter in Filters)
            {
                result = filter.execute(item);
                item = result;
            }

            if(Filters.Count > 0)
                OutputPort.PushItem(result);
            else
                OutputPort.PushItem(item);
        }

        protected override void onStop()
        {
        }
    }
}
