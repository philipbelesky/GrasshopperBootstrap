namespace GrasshopperBootstrap
{
    using System;
    using System.Drawing;
    using Grasshopper.Kernel;
    using GrasshopperBootstrap.Properties;

    public class GrasshopperBootstrapInfo : GH_AssemblyInfo
    {
        public override string Name => "GrasshopperBootstrap";

        public override Bitmap Icon => Resources.icons_icon_plugin;

        public override string Description => "GrasshopperBootstrapTODO: Provide description of the plugin";

        public override Guid Id => new Guid("69263764-3073-4c66-b093-33ad500105f1");

        public override string AuthorName => "GrasshopperBootstrapTODO: Add author name";

        public override string AuthorContact => "GrasshopperBootstrapTODO: Add author email";
    }
}
