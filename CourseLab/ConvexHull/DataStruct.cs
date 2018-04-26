using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseLab.ConvexHull
{
    public class Point : IComparable
    {
        public double x;
        public double y;

        public Point(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public static Vector operator -(Point A, Point B)
        {
            return new Vector(A.x - B.x, A.y - B.y);
        }

        public static Point operator -(Point A, Vector B)
        {
            return new Point(A.x - B.x, A.y - B.y);
        }

        public static Point operator +(Point A, Vector B)
        {
            return new Point(A.x + B.x, A.y + B.y);
        }

        public override string ToString()
        {
            return String.Format("{0} {1}", x, y);
        }

        public static Point LoadFromString(string str)
        {
            var parts = str.Split(' ');
            return new Point(double.Parse(parts.First()), double.Parse(parts.Last()));
        }

        public static List<Point> LoadPointsFromFile(string filename)
        {
            return (from line in File.ReadAllLines(filename, Encoding.UTF8)
                    select LoadFromString(line)).ToList();
        }

        public static void SavePointsToFile(IEnumerable<Point> points, string filename)
        {
            File.WriteAllLines(filename, points.Select(_ => _.ToString()), Encoding.UTF8);
        }

        int IComparable.CompareTo(object obj)
        {
            var p = obj as Point;
            var _ = x.CompareTo(p.x);
            if (_ != 0)
                return _;
            else
                return y.CompareTo(p.y);
        }
    }

    public class Vector : IComparable
    {
        public double x;
        public double y;

        public Vector(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public static double Dot(Vector A, Vector B)
        {
            return A.x * B.x + A.y * B.y;
        }

        public static double Cross(Vector A, Vector B)
        {
            return A.x * B.y - A.y * B.x;
        }

        public int CompareTo(object obj)
        {
            var p = obj as Vector;
            return (y / x).CompareTo(p.y / p.x);
        }

        public override string ToString()
        {
            return String.Format("{0} {1}", x, y);
        }
    }
}
