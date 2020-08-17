namespace GrasshopperBootstrap
{
    using System;
    using System.Linq;
    using System.Net.Http;
    using System.ServiceModel.Syndication;
    using System.Xml;
    using Grasshopper.Kernel;
    using GrasshopperBootstrap.Properties;

    public class AboutComponent : GHBComponent
    {
        public AboutComponent() : base(
            "About GrasshopperBootstrap", "AB", "Displays information about this plugin, including " +
            "documentation sources and current/latest versions.", "About")
        { }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager) { }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Current Version", "cV", "The version of the installed plugin.", GH_ParamAccess.item);
            pManager.AddTextParameter("Latest Version", "lV", "The latest released version of the installed plugin.", GH_ParamAccess.item);
            pManager.AddTextParameter("About", "A", "Information about this plugin.", GH_ParamAccess.item);
            pManager.AddTextParameter("URL", "U", "A link to this plugin's documentation.", GH_ParamAccess.item);
        }

        protected override void GrasshopperBootstrapSolveInstance(IGH_DataAccess da)
        {
            var assemblyInfo = new GrasshopperBootstrapInfo();
            da.SetData(0, GetPluginVersion());
            da.SetData(1, GetLatestVersion(assemblyInfo.ReleasesFeed));
            da.SetData(2, assemblyInfo.Description);
            da.SetData(3, assemblyInfo.PluginURL.ToString());
        }

        private static string GetLatestVersion(Uri feedURL)
        {
            var client = new HttpClient();
            var result = client.GetStreamAsync(feedURL).Result;
            var version = "A latest release could not be found";
            using (var xmlReader = XmlReader.Create(result))
            {
                SyndicationFeed feed = SyndicationFeed.Load(xmlReader);

                if (feed != null)
                    version = feed.Items.First().Title.Text;
            }

            client.Dispose();
            return version;
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;

        public override Guid ComponentGuid => new Guid("524e4194-6cca-4d09-908f-f2ee8e8b6352");

        protected override System.Drawing.Bitmap Icon => Resources.icons_icon_plugin;
    }
}
