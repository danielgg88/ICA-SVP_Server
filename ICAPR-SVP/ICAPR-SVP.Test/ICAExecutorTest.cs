using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ICAPR_SVP.ICA;

namespace ICAPR_SVP.Test
{
    [TestClass]
    class ICAExecutorTest : ICAExecutor
    {

        private ICAExecutorTest executor;

        public ICAExecutorTest()
            : base()
        {

        }

        [TestInitialize]
        public void Initialize()
        {
            executor = new ICAExecutorTest();
        }

        [TestCleanup]
        public void CleanUp()
        {

        }


        [TestMethod]
        public void testOne()
        {

        }
    }

}
