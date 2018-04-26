using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvexHull
{
    public abstract class Solution
    {
        public const double EPS = 1e-6;

        public List<Point> points;
        public int Count { get { return points.Count; } }

        public Solution(List<Point> points)
        {
            this.points = points;
        }

        abstract protected List<Point> Solve();

        public Tuple<List<Point>, TimeSpan> Run()
        {
            var startTime = DateTime.Now;
            var result = Solve();
            return new Tuple<List<Point>, TimeSpan>(result, DateTime.Now - startTime);
        }
    }
}
