using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseLab.ConvexHull
{
    public abstract class ConvexHullSolution : Solution<List<Point>>
    {
        public const double EPS = 1e-6;

        public List<Point> points;
        public int Count { get { return points.Count; } }

        public ConvexHullSolution(List<Point> points)
        {
            this.points = points;
        }
    }
}
