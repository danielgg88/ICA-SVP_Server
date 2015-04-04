using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ICAPR_SVP.Misc;

namespace ICAPR_SVP.DataCleaning
{
    public class FilterDenoise : Filter
    {
        public FilterDenoise(String name): base(name)
        { 
        }

        #region Protected methods
        protected override void OnExecute(Port input,Port output)
        {
            output.PushItem(input.GetItem());
        }

        protected override void OnStop(Port output)
        {
        }
        #endregion
    }
}
