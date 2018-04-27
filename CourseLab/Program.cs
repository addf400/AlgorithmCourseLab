using CourseLab.ConvexHull;
using CourseLab.SetCover;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseLab
{
    class Program
    {
        static void Main(string[] args)
        {
            //ConvexHull();
            SetCover();
            //test.Test();
        }

        static void ConvexHull()
        {
            string root = @"C:\Users\addf4\Desktop\Code\AlgrithmCouseLabData";
            var dataSet = CourseLab.ConvexHull.DataGen.Run(root, new int[] { 10, 15, 20, 50, 100, 200, 500, 1000, 2000, 5000, 10000 });

            foreach (var dataCase in dataSet)
            {
                var solvers = new List<ConvexHullSolution>();
                var points = Point.LoadPointsFromFile(dataCase.Item1);
                solvers.Add(new BruteForce(points));
                solvers.Add(new GrahamScan(points));
                solvers.Add(new DivAndConquer(points));
                
                foreach (var solver in solvers)
                {
                    var ans = solver.Run();
                    Point.SavePointsToFile(ans.Item1, 
                        Path.Combine(root, String.Format("Ans-{0}-{1}.txt", solver.GetMethodName(), dataCase.Item2)));
                    Console.WriteLine(solver.GetMethodName());
                    Console.WriteLine("N = " + dataCase.Item2);
                    Console.WriteLine(ans.Item2);
                    Console.WriteLine(ans);
                }
            }

            Console.ReadLine();
        }

        static void SetCover()
        {
            string root = @"Data";
            if (!Directory.Exists(root))
                Directory.CreateDirectory(root);
            var dataSet = CourseLab.SetCover.DataGen.Run(root, new List<Tuple<int, int>>()
            {
                new Tuple<int, int>(100, 100),
                new Tuple<int, int>(1000, 1000),
                new Tuple<int, int>(5000, 5000),
            });

            foreach (var dataCase in dataSet)
            {
                var solvers = new List<SetCoverSolution>();

                solvers.Add(new GreedySetCover(dataCase.Item1, dataCase.Item2));
                solvers.Add(new LP2_SetCover(dataCase.Item1, dataCase.Item2));

                foreach (var solver in solvers)
                {
                    var ans = solver.Run();
                    Console.WriteLine(ans.Item2);
                    Console.WriteLine(ans.Item1.Count);
                }
            }

            Console.ReadLine();

            Console.WriteLine();
        }
    }
}
