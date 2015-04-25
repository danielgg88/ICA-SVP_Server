using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICAPR_SVP.Misc;

namespace ICAPR_SVP.DataCleaning
{
    public class EmptyFilter: Misc.Filters.Filter
    {

       public EmptyFilter(): base("test")
        {
        }

        protected override void OnExecute(Port input,Port output)
        {
            Item item = input.GetItem();
            Eyes eyes = (Eyes)item.Value;
            eyes.LeftEyeProcessed = new Eye();
            eyes.RightEyeProcessed = new Eye();
            output.PushItem(item);
        }
        protected override void OnStop(Port output)
        {

        }
    }
}
