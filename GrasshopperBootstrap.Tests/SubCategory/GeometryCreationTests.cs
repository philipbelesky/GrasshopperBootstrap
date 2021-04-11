﻿namespace GrasshopperBootstrap.Tests
{
    using GrasshopperBootstrap.SubCategory;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Rhino.Geometry;

    [TestClass]
    public class GeometryCreationTests
    {
        /// <summary>
        /// Test the spiral creation function used in the TestComponent
        /// </summary>
        [TestMethod]
        public void TestCreateSpiral()
        {
            var stubInnerRadius = 10.0;
            var stubOuterRadius = 100.0;
            var stubTurns = 5;
            var stubPlane = new Plane(new Point3d(0, 0, 0), new Vector3d(0, 0, 1));

            using (var spiral = GeometryCreation.CreateSpiral(stubPlane, stubInnerRadius, stubOuterRadius, stubTurns))
            {
                var spiralPolyCurve = spiral as PolyCurve;
                var spiralSegments = spiralPolyCurve.Explode().Length;
                Assert.AreEqual(spiralSegments, 10); // Should have 10 segments
            }
        }
    }
}
