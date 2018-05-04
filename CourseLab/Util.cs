using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseLab
{
    public abstract class Solution<T>
    {
        abstract protected T Solve();

        public Tuple<T, TimeSpan> Run()
        {
            var startTime = DateTime.Now;
            var result = Solve();
            return new Tuple<T, TimeSpan>(result, DateTime.Now - startTime);
        }

        public abstract string GetMethodName();
    }
}
