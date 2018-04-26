using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvexHull
{
    public class Point
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
    }

    public class Vector
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
            return A.x * B.y - A.y - B.x;
        }
    }
}
