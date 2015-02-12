using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICAPR_RSVP.Misc;

namespace ICAPR_RSVP.Core.Filters
{
    public class LoggerFilter : Filter
    {
        private Misc.Utils.FileManager<String> fm { get; set; }

        public LoggerFilter(Misc.Utils.FileManager<String> fm)
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
