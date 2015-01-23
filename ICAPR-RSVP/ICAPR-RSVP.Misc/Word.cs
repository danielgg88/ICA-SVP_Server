using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICAPR_RSVP.Misc
{
    public class Word<T>
    {
        public long Timestamp { get; private set; }
        public T Value { get; private set; }

        public Word(long timestamp, T value)
        {
            Timestamp = timestamp;
            Value = value;
        }
    }
}
