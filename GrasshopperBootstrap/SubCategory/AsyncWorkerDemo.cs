namespace GrasshopperBootstrap.SubCategory
{
    using System;
    using System.Threading;
    using Grasshopper;
    using Grasshopper.Kernel;
    using GrasshopperAsyncComponent;

    public class AsyncWorkerDemo : WorkerInstance
    {
        // This demo/example was developed by Dimitrie Stefanescu for the [Speckle Systems project](https://speckle.systems)
        // This implementation is a near-direct copy of that published in [this repository](https://github.com/specklesystems/GrasshopperAsyncComponent/)

        private int MaxIterations { get; set; } = 100;

        public AsyncWorkerDemo() : base(null) { }

        public override void DoWork(Action<string, double> reportProgress, Action done)
        {
            // Checking for cancellation
            if (CancellationToken.IsCancellationRequested) { return; }

            for (int i = 0; i <= MaxIterations; i++)
            {
                var sw = new SpinWait();
                for (int j = 0; j <= 100; j++)
                    sw.SpinOnce();

                reportProgress(Id, (double)(i + 1) / (double)MaxIterations);

                // Checking for cancellation
                if (CancellationToken.IsCancellationRequested) { return; }
            }

            done();
        }

        public override WorkerInstance Duplicate() => new AsyncWorkerDemo();

        public override void GetData(IGH_DataAccess da, GH_ComponentParamServer ghParams)
        {
            if (CancellationToken.IsCancellationRequested) return;

            int maxIterations = 100;
            da.GetData(0, ref maxIterations);
            if (maxIterations > 1000) maxIterations = 1000;
            if (maxIterations < 10) maxIterations = 10;

            MaxIterations = maxIterations;
        }

        public override void SetData(IGH_DataAccess da)
        {
            if (CancellationToken.IsCancellationRequested) return;
            da.SetData(0, $"Hello world. Worker {Id} has spun for {MaxIterations} iterations.");
        }
    }
}
