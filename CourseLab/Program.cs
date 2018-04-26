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
            ConvexHull();
            //SetCover();
            //test.Run();
        }

        static void ConvexHull()
        {
            string root = @"C:\Users\addf4\Desktop\Code\AlgrithmCouseLabData";
            var dataSet = CourseLab.ConvexHull.DataGen.Run(root, new int[] { 10, 100, 200, 500, 1000, 2000, 3000, 4000, 5000, 6000, 7000, 8000, 10000 });

            foreach (var dataCase in dataSet)
            {
                var points = Point.LoadPointsFromFile(dataCase.Item1);
                var bruteForce = new GrahamScan(points);
                var ans = bruteForce.Run();
                Point.SavePointsToFile(ans.Item1, Path.Combine(root, String.Format("Ans-{0}.txt", dataCase.Item2)));
                Console.WriteLine(ans.Item2);
                Console.WriteLine(ans);
            }
        }

        static void SetCover()
        {
            string root = @"D:\users\v-hanbao\C#\Data";
            var dataSet = CourseLab.SetCover.DataGen.Run(root, new List<Tuple<int, int>>()
            {
                new Tuple<int, int>(100, 100),
                new Tuple<int, int>(1000, 1000),
                new Tuple<int, int>(5000, 5000),
            });

            foreach (var dataCase in dataSet)
            {
                var greedySetCover = new GreedySetCover(dataCase.Item1, dataCase.Item2);
                var ans = greedySetCover.Run();
                Console.WriteLine(ans.Item2);
                Console.WriteLine(ans.Item1.Count);
            }

            Console.WriteLine();
        }
    }
}
