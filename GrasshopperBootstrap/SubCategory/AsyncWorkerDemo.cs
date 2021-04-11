namespace GrasshopperBootstrap.SubCategory
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading;
    using Grasshopper;
    using Grasshopper.Kernel;
    using GrasshopperAsyncComponent;
    using Rhino.Geometry;

    public class AsyncWorkerDemo : WorkerInstance
    {
        // This demo/example was developed by Dimitrie Stefanescu for the [Speckle Systems project](https://speckle.systems)
        // This implementation is a near-direct copy of that published in [this repository](https://github.com/specklesystems/GrasshopperAsyncComponent/)

        private int MaxIterations { get; set; } = 100;
        private Plane plane = Plane.WorldXY;
        private double radius0;
        private double radius1;
        private int turns;
        private Curve spiral;

        public AsyncWorkerDemo() : base(null) { }

        public override void DoWork(Action<string, double> reportProgress, Action done)
        {
            // Checking for cancellation
            if (CancellationToken.IsCancellationRequested) { return; }

            // Do the useless spinning demo from Dimitrie's repo to take up time
            for (int i = 0; i <= MaxIterations; i++)
            {
                var sw = new SpinWait();
                for (int j = 0; j <= 100; j++)
                    sw.SpinOnce();

                reportProgress(Id, (double)(i + 1) / (double)MaxIterations);

                // Checking for cancellation
                if (CancellationToken.IsCancellationRequested) { return; }
            }

            // Also do the spiral creation from ExampleComponent.cs to show how to output geometry
            spiral = ModularityDemo.CreateSpiral(plane, radius0, radius1, turns);

            done();
        }

        public override WorkerInstance Duplicate() => new AsyncWorkerDemo();

        public override void GetData(IGH_DataAccess da, GH_ComponentParamServer ghParams)
        {
            if (CancellationToken.IsCancellationRequested) return;

            // As per ExampleComponent.cs
            da.GetData(0, ref plane);
            da.GetData(1, ref radius0);
            da.GetData(2, ref radius1);
            da.GetData(3, ref turns);

            // As per Sample_UslessCyclesComponent
            int maxIterations = 100;
            da.GetData(4, ref maxIterations);
            if (maxIterations > 1000) maxIterations = 1000;
            if (maxIterations < 10) maxIterations = 10;
            MaxIterations = maxIterations;
        }

        public override void SetData(IGH_DataAccess da)
        {
            if (CancellationToken.IsCancellationRequested) return;
            da.SetData(0, spiral); // As per ExampleComponent.cs

            // Can't use the GHBComponent approach to logging; so construct output for Debug param manually
            var debugOutput = new List<string> { $"Worker {Id} spun for {MaxIterations} iterations." };
            da.SetDataList(1, debugOutput);
        }
    }
}
