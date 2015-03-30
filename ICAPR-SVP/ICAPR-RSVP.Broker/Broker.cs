using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ICAPR_RSVP.Misc;

namespace ICAPR_RSVP.Broker
{
    class Broker
    {
        private readonly List<InputPort> mListInputPort;

        public Broker(List<InputPort> listInputPort)
        {
            this.mListInputPort = new List<InputPort>();
        }

        public bool AddInput(InputPort port){
            if (port != null)
            {
                this.mListInputPort.Add(port);
                return true;
            }
            else
                return false;
        }

        public Item GetItem()
        {
            Item item = null;
            foreach (InputPort port in this.mListInputPort)
            {
                //TODO Mockup
                item = port.GetItem();
                item = Merge(item);
            }
            return item;
        }

        private Item Merge(Item item){
            return item;
        }

        public void Cleanup()
        {
            this.mListInputPort.Clear();
        }
    }  
}
