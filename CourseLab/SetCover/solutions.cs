using Extreme.Mathematics;
using Extreme.Mathematics.Optimization;
using Microsoft.SolverFoundation.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseLab.SetCover
{
    public class GreedySetCover : SetCoverSolution
    {
        public GreedySetCover(string setfile, int range) : base(setfile, range)
        {
        }

        protected override List<int[]> Solve()
        {
            var used = new BitArray(range);
            var setLabel = new BitArray(sets.Count);
            int usedCount = 0;

            var ans = new List<int[]>();

            while (usedCount < range)
            {
                int maxSetIdx = -1;
                int maxSetCost = 0;
                
                for (int i = 0; i < sets.Count; ++i)
                    if (!setLabel[i])
                    {
                        int cost = sets[i].Count(item => !used[item]);
                        if (cost > maxSetCost)
                        {
                            maxSetCost = cost;
                            maxSetIdx = i;
                        }
                    }

                if (maxSetIdx == -1)
                    throw new InvalidProgramException();

                foreach (var item in sets[maxSetIdx])
                {
                    if (!used[item])
                    {
                        ++usedCount;
                        used[item] = true;
                    }
                }

                ans.Add(sets[maxSetIdx]);
                setLabel[maxSetIdx] = true;
            }

            return ans;
        }
    }

    public class LP_SetCover : SetCoverSolution
    {
        public LP_SetCover(string setfile, int range) : base(setfile, range)
        {
        }

        protected override List<int[]> Solve()
        {
            SolverContext context = SolverContext.GetContext();
            Model model = context.CreateModel();

            List<Decision> x = new List<Decision>();
            var itemToSets = new Dictionary<int, HashSet<int>>();

            for (int i = 0; i < sets.Count; ++i)
            {
                x.Add(new Decision(Domain.Real, "x_" + i.ToString()));
                model.AddDecision(x[i]);
                model.AddConstraint("self_C_" + i.ToString(), x[i] >= 0);

                foreach (var item in sets[i])
                {
                    HashSet<int> setIds;
                    if (!itemToSets.TryGetValue(item, out setIds))
                    {
                        itemToSets.Add(item, setIds = new HashSet<int>());
                    }
                    setIds.Add(i);
                }
            }

            int rangeCount = 0;

            foreach (var pair in itemToSets)
            {
                bool flag = true;
                Term term = null;
                foreach (var setId in pair.Value)
                {
                    if (flag)
                    {
                        term = x[setId];
                        flag = false;
                    }
                    else
                        term = term + x[setId];
                }
                term = term >= 1;
                model.AddConstraint("C_" + rangeCount++, term);
            }

            Term goal = x[0];
            for (int i = 1; i < rangeCount; ++i)
                goal = goal + x[i];
            Goal gmin = model.AddGoal("zmin", GoalKind.Minimize, goal);

            Solution solution = context.Solve();

            List<int[]> ans = new List<int[]>();
            for (int i = 0; i < sets.Count; ++i)
            {
                double value = x[i].ToDouble();
                if (sets[i].Any(item => value > 1 / itemToSets[item].Count))
                    ans.Add(sets[i]);
            }

            context.ClearModel();

            return ans;
        }
    }

    public class LP2_SetCover : SetCoverSolution
    {
        public LP2_SetCover(string setfile, int range) : base(setfile, range)
        {
        }

        protected override List<int[]> Solve()
        {
            var model = new LinearProgram();

            var itemToSets = new Dictionary<int, HashSet<int>>();

            for (int i = 0; i < sets.Count; ++i)
            {
                model.AddVariable("x_" + i.ToString(), 1.0, 0, 1);

                foreach (var item in sets[i])
                {
                    HashSet<int> setIds;
                    if (!itemToSets.TryGetValue(item, out setIds))
                    {
                        itemToSets.Add(item, setIds = new HashSet<int>());
                    }
                    setIds.Add(i);
                }
            }

            int rangeCount = 0;

            foreach (var pair in itemToSets)
            {
                double[] vector = new double[sets.Count];
                foreach (var item in pair.Value)
                    vector[item] = 1.0;

                model.AddLinearConstraint("C_" + rangeCount++, Vector.Create(vector), 1.0, double.PositiveInfinity);
            }

            var x = model.Solve();
            
            List<int[]> ans = new List<int[]>();
            for (int i = 0; i < sets.Count; ++i)
            {
                double value = x[i];
                if (sets[i].Any(item => value > 1 / itemToSets[item].Count))
                    ans.Add(sets[i]);
            }

            return ans;
        }
    }
}
