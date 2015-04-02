using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICAPR_SVP.Misc;

//Usuful only if we have filters that have same input as output
namespace ICAPR_SVP.DataCleaning
{
    public class FilterComposite<I> : Filter
    {

        private LinkedList<Filter> Filters
        {
            get;
            set;
        }

        public void addFilter(Filter filter)
        {
            Filters.AddLast(filter);

        }

        public void removeFilter(Filter filter)
        {
            Filters.Remove(filter);
        }

        public FilterComposite(String name)
            : base(name)
        {
            Filters = new LinkedList<Filter>();
        }

        protected override void OnExecute(Port input,Port output)
        {
            foreach(Filter filter in Filters)
            {
                filter.Execute();
            }
        }

        protected override void OnStop(Port output)
        {
            foreach(Filter filter in Filters)
                filter.Stop();
        }
    }
}
