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
        }

        /// <summary>
        /// 凸包算法测试
        /// </summary>
        static void ConvexHull()
        {
            string root = Path.Combine("..", "Data");
            StreamWriter logger = new StreamWriter(Path.Combine(root, "ch.log"));

            var dataRange = new List<int>();
            for (int i = 1000; i <= 20000; i += 1000)
            {
                dataRange.Add(i);
            }
            var dataSet = CourseLab.ConvexHull.DataGen.Run(root, dataRange);
            // 生成数据，惰性

            foreach (var dataCase in dataSet)
            {
                var solvers = new List<ConvexHullSolution>();
                var points = Point.LoadPointsFromFile(dataCase.Item1);
                solvers.Add(new BruteForce(points));
                solvers.Add(new GrahamScan(points));
                solvers.Add(new DivAndConquer(points));
                // 设定求解方法
                
                foreach (var solver in solvers)
                {
                    var ans = solver.Run();
                    // 求解

                    Point.SavePointsToFile(ans.Item1, 
                        Path.Combine(root, String.Format("Ans-{0}-{1}.txt", solver.GetMethodName(), dataCase.Item2)));
                    // 保存解

                    logger.WriteLine("{0}\t{1}\t{2}\t{3}",
                        solver.GetMethodName(), dataCase.Item2, ans.Item2, ans.Item1.Count);

                    Console.WriteLine("Solution: [{0}]\tN: {1}\tTime consume: {2}\tAns: {3}", 
                        solver.GetMethodName(), dataCase.Item2, ans.Item2, ans.Item1.Count);
                    // 打印耗时等信息
                }
            }

            Console.WriteLine("Calc done, press any key to continue... ");
            Console.ReadLine();
            logger.Close();
        }

        static void SetCover()
        {
            string root = Path.Combine("..", "Data");
            StreamWriter logger = new StreamWriter(Path.Combine(root, "sc.log"));

            var dataSet = CourseLab.SetCover.DataGen.Run(root, new List<Tuple<int, int>>()
            {
                new Tuple<int, int>(100, 100),
                new Tuple<int, int>(1000, 1000),
                new Tuple<int, int>(5000, 5000),
            });
            // 生成数据，惰性

            foreach (var dataCase in dataSet)
            {
                var solvers = new List<SetCoverSolution>();

                solvers.Add(new GreedySetCover(dataCase.Item1, dataCase.Item2));
                solvers.Add(new LP2_SetCover(dataCase.Item1, dataCase.Item2));
                // 设定求解方法

                foreach (var solver in solvers)
                {
                    var ans = solver.Run();
                    // 求解

                    logger.WriteLine("{0}\t{1}\t{2}\t{3}",
                        solver.GetMethodName(), dataCase.Item2, ans.Item2, ans.Item1.Count);

                    Console.WriteLine("Solution: [{0}]\tN: {1}\tTime consume: {2}\tAns: {3}",
                        solver.GetMethodName(), dataCase.Item2, ans.Item2, ans.Item1.Count);
                    // 打印耗时等信息
                }
            }

            Console.WriteLine("Calc done, press any key to continue... ");
            Console.ReadLine();
            logger.Close();
        }
    }
}
