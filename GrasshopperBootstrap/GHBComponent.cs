namespace GrasshopperBootstrap
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using Grasshopper.Kernel;
    using Rhino;

    public abstract class GHBComponent : GH_Component
    {
        // This is a base class that can be used by all the plugin's components. This can allow for better code reuse for:
        // - Very commonly used functions (e.g. retrieving tolerances)
        // - Shared setup tasks (e.g. plugin category; or if wrapping SolveInstance in exception tracking (e.g. Sentry))

        private static string pluginCategory = "GrasshopperBootstrap"; // GrasshopperBootstrapTODO: The Grasshopper tab that all components sit in
        public static readonly double DocAngleTolerance = RhinoDoc.ActiveDoc.ModelAngleToleranceRadians;
        public static readonly double DocAbsTolerance = RhinoDoc.ActiveDoc.ModelAbsoluteTolerance;

        // Pass the constructor parameters up to the main GH_Component abstract class
        protected GHBComponent(string name, string nickname, string description,  string subCategory)
            : base(name, nickname, description, pluginCategory, subCategory)
        {
        }

        // Components must implement the method
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1313:Parameter names should begin with lower-case letter", Justification = "<Pending>")]
        protected abstract void GrasshopperBootstrapSolveInstance(IGH_DataAccess da);

        // Override the main solve instance method. This allows it to be wrapped in a try/catch for error reporting purposes
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            GrasshopperBootstrapSolveInstance(DA);
        }

        // This is provided to all plugins so it can be passed along to error reporting
        public static string GetPluginVersion()
        {
            var v = Assembly.GetExecutingAssembly().GetName().Version;
            return v.Major.ToString() + '.' + v.Minor.ToString() + '.' + v.Build.ToString();
        }
    }
}
