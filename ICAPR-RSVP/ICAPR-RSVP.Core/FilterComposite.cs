using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICAPR_RSVP.Misc;



/* Usuful only if we have filters that have same input as output*/
namespace ICAPR_RSVP.Core
{
    class FilterComposite<I> : Filter<I,I>
    {

        private LinkedList<Filter<I, I>> Filters { get; set; }

        public FilterComposite(String name): base(name)
        {
            Filters = new LinkedList<Filter<I, I>>();
        }

        public override Bundle<I> execute(Bundle<I> input)
        {
            Bundle<I> output = null;

            foreach( Filter<I,I> filter in Filters ){
                output = filter.execute(input);
                input = output;
            }

            return output;
        }

        public void addFilter(Filter<I, I> filter)
        {
            Filters.AddLast(filter);

        }

        public void removeFilter(Filter<I, I> filter)
        {
            Filters.Remove(filter);
        }


    }
}
