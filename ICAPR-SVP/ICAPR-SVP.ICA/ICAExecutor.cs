using ICAPR_SVP.Misc.Executors;

namespace ICAPR_SVP.ICA
{
    public class ICAExecutor : ExecutorSingleThread
    {
        public ICAExecutor()
            : base()
        {

        }

        protected override void Run()
        {
            this._listOutputPort[0].PushItem(this._listInputPort[0].GetItem());
        }
    }
}
