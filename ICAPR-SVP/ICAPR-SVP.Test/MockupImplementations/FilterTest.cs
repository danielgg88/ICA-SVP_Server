﻿
using ICAPR_SVP.Misc;
using ICAPR_SVP.Misc.Filters;

namespace ICAPR_SVP.Test.MockupImplementations
{
    public class FilterTest : Filter
    {
        public FilterTest()
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
