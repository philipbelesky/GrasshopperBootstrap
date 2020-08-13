namespace GrasshopperBootstrap.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.IO;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading.Tasks;

    [TestClass]
    public static class TestInit
    {
        static bool initialized = false;
        static string systemDir = null;
        static string systemDirOld = null;

        [AssemblyInitialize]
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

            if (!name.StartsWith("RhinoCommon"))
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

            if (!name.StartsWith("Grasshopper"))
            {
                return null;
            }
            string fullPath = AppDomain.CurrentDomain.BaseDirectory;

            var path = System.IO.Path.Combine(fullPath, "Grasshopper.dll");
            return Assembly.LoadFrom(path);
        }
        
        [AssemblyCleanup]
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
