using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICAPR_RSVP.Misc
{
    public class WordAndEyes<T>
    {
        public Word<T> Word { get; private set; }   //Null when idle time
        public Queue<Eyes> Eyes { get; private set; }

        public WordAndEyes(Queue<Eyes> eyes, Word<T> word)
        {
            Word = word;
            Eyes = eyes;
        }
    }
}
