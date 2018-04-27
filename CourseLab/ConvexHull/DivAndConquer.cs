using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseLab.ConvexHull
{
    class DivAndConquer : ConvexHullSolution
    {
        public DivAndConquer(List<Point> points) : base(points)
        {
        }
        
        static void Swap(List<Point> list, int i, int j)
        {
            var tmp = list[i];
            list[i] = list[j];
            list[j] = tmp;
        }

        IEnumerable<Point> MergePointList(List<Point> left, List<Point> right, Point o)
        {
            int l = 0, r = 0;
            while (l < left.Count && r < right.Count)
            {
                if ((left[l] - o).CompareTo(right[r] - o) < 0)
                    yield return left[l++];
                else
                    yield return right[r++];
            }
            while (l < left.Count)
                yield return left[l++];

            while (r < right.Count)
                yield return right[r++];
        }

        List<Point> Conquer(List<Point> left, List<Point> right)
        {
            Point o = GetMeanPoint(left);

            int minXIdx = 0;
            for (int i = 1; i < left.Count; ++i)
                if (left[minXIdx].x > left[i].x)
                    minXIdx = i;

            var fixedLeft = left.GetRange(minXIdx, left.Count - minXIdx);
            fixedLeft.AddRange(left.GetRange(0, minXIdx));

            int minAngIdx = 0;
            int maxAngIdx = 0;
            for (int i = 1; i < right.Count; ++i)
            {
                if ((right[minAngIdx] - o).CompareTo(right[i] - o) > 0)
                    minAngIdx = i;
                if ((right[maxAngIdx] - o).CompareTo(right[i] - o) < 0)
                    maxAngIdx = i;
            }

            List<Point> a = null;
            List<Point> b = null;
            if (minAngIdx > maxAngIdx)
            {
                a = right.GetRange(minAngIdx, right.Count - minAngIdx);
                a.AddRange(right.GetRange(0, maxAngIdx));
                b = right.GetRange(maxAngIdx, right.Count - a.Count);
            }
            else
            {
                b = right.GetRange(maxAngIdx, right.Count - maxAngIdx);
                b.AddRange(right.GetRange(0, minAngIdx));
                a = right.GetRange(minAngIdx, right.Count - b.Count);
            }
            b.Reverse();

            Vector.FLAG = (fixedLeft.First() - o).Ang;
            var p = MergePointList(fixedLeft.GetRange(1, fixedLeft.Count - 1), MergePointList(a, b, o).ToList(), o).ToList();

            var stack = new Point[p.Count + 2];
            p.Add(fixedLeft.First());
            int top = 0;
            stack[top++] = fixedLeft.First();
            for (int i = 0; i < p.Count; ++i)
            {
                while (top >= 2 && Vector.Cross(stack[top - 1] - stack[top - 2], p[i] - stack[top - 2]) <= 0)
                    --top;
                stack[top++] = p[i];
            }

            return stack.ToList().GetRange(0, top - 1);
        }

        void FindMid(List<Point> p, int l, int r, int k)
        {
            if (l >= r)
                return;
            var key = p[l];
            int left = l, right = r;
            while (left < right)
            {
                while (left < right && p[right].x >= key.x)
                    --right;
                p[left] = p[right];
                while (left < right && p[left].x <= key.x)
                    ++left;
                p[right] = p[left];
            }
            p[left] = key;

            if (left == k)
                return;
            else if (left > k)
                FindMid(p, l, left - 1, k);
            else
                FindMid(p, left + 1, r, k);
        }

        public Point GetMeanPoint(List<Point> p)
        {
            double _x = p.Sum(_ => _.x) / p.Count;
            double _y = p.Sum(_ => _.y) / p.Count;
            return  new Point(_x, _y);
        }

        List<Point> GetAns(List<Point> p)
        {
            if (p.Count <= 3)
            {
                var c = GetMeanPoint(p);
                return p.OrderBy(_ => _ - c).ToList();
            }

            int mid = (p.Count - 1) / 2;
            FindMid(p, 0, p.Count - 1, p.Count / 2);

            var left = GetAns(p.GetRange(0, mid + 1));
            var right = GetAns(p.GetRange(mid + 1, p.Count - mid - 1));

            return Conquer(left, right);
        }

        protected override List<Point> Solve()
        {
            return GetAns(points);
        }

        public override string GetMethodName()
        {
            return "DivAndConquer";
        }
    }
}
