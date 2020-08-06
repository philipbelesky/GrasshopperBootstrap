using System;
using System.Drawing;
using Grasshopper.Kernel;

namespace GrasshopperBootstrap
{
    public class GrasshopperBootstrapInfo : GH_AssemblyInfo
    {
        public override string Name
        {
            get
            {
                return "GrasshopperBootstrap";
            }
        }
        public override Bitmap Icon
        {
            get
            {
                //Return a 24x24 pixel bitmap to represent this GHA library.
                return null;
            }
        }
        public override string Description
        {
            get
            {
                //Return a short string describing the purpose of this GHA library.
                return "";
            }
        }
        public override Guid Id
        {
            get
            {
                return new Guid("69263764-3073-4c66-b093-33ad500105f1");
            }
        }

        public override string AuthorName
        {
            get
            {
                //Return a string identifying you or your company.
                return "";
            }
        }
        public override string AuthorContact
        {
            get
            {
                //Return a string representing your preferred contact details.
                return "";
            }
        }
    }
}
