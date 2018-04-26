using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvexHull
{
    class Program
    {
        static void Main(string[] args)
        {
            string root = @"D:\users\v-hanbao\C#\Data";
            var dataSet = DataGen.Run(root, new int[] { 10, 100, 200, 500, 1000, 2000, 3000, 4000, 5000 });

            foreach (var dataCase in dataSet)
            {
                var points = Point.LoadPointsFromFile(dataCase.Item1);
                var bruteForce = new BruteForce(points);
                var ans = bruteForce.Run();
                Point.SavePointsToFile(ans.Item1, Path.Combine(root, String.Format("Ans-{0}.txt", dataCase.Item2)));
                Console.WriteLine(ans.Item2);
                Console.WriteLine(ans);
            }
        }
    }
}
