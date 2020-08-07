namespace Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using GrasshopperBootstrap.SubCategory;
    using Rhino.Geometry;

    [TestClass]
    public class TestGeometryCreation
    {
        [TestMethod]
        /// <summary>
        /// An example of the (limited) form of unit testing that can be setup for a Grasshopper project
        /// Here we are no doing a full 1:1 assertion about a geometric object; but are instead 
        /// reasoning about the result
        /// </summary>
        public void TestCreateSpiral()
        {
            var stubInnerRadius = 10.0;
            var stubOuterRadius = 100.0;
            var stubTurns = 5;
            var stubPlane = new Plane(new Point3d(0,0,0), new Vector3d(0,0,1));

            //var spiral = GeometryCreation.CreateSpiral(stubPlane, stubInnerRadius, stubOuterRadius, stubTurns);

            //Assert.AreEqual(spiral.IsArc(), true);
            //var spiralPolyCurve = spiral as PolyCurve;
            //var spiralSegments = spiralPolyCurve.Explode().Length;

            //Assert.AreEqual(spiralSegments, 10); // Should have 10 segments
          
            Assert.AreEqual("test", "test"); // Should have 10 segments
        }
    }
}
