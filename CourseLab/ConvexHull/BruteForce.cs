using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseLab.ConvexHull
{
    public class BruteForce : ConvexHullSolution
    {
        public BruteForce(List<Point> points) : base(points)
        {
        }

        int Judge(Tuple<Vector, int>[] _vectors)
        {
            var vectors = _vectors.OrderBy(_ => _).ToArray();
            var big = Vector.Cross(vectors[0].Item1, vectors[2].Item1);
            var s1 = Vector.Cross(vectors[0].Item1, vectors[1].Item1);
            var s2 = Vector.Cross(vectors[1].Item1, vectors[2].Item1);
            if (s1 + s2 + EPS <= big)
                return vectors[1].Item2;
            else
                return -1;
        }

        Tuple<List<Point>, List<Point>> GetAnswer()
        {
            var p = points.OrderBy(_ => _).ToArray();
            var abandon = new bool[Count];
            var vectors = new Tuple<Vector, int>[3];
            for (int i = 1; i < Count; ++i)
            {
                vectors[0] = new Tuple<Vector, int>(p[i] - p[0], i);
                for (int j = i + 1; j < Count && !abandon[i]; ++j)
                {
                    vectors[1] = new Tuple<Vector, int>(p[j] - p[0], j);
                    for (int k = j + 1; k < Count && !abandon[i] && !abandon[j]; ++k)
                    {
                        vectors[2] = new Tuple<Vector, int>(p[k] - p[0], k);
                        var idx = Judge(vectors);
                        if (idx >= 0)
                            abandon[idx] = true;
                    }
                }
            }

            var forward = new List<Point>();
            var backward = new List<Point>();
            var selectPoints = new List<Point>();
            for (int i = 0; i < Count; ++i)
                if (!abandon[i])
                    selectPoints.Add(p[i]);

            var bottom = selectPoints.Min();
            var top = selectPoints.Max();
            var splitVector = top - bottom;

            foreach (var item in selectPoints)
            {
                if (Vector.Cross(splitVector, item - bottom) <= EPS)
                    forward.Add(item);
                else
                    backward.Add(item);
            }

            backward.Reverse();

            return new Tuple<List<Point>, List<Point>>(forward, backward);
        }

        protected override List<Point> Solve()
        {
            var ans = GetAnswer();
            ans.Item1.AddRange(ans.Item2);
            return ans.Item1;
        }


        public override string GetMethodName()
        {
            return "BruteForce";
        }
    }
}
