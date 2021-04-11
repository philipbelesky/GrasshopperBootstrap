namespace GrasshopperBootstrap
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using Grasshopper.Kernel;
    using GrasshopperAsyncComponent;
    using GrasshopperBootstrap.Properties;
    using GrasshopperBootstrap.SubCategory;

    public class ExampleAsyncComponent : GHBAsyncComponent
    {
        // This demo/example was developed by Dimitrie Stefanescu for the [Speckle Systems project](https://speckle.systems)
        // This implementation is a near-direct copy of that published in [this repository](https://github.com/specklesystems/GrasshopperAsyncComponent/)

        public ExampleAsyncComponent() : base(
            "TestAsyncComponent", "TCA", "Provides a demonstration of a component that does work asynchronously", "TestAsync")
        {
            BaseWorker = new AsyncWorkerDemo();
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddIntegerParameter("N", "N", "Number of spins.", GH_ParamAccess.item);
        }

        protected override void GrasshopperBootstrapRegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Output", "O", "Nothing really interesting.", GH_ParamAccess.item);
        }

        public override void AppendAdditionalMenuItems(ToolStripDropDown menu)
        {
            base.AppendAdditionalMenuItems(menu);
            Menu_AppendItem(menu, "Cancel", (s, e) =>
            {
                RequestCancellation();
            });
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;

        public override Guid ComponentGuid => new Guid("932176ea-061e-4b5b-9642-8417372d6371");

        protected override System.Drawing.Bitmap Icon => Resources.icons_icon_test;
    }
}
