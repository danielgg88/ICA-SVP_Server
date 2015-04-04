using System.Collections.Concurrent;

namespace ICAPR_SVP.Misc
{
    public abstract class PortBlocking : Port
    {
        private BlockingCollection<Item> _itemQueueBlocking;   //For blocking mode

        #region Queue methods
        public override Item GetItem()
        {
            return _itemQueueBlocking.Take();
        }

        public override void PushItem(Item item)
        {
            if(item != null)
                _itemQueueBlocking.Add(item);
        }
        #endregion

        #region Port behavior
        protected override void OnStartPort()
        {
            _itemQueueBlocking = new BlockingCollection<Item>();
        }

        protected override void OnStopPort()
        {
            _itemQueueBlocking = null;
        }
        #endregion

        #region Abstract
        public override abstract bool IsRunning
        {
            get;
        }
        protected override abstract void OnStart();
        protected override abstract void OnStop();
        #endregion
    }
}