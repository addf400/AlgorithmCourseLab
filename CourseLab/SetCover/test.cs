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
    class test
    {
        public static void Test()
        {
            var c = Vector.Create(-1.0, -3.0, 0.0, 0.0, 0.0, 0.0);
            // The coefficients of the constraints:
            var A = Matrix.Create(4, 6, new double[]
            {
                1, 1, 1, 0, 0, 0,
                1, 1, 0, -1, 0, 0,
                1, 0, 0, 0, 1, 0,
                0, 1, 0, 0, 0, 1
            }, MatrixElementOrder.RowMajor);

            // The right-hand sides of the constraints:
            var b = Vector.Create(1.5, 0.5, 1.0, 1.0);

            // We're now ready to call the constructor.
            // The last parameter specifies the number of equality
            // constraints.
            LinearProgram lp1 = new LinearProgram(c, A, b, 4);

            // Now we can call the Solve method to run the Revised
            // Simplex algorithm:
            var x = lp1.Solve();
            // The GetDualSolution method returns the dual solution:
            var y = lp1.GetDualSolution();
            Console.WriteLine("Primal: {0:F1}", x);
            Console.WriteLine("Dual:   {0:F1}", y);
            // The optimal value is returned by the Extremum property:
            Console.WriteLine("Optimal value:   {0:F1}", lp1.OptimalValue);

            // The second way to create a Linear Program is by constructing
            // it by hand. We start with an 'empty' linear program.
            LinearProgram lp2 = new LinearProgram();

            // Next, we add two variables: we specify the name, the cost,
            // and optionally the lower and upper bound.
            lp2.AddVariable("X1", -1.0, 0, 1);
            lp2.AddVariable("X2", -3.0, 0, 1);

            // Next, we add constraints. Constraints also have a name.
            // We also specify the coefficients of the variables,
            // the lower bound and the upper bound.
            lp2.AddLinearConstraint("C1", Vector.Create(1.0, 1.0), 0.5, 1.5);
            // If a constraint is a simple equality or inequality constraint,
            // you can supply a LinearProgramConstraintType value and the
            // right-hand side of the constraint.

            // We can now solve the linear program:
            x = lp2.Solve();
            y = lp2.GetDualSolution();
            Console.WriteLine("Primal: {0:F1}", x);
            Console.WriteLine("Dual:   {0:F1}", y);
            Console.WriteLine("Optimal value:   {0:F1}", lp2.OptimalValue);

            Console.Write("Press Enter key to exit...");
            Console.ReadLine();
        }

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
