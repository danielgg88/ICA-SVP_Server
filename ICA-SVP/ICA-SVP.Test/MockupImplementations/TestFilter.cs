
using ICA_SVP.Misc;
using ICA_SVP.Misc.Filters;

namespace ICA_SVP.Test.MockupImplementations
{
    public class TestFilter : Filter
    {
        public TestFilter()
            : base("test")
        {
        }

        protected override void OnExecute(Port input,Port output)
        {
            Item item = input.GetItem();
            Eyes eyes = (Eyes)item.Value;
            eyes.LeftEyeProcessed = new Eye();
            eyes.RightEyeProcessed = new Eye();
            eyes.LeftEyeProcessed.PupilSize = 5;
            eyes.RightEyeProcessed.PupilSize = 5;
            output.PushItem(item);
        }
        protected override void OnStop(Port output)
        {

        }

    }
}
