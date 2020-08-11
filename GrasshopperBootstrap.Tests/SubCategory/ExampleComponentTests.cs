namespace GrasshopperBootstrap.Tests
{
    using Rhino.Geometry;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Collections.Generic;

    [TestClass]
    public class ExampleComponentTests
    {
        /// <summary>
        /// Test the native Grasshopper line component to show how to test components with Node in Code
        /// </summary>
        [TestMethod]
        public void TestLineComponent()
        {
            var ptA = new Point3d(10.0, 10.0, 10.0);
            var ptB = new Point3d(140.0, 110.0, -100.0);
            var lineBaseCase = new Line(ptA, ptB);

            var lineInfo = Rhino.NodeInCode.Components.FindComponent("Line");
            var lineFunction = (System.Func<object, object, object>) lineInfo.Delegate; // Input oject and output object
            var lineResults = (IList<object>) lineFunction(ptA, ptB);
            var lineTestCase = lineResults[0] as Line?;
            
            Assert.AreEqual(lineBaseCase, lineTestCase); // Check plugin's line matches the RhinoCommon line
        }

        /// <summary>
        /// Test the plugin's Grasshopper example component to show how to test components with Node in Code
        /// </summary>
        [TestMethod]
        public void TestExampleComponent()
        {
            var ptA = new Point3d(10.0, 10.0, 10.0);
            var ptB = new Point3d(140.0, 110.0, -100.0);
            var lineBaseCase = new Line(ptA, ptB);

            var exampleInfo = Rhino.NodeInCode.Components.FindComponent("ExampleComponent");
            var exampleFunction = (System.Func<object, object, object>) exampleInfo.Delegate; // Input oject and output object
            var exampleResults = (IList<object>) exampleFunction(ptA, ptB);
            var exampleSpiral = exampleResults[0] as Curve;

            Assert.AreEqual(exampleSpiral.IsArc(), true); // Check spiral is infact an arc
            var spiralPolyCurve = exampleSpiral as PolyCurve;
            var spiralSegments = spiralPolyCurve.Explode().Length;
            Assert.AreEqual(spiralSegments, 10); // Should have 10 segments
        }
    }
}
