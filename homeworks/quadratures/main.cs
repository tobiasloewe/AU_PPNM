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
    }
    static double erf(double z) {
        if (z < 0) {
        return -erf(-z);
        } else if (z <= 1) {
        return 2 / Math.Sqrt(Math.PI) * Quadratures.integrate(
            (x) => Math.Exp(-x * x), 0, z, 1e-6, 1e-6, double.NaN, double.NaN
        );
        } else {
        return 1 - 2 / Math.Sqrt(Math.PI) * Quadratures.integrate(
            (t) => Math.Exp(-Math.Pow(z + (1 - t) / t, 2) / (t * t)) / t, 0, 1, 1e-8, 1e-8, double.NaN, double.NaN
        );
        }
    }
}