using System;

class Program
{
    static void Main(string[] args)
    {
        // Rosenbrock's valley function: f(x, y) = (1 - x)^2 + 100 * (y - x^2)^2
        Func<vector, double> rosenbrock = (vector x) =>
        {
            double x1 = x[0];
            double x2 = x[1];
            return Math.Pow(1 - x1, 2) + 100 * Math.Pow(x2 - x1 * x1, 2);
        };

        // Himmelblau's function: f(x, y) = (x^2 + y - 11)^2 + (x + y^2 - 7)^2
        Func<vector, double> himmelblau = (vector x) =>
        {
            double x1 = x[0];
            double x2 = x[1];
            return Math.Pow(x1 * x1 + x2 - 11, 2) + Math.Pow(x1 + x2 * x2 - 7, 2);
        };

        // Accuracy goal
        double accgoal = 1e-3;

        // Find minimum of Rosenbrock's function
        vector rosenbrockInit = new vector(-1.0, 2.0); // Initial guess
        var (rosenbrockMin, rosenSteps) = Min.newton(rosenbrock, rosenbrockInit, accgoal);
        rosenbrockMin.print($"Minimum of Rosenbrock's function found after {rosenSteps} steps at:");

        // Find minima of Himmelblau's function (there are four expected minima)
        vector himmelblauInit1 = new vector(2.0, 2.0); // Near (3,2)
        var (himmelMin1, himmelSteps1) = Min.newton(himmelblau, himmelblauInit1, accgoal);
        himmelMin1.print($"Minimum of Himmelblau's function found after {himmelSteps1} steps at:");

        vector himmelblauInit2 = new vector(-2.5, 3.0); // Near (-2.805, 3.131)
        var (himmelMin2, himmelSteps2) = Min.newton(himmelblau, himmelblauInit2, accgoal);
        himmelMin2.print($"Minimum of Himmelblau's function found after {himmelSteps2} steps at:");

        vector himmelblauInit3 = new vector(-3.5, -3.0); // Near (-3.779, -3.283)
        var (himmelMin3, himmelSteps3) = Min.newton(himmelblau, himmelblauInit3, accgoal);
        himmelMin3.print($"Minimum of Himmelblau's function found after {himmelSteps3} steps at:");

        vector himmelblauInit4 = new vector(3.5, -2.0); // Near (3.584, -1.848)
        var (himmelMin4, himmelSteps4) = Min.newton(himmelblau, himmelblauInit4, accgoal);
        himmelMin4.print($"Minimum of Himmelblau's function found after {himmelSteps4} steps at:");
    
        vector himmelblauInit5 = new vector(-0.2, -0.8); // Near (3.584, -1.848)
        var (himmelMin5, himmelSteps5) = Min.newton(himmelblau, himmelblauInit5, accgoal, 3000);
        himmelMin5.print($"Minimum of Himmelblau's function found after {himmelSteps5} steps at:");
    }
}