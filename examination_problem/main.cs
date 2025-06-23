using System;

class MainClass
{
    static void Main()
    {
        Console.WriteLine("2D Integration Examples using Integ2D Class\n");
        // Example 1: ∫₀¹ dx ∫₀¹ dy (x * y) = (1/2)*(1/2) = 0.25
        double result1 = Integ2D.Simpson2D(
            (x, y) => x * y,
            0, 1,
            x => 0,
            x => 1,
            1e-6, 1e-6
        );
        Console.WriteLine("Example 1: ∫₀¹ dx ∫₀¹ dy (x*y)");
        Console.WriteLine($"Computed: {result1}, Expected: 0.25, Error: {Math.Abs(result1 - 0.25)}\n");

        // Example 2: ∫₀¹ dx ∫₀ˣ dy (1) = area of triangle = 1/2
        double result2 = Integ2D.Simpson2D(
            (x, y) => 1,
            0, 1,
            x => 0,
            x => x,
            1e-6, 1e-6
        );
        Console.WriteLine("Example 2: ∫₀¹ dx ∫₀ˣ dy (1)");
        Console.WriteLine($"Computed: {result2}, Expected: 0.5, Error: {Math.Abs(result2 - 0.5)}\n");

        // Example 3: ∫₀^π dx ∫₀¹ dy (sin(x)) = ∫₀^π sin(x) dx = 2 (independent of y)
        double result3 = Integ2D.Simpson2D(
            (x, y) => Math.Sin(x),
            0, Math.PI,
            x => 0,
            x => 1,
            1e-6, 1e-6
        );
        Console.WriteLine("Example 3: ∫₀^π dx ∫₀¹ dy (sin(x))");
        Console.WriteLine($"Computed: {result3}, Expected: 2, Error: {Math.Abs(result3 - 2)}\n");

        // Oscillatory test: cos(10(x + y)) over [0,1]x[0,1]
        double result4 = Integ2D.Simpson2D(
            (x, y) => Math.Cos(10 * (x + y)),
            0, 1,
            x => 0,
            x => 1,
            1e-8, 1e-8
        );
        double expected4 = (Math.Sin(20) - 2 * Math.Sin(10)) / 100.0;
        Console.WriteLine("Example 4: ∫₀¹ dx ∫₀¹ dy cos(10(x+y))");
        Console.WriteLine($"Computed: {result4}, Expected: {expected4}, Error: {Math.Abs(result4 - expected4)}\n");

        // Gaussian bump: e^{-(x^2 + y^2)} over [-2,2]x[-2,2]
        double result5 = Integ2D.Simpson2D(
            (x, y) => Math.Exp(-(x * x + y * y)),
            -2, 2,
            x => -2,
            x => 2,
            1e-6, 1e-6
        );
        double resultGrid5 = Integ2D.IntegrateGrid(
            (x, y) => Math.Exp(-(x * x + y * y)),
            -2, 2,
            x => -2,
            x => 2,
            1e-6, 1e-6 // maxDepth
        );
        double erf2 = SpecialErf(2); // we'll define this below
        double expected5 = Math.PI * erf2 * erf2;
        Console.WriteLine("Example 5: ∫_{-2}^{2} dx ∫_{-2}^{2} dy e^{-(x^2 + y^2)}");
        Console.WriteLine($"Computed: {result5}, Expected: {expected5}, Error: {Math.Abs(result5 - expected5)}\n");
        Console.WriteLine($"Computed with grid: {resultGrid5}, Expected: {expected5}, Error: {Math.Abs(result5 - expected5)}\n");

        // Triangular domain: ∫₀¹ dx ∫₀^{1-x} xy dy
        double result6 = Integ2D.Simpson2D(
            (x, y) => x * y,
            0, 1,
            x => 0,
            x => 1 - x,
            1e-6, 1e-6
        );
        double expected6 = 1.0 / 24;
        Console.WriteLine("Example 6: ∫₀¹ dx ∫₀^{1-x} x*y dy");
        Console.WriteLine($"Computed: {result6}, Expected: {expected6}, Error: {Math.Abs(result6 - expected6)}\n");

        // Oscillatory test with grid-based 2D integration
        double resultGrid = Integ2D.IntegrateGrid(
            (x, y) => Math.Cos(10 * (x + y)),
            0, 1,
            x => 0,
            x => 1,
            1e-6, 1e-6,
            10 // maxDepth
        );

        double expectedGrid = (Math.Sin(20) - 2 * Math.Sin(10)) / 100.0;

        Console.WriteLine("Grid-based adaptive integrator (cos(10(x + y))):");
        Console.WriteLine($"Computed: {resultGrid}, Expected: {expectedGrid}, Error: {Math.Abs(resultGrid - expectedGrid)}\n");

        double resultMid = GridMidpoint2D.Integrate(
            (x, y) => Math.Cos(10 * (x + y)),
            0, 1,
            0, 1,
            1e-6, 1e-6
        );

        double expectedMid = (Math.Sin(20) - 2 * Math.Sin(10)) / 100.0;

        Console.WriteLine("Midpoint grid integrator (cos(10(x + y))):");
        Console.WriteLine($"Computed: {resultMid}, Expected: {expectedMid}, Error: {Math.Abs(resultMid - expectedMid)}\n");

    }
    // Approximation to erf(x), accurate enough for our test
    public static double SpecialErf(double x)
    {
        // Abramowitz and Stegun formula 7.1.26
        double t = 1.0 / (1.0 + 0.3275911 * x);
        double tau = t * (0.254829592 +
                        t * (-0.284496736 +
                        t * (1.421413741 +
                        t * (-1.453152027 +
                        t * 1.061405429))));
        return 1.0 - tau * Math.Exp(-x * x);
    }

}
