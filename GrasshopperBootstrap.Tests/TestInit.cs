namespace GrasshopperBootstrap.Tests
{
    using System;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1060:Move pinvokes to native methods class", Justification = "<Pending>")]
    public static class TestInit
    {
        static bool initialized;
        static string systemDir;
        static string systemDirOld;

        [AssemblyInitialize]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1806:Do not ignore method results", Justification = "<Pending>")]
        public static void AssemblyInitialize(TestContext context)
        {
            if (initialized)
            {
                throw new InvalidOperationException("AssemblyInitialize should only be called once");
            }
            initialized = true;

            context.WriteLine("Assembly init started");

            // Ensure we are 64 bit
            Assert.IsTrue(Environment.Is64BitProcess, "Tests must be run as x64");

            // Set path to rhino system directory
            string envPath = Environment.GetEnvironmentVariable("path");
            string programFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            systemDir = System.IO.Path.Combine(programFiles, "Rhino 7 WIP", "System");
            systemDirOld = System.IO.Path.Combine(programFiles, "Rhino WIP", "System");
            if (System.IO.Directory.Exists(systemDir) != true)
            {
                systemDir = systemDirOld;
            }
            Assert.IsTrue(System.IO.Directory.Exists(systemDir), "Rhino system dir not found: {0}", systemDir);

            Environment.SetEnvironmentVariable("path", envPath + ";" + systemDir);

            // Add hook for .Net assmbly resolve (for RhinoCommmon.dll and Grasshopper.dll)
            AppDomain.CurrentDomain.AssemblyResolve += ResolveRhinoCommon;
            AppDomain.CurrentDomain.AssemblyResolve += ResolveGrasshopper;

            // Start headless Rhino process
            LaunchInProcess(0, 0);
        }

        private static Assembly ResolveRhinoCommon(object sender, ResolveEventArgs args)
        {
            var name = args.Name;

            if (!name.StartsWith("RhinoCommon", StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }
            string fullPath = AppDomain.CurrentDomain.BaseDirectory;

            var path = System.IO.Path.Combine(fullPath, "RhinoCommon.dll");
            return Assembly.LoadFrom(path);
        }

        private static Assembly ResolveGrasshopper(object sender, ResolveEventArgs args)
        {
            var name = args.Name;

            if (!name.StartsWith("Grasshopper", StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }
            string fullPath = AppDomain.CurrentDomain.BaseDirectory;

            var path = System.IO.Path.Combine(fullPath, "Grasshopper.dll");
            return Assembly.LoadFrom(path);
        }

        [AssemblyCleanup]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1806:Do not ignore method results", Justification = "<Pending>")]
        public static void AssemblyCleanup()
        {
            // Shutdown the rhino process at the end of the test run
            ExitInProcess();
        }

        [DllImport("RhinoLibrary.dll")]
        internal static extern int LaunchInProcess(int reserved1, int reserved2);

        [DllImport("RhinoLibrary.dll")]
        internal static extern int ExitInProcess();
    }
}
