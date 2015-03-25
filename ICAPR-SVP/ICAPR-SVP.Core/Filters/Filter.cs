using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICAPR_SVP.Misc;

namespace ICAPR_SVP.Core
{
    public abstract class Filter
    {
        public String Name
        {
            get;
            set;
        }


        public Filter()
        {
        }

        public Filter(String name)
        {
            Name = name;
        }

        public abstract Item execute(Item input);

    }
}
