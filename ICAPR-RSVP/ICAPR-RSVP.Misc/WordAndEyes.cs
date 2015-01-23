using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICAPR_RSVP.Misc
{
    public class WordAndEyes<T>
    {
        public Word<T> Word { get; private set; }
        public Eyes Eyes { get; private set; }

        public WordAndEyes(Eyes eyes, Word<T> word)
        {
            Word = word;
            Eyes = eyes;
        }
    }
}
