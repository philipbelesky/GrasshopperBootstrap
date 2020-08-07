namespace GrasshopperBootstrap.SubCategory
{
    using Rhino.Geometry;

    public static class GeometryCreation
    {
        /// <summary>
        /// Core functionality of each component is split out of the actual component file
        /// This is partly to prevent those files from getting overly long and partly to better
        /// faciliate unit testing and performance testing.
        /// </summary>
        public static Curve CreateSpiral(Plane plane, double r0, double r1, int turns)
        {
            Line l0 = new Line(plane.Origin + r0 * plane.XAxis, plane.Origin + r1 * plane.XAxis);
            Line l1 = new Line(plane.Origin - r0 * plane.XAxis, plane.Origin - r1 * plane.XAxis);

            Point3d[] p0;
            Point3d[] p1;

            l0.ToNurbsCurve().DivideByCount(turns, true, out p0);
            l1.ToNurbsCurve().DivideByCount(turns, true, out p1);

            PolyCurve spiral = new PolyCurve();

            for (int i = 0; i < p0.Length - 1; i++)
            {
                Arc arc0 = new Arc(p0[i], plane.YAxis, p1[i + 1]);
                Arc arc1 = new Arc(p1[i + 1], -plane.YAxis, p0[i + 1]);

                spiral.Append(arc0);
                spiral.Append(arc1);
            }

            return spiral;
        }
    }
}