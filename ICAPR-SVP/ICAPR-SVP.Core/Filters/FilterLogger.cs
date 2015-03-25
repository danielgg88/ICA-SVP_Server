using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICAPR_SVP.Misc;

namespace ICAPR_SVP.Core.Filters
{
    public class FilterLogger : Filter
    {
        private Misc.Utils.FileManager<String> fm
        {
            get;
            set;
        }

        public FilterLogger(Misc.Utils.FileManager<String> fm)
        {
            this.fm = fm;
        }

        public override Item execute(Item input)
        {
            fm.AddToFile(input);
            return input;
        }
    }
}
