using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICAPR_RSVP.Misc;



/* Usuful only if we have filters that have same input as output*/
namespace ICAPR_RSVP.Core
{
    class FilterComposite<I> : Filter
    {

        private LinkedList<Filter> Filters { get; set; }

        public FilterComposite(String name): base(name)
        {
            Filters = new LinkedList<Filter>();
        }

        public override Item execute(Item input)
        {
            Item output = null;

            foreach( Filter filter in Filters ){
                output = filter.execute(input);
                input = output;
            }

            return output;
        }

        public void addFilter(Filter filter)
        {
            Filters.AddLast(filter);

        }

        public void removeFilter(Filter filter)
        {
            Filters.Remove(filter);
        }


    }
}
