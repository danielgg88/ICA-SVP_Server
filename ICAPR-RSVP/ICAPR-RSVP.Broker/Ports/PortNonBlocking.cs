using ICAPR_RSVP.Misc;
using System.Collections.Concurrent;

namespace ICAPR_RSVP.Broker
{
    public abstract class PortNonBlocking : Port
    {
        private ConcurrentQueue<Item> _itemQueue; //Non-blocking mode

        #region Queue methods
        public override Item GetItem()
        {
            Item item;
            if (_itemQueue.TryDequeue(out item))
                return (item);
            else
                return null;
        }

        public override void PushItem(Item item)
        {
            if (item != null)
                _itemQueue.Enqueue(item);
        }
        #endregion

        #region Port behavior
        protected override void OnStartPort()
        {
            _itemQueue = new ConcurrentQueue<Item>();
        }

        protected override void OnStopPort()
        {
            _itemQueue = null;
        }
        #endregion

        #region Abstract
        public override abstract bool IsRunning { get; }
        protected override abstract void OnStart();
        protected override abstract void OnStop();
        #endregion
    }
}
