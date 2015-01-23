using ICAPR_RSVP.Misc;

namespace ICAPR_RSVP.Broker
{
   public  class BrokerEyeTribeSpritz : Broker
    {
        public BrokerEyeTribeSpritz() : base() { /*Do nothing*/ }

        protected override void Run()
        {
            //Merge input from Spritz and EyeTribe
            Item item = null;
            foreach (Port port in base._listInputPort)
            {
                item = port.GetItem();

                if (item != null)
                {
                    switch (item.Type)
                    {
                        case ItemTypes.Word:
                            //TODO
                            break;
                        case ItemTypes.Eyes:
                            //TODO
                            break;
                        default:
                            break;
                    }
                }

                //Add to output queues
                foreach (Port output in base._listOutputPort)
                {
                    output.PushItem(item);
                }
            }
        }
    }  
}
