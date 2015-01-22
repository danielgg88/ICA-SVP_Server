using ICAPR_RSVP.Misc;
using System.Collections.Concurrent;

namespace ICAPR_RSVP.Broker
{
    public abstract class Port
    {
        private BlockingCollection<Item> _itemQueue;
        private static int _count = 0;
        private int _id;
       
        public Port()
        {
            _id = _count;
            _count++;
        }

        #region Properties
        public int ID
        {
            get { return _id; }
        }
        public abstract bool IsRunning { get; }
        #endregion

        #region Queue methods
        public Item GetItem(){
            return _itemQueue.Take();
        }

        public void PushItem(Item item)
        {
            if (item != null)
                _itemQueue.Add(item);
        }
        #endregion

        #region Port behavior
        public void Start()
        {
            _itemQueue = new BlockingCollection<Item>();
            OnStart();
        }

        public void Stop()
        {
            OnStop();
            _itemQueue = null;
        }
        protected abstract void OnStart();
        protected abstract void OnStop();
        #endregion
    }
}