using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ICAPR_SVP.DataCleaning;
using ICAPR_SVP.Misc;

namespace ICAPR_SVP.Test.MockupImplementations
{
    public class FilterTest : Filter
    {
        public FilterTest()
            : base("test")
        {
        }

        protected override void OnExecute(Port input,Port output)
        {
            output.PushItem(input.GetItem());
        }
        protected override void OnStop(Port output)
        {

        }

    }
}
