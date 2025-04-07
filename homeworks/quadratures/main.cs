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