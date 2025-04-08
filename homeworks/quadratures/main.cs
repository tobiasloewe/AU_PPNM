using System;

class main{
    static void Main(){

        // Example 1
        double result = Quadratures.integrate(
            (x) => Math.Sqrt(x), 0, 1, 0.001, 0.001, double.NaN, double.NaN
        );
        Console.WriteLine($"∫_0^1 √(x) dx = {result} (Expected: 2/3)");

        // Example 2
        result = Quadratures.integrate(
            (x) => 1 / Math.Sqrt(x), 0, 1, 0.001, 0.001, double.NaN, double.NaN
        );
        Console.WriteLine($"∫_0^1 1/√(x) dx = {result} (Expected: 2)");

        // Example 3
        result = Quadratures.integrate(
            (x) => Math.Sqrt(1 - x * x), 0, 1, 0.001, 0.001, double.NaN, double.NaN
        );
        Console.WriteLine($"∫_0^1 √(1-x²) dx = {result} (Expected: π/2)");

        // Example 4
        result = Quadratures.integrate(
            (x) => Math.Log(x) / Math.Sqrt(x), 0, 1, 0.001, 0.001, double.NaN, double.NaN
        );
        Console.WriteLine($"∫_0^1 ln(x)/√(x) dx = {result} (Expected: -4)");

        // Example 5: Error function implementation
        

        // Test the error function
        double z = 0.5;
        result = erf(z);
        Console.WriteLine($"erf({z}) = {result} (Expected: ~0.5205)");

        z = 1.5;
        result = erf(z);
        Console.WriteLine($"erf({z}) = {result} (Expected: ~0.9661)");

        // Additional test cases for erf
        z = 0.0;
        result = erf(z);
        Console.WriteLine($"erf({z}) = {result} (Expected: 0)");

        z = 2.0;
        result = erf(z);
        Console.WriteLine($"erf({z}) = {result} (Expected: ~0.9953)");

        z = -1.0;
        result = erf(z);
        Console.WriteLine($"erf({z}) = {result} (Expected: ~-0.8427)");

        // Calculate erf(1) with varying accuracy and write results to a file   
        using (System.IO.StreamWriter file = new System.IO.StreamWriter("erf1.txt"))
        {
            double exactResult = 0.84270079294971486934; // Exact value of erf(1)
            double acc = 0.1;

            while (acc >= 1e-16)
            {
            double computedResult = erf(1, acc,0.0);
            double error = Math.Abs(computedResult - exactResult);
            file.WriteLine($"{acc} {error}");
            acc /= 10; // Decrease accuracy
            }
        }
        // Example 6: Using CCintegrate for comparison
        Quadratures.ResetEvaluationCount();
        result = Quadratures.CCintegrate(
            (x) => 1 / Math.Sqrt(x), 0, 1, 1e-6, 1e-6
        );
        Console.WriteLine($"CCintegrate ∫_0^1 1/√(x) dx = {result} (Expected: 2), Evaluation Count: {Quadratures.EvaluationCount}, Python (Scipy): 231");
        
        Quadratures.ResetEvaluationCount();
        result = Quadratures.CCintegrate(
            (x) => Math.Log(x) / Math.Sqrt(x), 0, 1, 0.0001, 0.0001
        );
        Console.WriteLine($"CCintegrate ∫_0^1 ln(x)/√(x) dx = {result} (Expected: -4), Evaluation Count: {Quadratures.EvaluationCount}, Python (Scipy): 315");

        // Example 7: Infinite limit integral ∫_0^∞ e^(-x) dx (Expected: 1)
        Quadratures.ResetEvaluationCount();
        result = Quadratures.integrate(
            (x) => Math.Exp(-x), 0, double.PositiveInfinity, 1e-6, 1e-6, double.NaN, double.NaN
        );
        Console.WriteLine($"∫_0^∞ e^(-x) dx = {result} (Expected: 1), Evaluation Count: {Quadratures.EvaluationCount}, Python (Scipy): 135");

        // Example 8: Infinite limit integral ∫_0^∞ x^2 e^(-x) dx (Expected: 2)
        Quadratures.ResetEvaluationCount();
        result = Quadratures.integrate(
            (x) => x * x * Math.Exp(-x), 0, double.PositiveInfinity, 1e-6, 1e-6, double.NaN, double.NaN
        );
        Console.WriteLine($"∫_0^∞ x^2 e^(-x) dx = {result} (Expected: 2), Evaluation Count: {Quadratures.EvaluationCount}, Python (Scipy): 165");

        // Example 9: Infinite limit integral ∫_0^∞ e^(-x^2) dx (Expected: √π/2)
        Quadratures.ResetEvaluationCount();
        result = Quadratures.integrate(
            (x) => Math.Exp(-x * x), 0, double.PositiveInfinity, 1e-6, 1e-6, double.NaN, double.NaN
        );
        Console.WriteLine($"∫_0^∞ e^(-x²) dx = {result} (Expected: {Math.Sqrt(Math.PI) / 2}), Evaluation Count: {Quadratures.EvaluationCount}, Python (Scipy): 135");
    
    }
    static double erf(double z, double acc = 1e-6, double eps = 1e-6) {
        if (z < 0) {
        return -erf(-z);
        } else if (z <= 1) {
        return 2 / Math.Sqrt(Math.PI) * Quadratures.integrate(
            (x) => Math.Exp(-x * x), 0, z, acc, eps, double.NaN, double.NaN
        );
        } else {
        return 1 - 2 / Math.Sqrt(Math.PI) * Quadratures.integrate(
            (t) => Math.Exp(-Math.Pow(z + (1 - t) / t, 2) / (t * t)) / t, 0, 1, acc, eps, double.NaN, double.NaN
        );
        }
    }
}