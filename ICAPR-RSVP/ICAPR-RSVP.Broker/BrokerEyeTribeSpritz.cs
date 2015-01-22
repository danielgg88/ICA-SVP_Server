using ICAPR_RSVP.Misc;

namespace ICAPR_RSVP.Broker
{
   public  class BrokerEyeTribeSpritz : Broker
    {
        public BrokerEyeTribeSpritz() : base()
        {
            //Do nothing
        }

        protected override void Run()
        {
            //TODO Testing code. Replace with merging
            Item item = null;
            foreach (Port port in base._listInputPort)
            {
                item = port.GetItem();
            }

            foreach (Port port in base._listOutputPort)
            {
                port.PushItem(item);
            }
        }
    }  
}
