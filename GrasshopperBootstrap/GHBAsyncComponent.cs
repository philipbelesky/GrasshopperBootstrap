namespace GrasshopperBootstrap
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Grasshopper.Kernel;
    using GrasshopperAsyncComponent;
    using Timer = System.Timers.Timer;

    public abstract class GHBAsyncComponent : GHBComponent
    {
        // This is a base class that can be used by all of a plugin's components to do calculations asynchronously
        // This approach was developed by Dimitrie Stefanescu for the [Speckle Systems project](https://speckle.systems)
        // This implementation is a near-direct copy of that published in [this repository](https://github.com/specklesystems/GrasshopperAsyncComponent/)

        Action<string, double> reportProgress;
        public ConcurrentDictionary<string, double> ProgressReports;
        Action done;
        Timer displayProgressTimer;
        int state = 0;
        int setData = 0;
        public List<WorkerInstance> workers;
        List<Task> tasks;
        public readonly List<CancellationTokenSource> CancellationSources;

        /// <summary>
        /// Set this property inside the constructor of your derived component.
        /// </summary>
        public WorkerInstance BaseWorker { get; set; }

        /// <summary>
        /// Optional: if you have opinions on how the default system task scheduler should treat your workers, set it here.
        /// </summary>
        public TaskCreationOptions? TaskCreationOptions { get; set; } = null;

        // Pass the constructor parameters up to the main GHBComponent abstract class
        protected GHBAsyncComponent(string name, string nickname, string description, string subCategory)
            : base(name, nickname, description, subCategory)
        {
            displayProgressTimer = new Timer(333) { AutoReset = false };
            displayProgressTimer.Elapsed += DisplayProgress;

            reportProgress = (id, value) =>
            {
                ProgressReports[id] = value;
                if (!displayProgressTimer.Enabled)
                {
                    displayProgressTimer.Start();
                }
            };

            done = () =>
            {
                Interlocked.Increment(ref state);
                if (state == workers.Count && setData == 0)
                {
                    Interlocked.Exchange(ref setData, 1);

                    // We need to reverse the workers list to set the outputs in the same order as the inputs.
                    workers.Reverse();

                    Rhino.RhinoApp.InvokeOnUiThread((Action)delegate
                    {
                        ExpireSolution(true);
                    });
                }
            };

            ProgressReports = new ConcurrentDictionary<string, double>();

            workers = new List<WorkerInstance>();
            CancellationSources = new List<CancellationTokenSource>();
            tasks = new List<Task>();
        }

        public virtual void DisplayProgress(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (workers.Count == 0 || ProgressReports.Values.Count == 0)
            {
                return;
            }

            if (workers.Count == 1)
            {
                Message = ProgressReports.Values.Last().ToString("0.00%");
            }
            else
            {
                double total = 0;
                foreach (var kvp in ProgressReports)
                {
                    total += kvp.Value;
                }

                Message = (total / workers.Count).ToString("0.00%");
            }

            Rhino.RhinoApp.InvokeOnUiThread((Action)delegate
            {
                OnDisplayExpired(true);
            });
        }

        protected override void BeforeSolveInstance()
        {
            if (state != 0 && setData == 1)
            {
                return;
            }

            Debug.WriteLine("Killing");

            foreach (var source in CancellationSources)
            {
                source.Cancel();
            }

            CancellationSources.Clear();
            workers.Clear();
            ProgressReports.Clear();
            tasks.Clear();

            Interlocked.Exchange(ref state, 0);
        }

        protected override void AfterSolveInstance()
        {
            System.Diagnostics.Debug.WriteLine("After solve instance was called " + state + " ? " + workers.Count);
            // We need to start all the tasks as close as possible to each other.
            if (state == 0 && tasks.Count > 0 && setData == 0)
            {
                System.Diagnostics.Debug.WriteLine("After solve INVOKATIONM");
                foreach (var task in tasks)
                {
                    task.Start();
                }
            }
        }

        protected override void ExpireDownStreamObjects()
        {
            // Prevents the flash of null data until the new solution is ready
            if (setData == 1)
            {
                base.ExpireDownStreamObjects();
            }
        }

        protected override void GrasshopperBootstrapSolveInstance(IGH_DataAccess da)
        {
            if (state == 0)
            {
                if (BaseWorker == null)
                {
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Worker class not provided.");
                    return;
                }

                var currentWorker = BaseWorker.Duplicate();
                if (currentWorker == null)
                {
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Could not get a worker instance.");
                    return;
                }

                // Let the worker collect data.
                currentWorker.GetData(da, Params);

                // Create the task
                var tokenSource = new CancellationTokenSource();
                currentWorker.CancellationToken = tokenSource.Token;
                currentWorker.Id = $"Worker-{da.Iteration}";

                var currentRun = TaskCreationOptions != null
                  ? new Task(() => currentWorker.DoWork(reportProgress, done), tokenSource.Token, (TaskCreationOptions)TaskCreationOptions)
                  : new Task(() => currentWorker.DoWork(reportProgress, done), tokenSource.Token);

                // Add cancellation source to our bag
                CancellationSources.Add(tokenSource);

                // Add the worker to our list
                workers.Add(currentWorker);

                tasks.Add(currentRun);

                return;
            }

            if (setData == 0)
            {
                return;
            }

            if (workers.Count > 0)
            {
                Interlocked.Decrement(ref state);
                workers[state].SetData(da);
            }

            if (state != 0)
            {
                return;
            }

            CancellationSources.Clear();
            workers.Clear();
            ProgressReports.Clear();
            tasks.Clear();

            Interlocked.Exchange(ref setData, 0);

            Message = "Done";
            OnDisplayExpired(true);
        }

        public void RequestCancellation()
        {
            foreach (var source in CancellationSources)
            {
                source.Cancel();
            }

            CancellationSources.Clear();
            workers.Clear();
            ProgressReports.Clear();
            tasks.Clear();

            Interlocked.Exchange(ref state, 0);
            Interlocked.Exchange(ref setData, 0);
            Message = "Cancelled";
            OnDisplayExpired(true);
        }

    }
}
