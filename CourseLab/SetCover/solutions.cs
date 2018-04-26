using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseLab.SetCover
{
    public class GreedySetCover : SerCoverSolution
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
}
