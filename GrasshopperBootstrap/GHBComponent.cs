using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using Rhino;

namespace GrasshopperBootstrap
{

    public abstract class GrasshopperBootstrapComponent : GH_Component
    {
        // This is a base class that can be used by all the plugin's components. This can allow for better code reuse for:
        // - Very commonly used functions (e.g. retrieving tolerances)
        // - Shared setup tasks (e.g. plugin category; or if wrapping SolveInstance in exception tracking (e.g. Sentry))

        static string pluginCategory = "GrasshopperBootstrap"; // GrasshopperBootstrapTODO: The Grasshopper tab that all components sit in
        protected readonly double docAngularTolerance = RhinoDoc.ActiveDoc.ModelAngleToleranceRadians; 
        protected readonly double docUnitTolerance = RhinoDoc.ActiveDoc.ModelAbsoluteTolerance;

        // Pass the constructor parameters up to the main GH_Component abstract class
        protected GrasshopperBootstrapComponent(string name, string nickname, string description,  string subCategory)
            : base(name, nickname, description, pluginCategory, subCategory)
        {
        }
        
        // Components must implement the method
        protected abstract void GrasshopperBootstrapSolveInstance(IGH_DataAccess DA);

        // Override the main solve instance method. This allows it to be wrapped in a try/catch for error reporting purposes
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            GrasshopperBootstrapSolveInstance(DA);
        }
    }
}
