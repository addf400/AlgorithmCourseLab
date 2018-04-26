using Microsoft.SolverFoundation.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseLab.SetCover
{
    class test
    {
        public static void Run()
        {
            BitArray bitArray;

            SolverContext context = SolverContext.GetContext();
            //创建模型
            Model model = context.CreateModel();
            //优化决策因子，变量为实数,Domain.Integer|Domain.IntegerNonnegative为整数优化
            Decision x = new Decision(Domain.Real, "x");
            Decision y = new Decision(Domain.Real, "y");
            //添加
            model.AddDecisions(x, y);
            //x,y变量范围
            // model.AddConstraints("变量范围",
            // double.NegativeInfinity < x <= double.PositiveInfinity,
            //double.NegativeInfinity < y <= double.PositiveInfinity);

            model.AddConstraints("约束",
            double.NegativeInfinity < x <= double.PositiveInfinity,
           double.NegativeInfinity < y <= double.PositiveInfinity,
            2 * x + y - 2 >= 0,
            x - 2 * y + 4 >= 0,
           3 * x - y - 3 <= 0);
            //目标函数 min z=x * x + y * y , GoalKind.Minimize最小值
            Goal gmin = model.AddGoal("zmin", GoalKind.Minimize, x * x + y * y);

            //优化
            Solution solution = context.Solve();
            //优化报告
            Report report = solution.GetReport();

            string s = string.Format("min [ x={0:N2},y={1:N2}", x.ToDouble().ToString("0.00"), y.ToDouble().ToString("0.00"));
            s += string.Format(",min={0} ] ", solution.Goals.First<Goal>().ToDouble().ToString("0.00"));

            //=================================================================
            model.RemoveGoal(gmin);
            Goal gmax = model.AddGoal("zmax", GoalKind.Maximize, x * x + y * y);
            //优化
            solution = context.Solve();
            s += string.Format("| max[ x={0:N2},y={1:N2}", x.ToDouble().ToString("0.00"), y.ToDouble().ToString("0.00"));
            s += string.Format(",max={0} ] ", solution.Goals.First<Goal>().ToDouble().ToString("0.00"));
            //--------------------------------------------------------------------------------------------------------------------------------

            context.ClearModel();
        }
    }
}
