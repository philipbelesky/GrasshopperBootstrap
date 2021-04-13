using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace GrasshopperBootstrap.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            // This method is just picked to show how to call into the GrasshopperBoostrap logics
            var result = GrasshopperBootstrap.SubCategory.ModularityDemo.DummyMethodUsedByExampleTests(1, 11);
            Assert.AreEqual(result, 12);
        }
    }
}
