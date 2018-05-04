using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseLab
{
    /// <summary>
    /// 最基础的Solution类，封装时间计时功能，以及规定方法名
    /// </summary>
    /// <typeparam name="T">解的描述</typeparam>
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
