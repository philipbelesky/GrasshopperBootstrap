namespace GrasshopperBootstrap.Benchmarks.SubCategory
{
    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Running;
    using Rhino.Geometry;
    using System.Collections.Generic;
    using GrasshopperBootstrap.SubCategory;

    public class RunGeometryCreationBenchmarks
    {
        public static void Benchmarks()
        {
            BenchmarkRunner.Run<GeometryCreationBenchmarks>();
            GeometryCreationBenchmarks.TestCurrentImplementation();
            GeometryCreationBenchmarks.TestProposedImplementation();
        }
    }

    public class GeometryCreationBenchmarks
    {
        private static Plane stubPlane = new Plane(new Point3d(0, 0, 0), new Vector3d(0, 0, 1));

        [Benchmark(Baseline = true)]
        public static void TestCurrentImplementation()
        {
            var stubInnerRadius = 10.0;
            var stubOuterRadius = 100.0;
            var stubTurns = 5;
            Curve spiral = GeometryCreation.CreateSpiral(stubPlane, stubInnerRadius, stubOuterRadius, stubTurns);
        }

        [Benchmark]
        public static void TestProposedImplementation()
        {;
            var stubInnerRadius = 10.0;
            var stubOuterRadius = 10000.0;
            var stubTurns = 25;
            Curve spiral = GeometryCreation.CreateSpiral(stubPlane, stubInnerRadius, stubOuterRadius, stubTurns);
        }
    }
}


