using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseLab.ConvexHull
{
    public class DataGen
    {
        public static Random rand = new Random(12345678);
        public static double XRange = 100.0;
        public static double YRange = 100.0;

        public static Point GenPoint()
        {
            return new Point(rand.NextDouble() * XRange, rand.NextDouble() * YRange);
        }

        public static void GenPointsToFile(int pointsCount, string filename)
        {
            using (var writer = new StreamWriter(filename, false, Encoding.UTF8))
            {
                for (int i = 0; i < pointsCount; ++i)
                    writer.WriteLine(GenPoint());
            }
        }

        public static IEnumerable<Tuple<string, int>> Run(string root, IEnumerable<int> ranges)
        {
            if (!Directory.Exists(root))
                Directory.CreateDirectory(root);

            foreach (var range in ranges)
            {
                string filename = Path.Combine(root, String.Format("{0}-points.XRange-{1}.YRange-{2}.txt", range, XRange, YRange));
                if (!File.Exists(filename))
                    GenPointsToFile(range, filename);

                yield return new Tuple<string, int>(filename, range);
            }
        }
    }
}
