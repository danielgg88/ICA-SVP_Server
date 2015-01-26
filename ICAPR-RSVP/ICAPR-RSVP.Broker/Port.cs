using ICAPR_RSVP.Misc;

namespace ICAPR_RSVP.Broker
{
    public abstract class Port
    {
        #region Properties
        public int Index { get; set; }
        #endregion

        #region Abstract
        public abstract Item GetItem();             //Implemented by the port type (bloking/non-blocking)
        public abstract void PushItem(Item item);   //Implemented by the port type (bloking/non-blocking)
        protected abstract void OnStartPort();      //Implemented by the port type (bloking/non-blocking)
        protected abstract void OnStopPort();       //Implemented by the port type (bloking/non-blocking)
        public abstract bool IsRunning { get; }     //Implemented by the port spcific implementation
        protected abstract void OnStart();          //Implemented by the port spcific implementation
        protected abstract void OnStop();           //Implemented by the port spcific implementation
        #endregion

        #region Port behavior
        public void Start()
        {
            OnStartPort();
            OnStart();
        }

        public void Stop()
        {
            OnStop();
            OnStopPort();
        }
        #endregion
    }
}