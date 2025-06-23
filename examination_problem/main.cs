using System;

class MainClass
{
    static void Main()
    {
        // Example 1: ∫₀¹ dx ∫₀¹ dy (x * y) = (1/2)*(1/2) = 0.25
        double result1 = Integ2D.Integrate2D(
            (x, y) => x * y,
            0, 1,
            x => 0,
            x => 1,
            1e-6, 1e-6
        );
        Console.WriteLine("Example 1: ∫₀¹ dx ∫₀¹ dy (x*y)");
        Console.WriteLine($"Computed: {result1}, Expected: 0.25, Error: {Math.Abs(result1 - 0.25)}\n");

        // Example 2: ∫₀¹ dx ∫₀ˣ dy (1) = area of triangle = 1/2
        double result2 = Integ2D.Integrate2D(
            (x, y) => 1,
            0, 1,
            x => 0,
            x => x,
            1e-6, 1e-6
        );
        Console.WriteLine("Example 2: ∫₀¹ dx ∫₀ˣ dy (1)");
        Console.WriteLine($"Computed: {result2}, Expected: 0.5, Error: {Math.Abs(result2 - 0.5)}\n");

        // Example 3: ∫₀^π dx ∫₀¹ dy (sin(x)) = ∫₀^π sin(x) dx = 2 (independent of y)
        double result3 = Integ2D.Integrate2D(
            (x, y) => Math.Sin(x),
            0, Math.PI,
            x => 0,
            x => 1,
            1e-6, 1e-6
        );
        Console.WriteLine("Example 3: ∫₀^π dx ∫₀¹ dy (sin(x))");
        Console.WriteLine($"Computed: {result3}, Expected: 2, Error: {Math.Abs(result3 - 2)}\n");
    }
}
