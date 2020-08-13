namespace GrasshopperBootstrap.Benchmarks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using SubCategory;

    class RunBenchmarks
    {
        static void Main(string[] args)
        {
            RunGeometryCreationBenchmarks.Benchmarks();

            // Prevent console from auto exiting
            Console.Write("Note: Benchmark summaries saved to \\bin\\Release\\BenchmarkDotNet.Artifacts\\results.");
            Console.Write("Finished Benchmarks. Press any key to exit.");
            Console.ReadLine();
        }
    }
}
