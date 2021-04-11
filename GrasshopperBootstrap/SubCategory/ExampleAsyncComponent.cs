namespace GrasshopperBootstrap
{
    using System;
    using Grasshopper.Kernel;
    using GrasshopperBootstrap.Properties;
    using GrasshopperBootstrap.SubCategory;
    using Rhino.Geometry;

    public class ExampleAsyncComponent : GHBAsyncComponent
    {
        // This demo/example was developed by Dimitrie Stefanescu for the [Speckle Systems project](https://speckle.systems)
        // This implementation is a near-direct copy of that published in [this repository](https://github.com/specklesystems/GrasshopperAsyncComponent/)

        public ExampleAsyncComponent() : base(
            "TestAsyncComponent", "TCA", "Provides a demonstration of a component that does work asynchronously", "AsyncTest")
        {
            BaseWorker = new AsyncWorkerDemo();
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddPlaneParameter("Plane", "P", "Base plane for spiral", GH_ParamAccess.item, Plane.WorldXY);
            pManager.AddNumberParameter("Inner Radius", "R0", "Inner radius for spiral", GH_ParamAccess.item, 1.0);
            pManager.AddNumberParameter("Outer Radius", "R1", "Outer radius for spiral", GH_ParamAccess.item, 10.0);
            pManager.AddIntegerParameter("Turns", "T", "Number of turns between radii", GH_ParamAccess.item, 10);
            pManager.AddIntegerParameter("AsyncLoops", "N", "Number of spins.", GH_ParamAccess.item, 50); // As per Sample_UslessCyclesComponent
        }

        protected override void GrasshopperBootstrapRegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddCurveParameter("Spiral", "S", "Spiral curve", GH_ParamAccess.item);
        }


        public override GH_Exposure Exposure => GH_Exposure.primary;

        public override Guid ComponentGuid => new Guid("932176ea-061e-4b5b-9642-8417372d6371");

        protected override System.Drawing.Bitmap Icon => Resources.icons_icon_test;
    }
}
