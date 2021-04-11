namespace GrasshopperAsyncComponent
{
    using System;
    using System.Threading;
    using Grasshopper.Kernel;

    /// <summary>
    /// A class that holds the actual compute logic and encapsulates the state it needs. Every <see cref="GHBAsyncComponent"/> needs to have one.
    /// </summary>
    public abstract class WorkerInstance
    {
        /// <summary>
        /// The parent component. Useful for passing state back to the host component.
        /// </summary>
        public GH_Component Parent { get; set; }

        /// <summary>
        /// This token is set by the parent <see cref="GHBAsyncComponent"/>.
        /// </summary>
        public CancellationToken CancellationToken { get; set; }

        /// <summary>
        /// This is set by the parent <see cref="GHBAsyncComponent"/>. You can set it yourself, but it's not really worth it.
        /// </summary>
        public string Id { get; set; }

        protected WorkerInstance(GH_Component parent)
        {
            Parent = parent;
        }

        /// <summary>
        /// This is a "factory" method. It should return a fresh instance of this class, but with all the necessary state that you might have passed on directly from your component.
        /// </summary>
        /// <returns></returns>
        public abstract WorkerInstance Duplicate();

        /// <summary>
        /// This method is where the actual calculation/computation/heavy lifting should be done.
        /// <b>Make sure you always check as frequently as you can if <see cref="WorkerInstance.CancellationToken"/> is cancelled. For an example, see the <see cref="PrimeCalculatorWorker"/>.</b>
        /// </summary>
        /// <param name="reportProgress">Call this to report progress up to the parent component.</param>
        /// <param name="done">Call this when everything is <b>done</b>. It will tell the parent component that you're ready to <see cref="SetData(IGH_DataAccess)"/>.</param>
        public abstract void DoWork(Action<string, double> reportProgress, Action done);

        /// <summary>
        /// Write your data setting logic here. <b>Do not call this function directly from this class. It will be invoked by the parent <see cref="GHBAsyncComponent"/> after you've called `Done` in the <see cref="DoWork(Action{string}, Action{string, GH_RuntimeMessageLevel}, Action)"/> function.</b>
        /// </summary>
        /// <param name="da"></param>
        public abstract void SetData(IGH_DataAccess da);

        /// <summary>
        /// Write your data collection logic here. <b>Do not call this method directly. It will be invoked by the parent <see cref="GHBAsyncComponent"/>.</b>
        /// </summary>
        /// <param name="da"></param>
        /// <param name="params"></param>
        public abstract void GetData(IGH_DataAccess da, GH_ComponentParamServer ghParams);
    }
}
