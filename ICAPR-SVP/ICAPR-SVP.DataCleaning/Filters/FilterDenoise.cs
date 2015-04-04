using ICAPR_SVP.Misc;
using ICAPR_SVP.Misc.Filters;
using System;

namespace ICAPR_SVP.DataCleaning
{
    public class FilterDenoise : Filter
    {
        public FilterDenoise(String name)
            : base(name)
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
