namespace GrasshopperBootstrap
{
    using System;
    using Grasshopper.Kernel;
    using GrasshopperBootstrap.Properties;
    using GrasshopperBootstrap.SubCategory;
    using Rhino.Geometry;

    public class ExampleComponent : GHBComponent
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear,
        /// Subcategory the panel. If you use non-existing tab or panel names,
        /// new tabs/panels will automatically be created.
        /// </summary>
        public ExampleComponent() : base(
            "TestComponent", "TC", "Construct an Archimedean, or arithmetic, spiral given its " +
            "radii and number of turns.", "Test")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            // Use the pManager object to register your input parameters.
            // You can often supply default values when creating parameters.
            // All parameters must have the correct access type. If you want
            // to import lists or trees of values, modify the ParamAccess flag.
            pManager.AddPlaneParameter("Plane", "P", "Base plane for spiral", GH_ParamAccess.item, Plane.WorldXY);
            pManager.AddNumberParameter("Inner Radius", "R0", "Inner radius for spiral", GH_ParamAccess.item, 1.0);
            pManager.AddNumberParameter("Outer Radius", "R1", "Outer radius for spiral", GH_ParamAccess.item, 10.0);
            pManager.AddIntegerParameter("Turns", "T", "Number of turns between radii", GH_ParamAccess.item, 10);

            // If you want to change properties of certain parameters,
            // you can use the pManager instance to access them by index:
            // pManager[0].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void GrasshopperBootstrapRegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            // Use the pManager object to register your output parameters.
            // Output parameters do not have default values, but they too must have the correct access type.
            pManager.AddCurveParameter("Spiral", "S", "Spiral curve", GH_ParamAccess.item);

            // Sometimes you want to hide a specific parameter from the Rhino preview.
            // You can use the HideParameter() method as a quick way:
            // pManager.HideParameter(0);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="da">The DA object can be used to retrieve data from input parameters and
        /// to store data in output parameters.</param>
        protected override void GrasshopperBootstrapSolveInstance(IGH_DataAccess da)
        {
            // First, we need to retrieve all data from the input parameters.
            // We'll start by declaring variables and assigning them starting values.
            Plane plane = Plane.WorldXY;
            double radius0 = 0.0;
            double radius1 = 0.0;
            int turns = 0;

            // Then we need to access the input parameters individually.
            // When data cannot be extracted from a parameter, we should abort this method.
            // GHB note: There is no need to wrap these getters in a return - components will not
            //  execute if non-optional values are not provided by users
            da.GetData(0, ref plane);
            da.GetData(1, ref radius0);
            da.GetData(2, ref radius1);
            da.GetData(3, ref turns);

            // We should now validate the data and warn the user if invalid data is supplied.
            if (radius0 < 0.0)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Inner radius must be bigger than or equal to zero");
                return;
            }

            if (radius1 <= radius0)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Outer radius must be bigger than the inner radius");
                return;
            }

            if (turns <= 0)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Spiral turn count must be bigger than or equal to one");
                return;
            }

            LogTiming("Initial setup"); // Debug Info

            // GHB note: accessing these shortcuts just to show they exist and to prevent the build warning
            var aTolerance = DocAngleTolerance;
            var dTolerance = DocAbsTolerance;

            // We're set to create the spiral now. To keep the size of the SolveInstance() method small,
            // the actual functionality will be in a different method:
            // GHB note: the function here has been shifted to a separate file (GeometryCreation.cs).
            using (Curve spiral = ModularityDemo.CreateSpiral(plane, radius0, radius1, turns))
            {
                // Finally assign the spiral to the output parameter.
                da.SetData(0, spiral);
            }

            LogTiming("Creating spiral"); // Debug Info
            LogGeneral(string.Format("Angle tolerance: {0}\nAbsolute tolerance: {1}", aTolerance, dTolerance));
        }

        /// <summary>
        /// The Exposure property controls where in the panel a component icon
        /// will appear. There are seven possible locations (primary to septenary),
        /// each of which can be combined with the GH_Exposure.obscure flag, which
        /// ensures the component will only be visible on panel dropdowns.
        /// </summary>
        /// GHB note: following properties are set via arrow assignment to reduce clutter
        public override GH_Exposure Exposure => GH_Exposure.primary;

        /// <summary>
        /// Each component must have a unique Guid to identify it.
        /// It is vital this Guid doesn't change otherwise old ghx files
        /// that use the old ID will partially fail during loading.
        /// </summary>
        /// GHB note: new Guids easily generated at https://www.guidgenerator.com
        public override Guid ComponentGuid => new Guid("932176ea-061e-4b5b-9642-8417372d6372");

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// </summary>
        /// GHB note: resources are added under a project's properties, then the resources tab
        protected override System.Drawing.Bitmap Icon => Resources.icons_icon_test;
    }
}
