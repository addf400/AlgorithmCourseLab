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

        public double Ang
        {
            get
            {
                double t = y / x;
                return t;
            }
        }

        public static double FLAG;

        /// <summary>
        /// 以二、三象限的某个点作为基准划分区域
        /// </summary>
        public int Direction
        {
            get
            {
                if (x >= 0)
                    return 1;
                if (Ang < FLAG)
                    return 2;
                return 0;
            }
        }

        /// <summary>
        /// 极角比较先看Direction，再看y/x
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            var p = obj as Vector;
            if (Direction != p.Direction)
                return Direction.CompareTo(p.Direction);
            return Ang.CompareTo(p.Ang);
        }

        public override string ToString()
        {
            return String.Format("{0} {1}", x, y);
        }
    }
}
