using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICAPR_RSVP.Misc
{
    public class DisplayItemAndEyes<T>
    {
        public DisplayItem<T> Word { get; private set; }   //Null when idle time
        public Queue<Eyes> Eyes { get; private set; }

        public DisplayItemAndEyes(Queue<Eyes> eyes, DisplayItem<T> word)
        {
            Word = word;
            Eyes = eyes;
        }
    }
}
