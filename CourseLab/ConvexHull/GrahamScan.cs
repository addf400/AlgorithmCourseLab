using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseLab.ConvexHull
{
    public class GrahamScan : ConvexHullSolution
    {
        public GrahamScan(List<Point> points) : base(points)
        {
        }

        protected override List<Point> Solve()
        {
            var p = points.OrderBy(_ => _).ToArray();
            var stack = new Point[Count];
            int top = 0;
            stack[top++] = p[0];
            for (int i = 1; i < Count; ++i)
            {
                while (top >= 2 && Vector.Cross(stack[top - 1] - stack[top - 2], p[i] - stack[top - 2]) <= 0)
                    --top;
                stack[top++] = p[i];
            }

            int lim = top;

            for (int i = Count - 2; i >= 0; --i)
            {
                while (top > lim && Vector.Cross(stack[top - 1] - stack[top - 2], p[i] - stack[top - 2]) <= 0)
                    --top;
                stack[top++] = p[i];
            }

            return stack.ToList().GetRange(0, top - 1);
        }

        public override string GetMethodName()
        {
            return "GrahamScan";
        }
    }
}
