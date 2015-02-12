using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICAPR_RSVP.Broker;
using ICAPR_RSVP.Misc;

namespace ICAPR_RSVP.Core
{
    public class DataCleaningExecutor : Executor
    {

        public Port InputPort { get; set; }
        
        public Port OutputPort { get; set; }

        Misc.Utils.FileManager<String> FileManager { get; set; }

        public DataCleaningExecutor(Misc.Utils.FileManager<String> fm , Port input , Port output): base()
        {
            this.FileManager = fm;
            this.InputPort = input;
            this.OutputPort = output;
            this.AddFilter(new Filters.LoggerFilter(this.FileManager));
        }

        protected override void loop()
        {
            Console.WriteLine("Loop");
            Item item = InputPort.GetItem();
            Console.WriteLine("Data cleaner received item");

            Item result = null;

            foreach (Filter filter in Filters)
            {
                result = filter.execute(item);
                item = result;
            }

            OutputPort.PushItem(result);
        }

        protected override void onStop()
        {
            Console.WriteLine("Saving logs...");
            FileManager.SaveLog();
        }

    }
}
