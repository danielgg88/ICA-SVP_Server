using ICAPR_SVP.Misc;
using ICAPR_SVP.Misc.Executors;
using ICAPR_SVP.Test.MockupImplementations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ICAPR_SVP.Test
{
    [TestClass]
    public class CleanerExecutorTest
    {
        private int NO_SAMPLES = 1000000;

        private ExecutorMultiThreadFilters dataCleaner;
        private Port inputPort;
        private Port outputPort;

        [TestInitialize]
        public void Initialize()
        {
            inputPort = new Misc.PortBlockingDefaultImpl();
            outputPort = new Misc.PortBlockingDefaultImpl();
            inputPort.Start();
            outputPort.Start();

            //Create data cleaning executor
            dataCleaner = new ExecutorMultiThreadFilters(inputPort,outputPort);
            dataCleaner.AddFilter(new FilterTest());
            dataCleaner.AddFilter(new FilterTest());
            dataCleaner.AddFilter(new FilterTest());
            dataCleaner.startInBackground();
        }

        [TestMethod]
        public void TestCleanerExecutor()
        {
            int i;
            for(i = 0;i < NO_SAMPLES;i++)
            {
                Eyes eyes = new Eyes(343253253,new Eye(),new Eye());
                inputPort.PushItem(new Bundle<Eyes>(ItemTypes.Eyes,eyes));
            }

            int j;
            for(j = 0;j < NO_SAMPLES;j++)
            {
                outputPort.GetItem();
            }

            Assert.IsTrue(i == j,"Not equal");
        }
    }
}


