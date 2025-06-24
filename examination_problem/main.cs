using System;

class MainClass
{
    static void Main()
    {
        // funcs to integrate 
        Func<double, double, double> func1 = (x, y) => x * y; // Example 1
        Func<double, double, double> func2 = (x, y) => 1; // Example 2
        Func<double, double, double> func3 = (x, y) => Math.Sin(x); // Example 3
        Func<double, double, double> func4 = (x, y) => Math.Cos(10 * (x + y)); // Example 4
        Func<double, double, double> func5 = (x, y) => Math.Exp(-(x * x + y * y)); // Example 5
        Func<double, double, double> func6 = (x, y) => x * y; // Example 6
        Func<double, double, double> func7 = (x, y) => Math.Exp(-(x * x + y * y));
        
        Console.WriteLine("A: 2D Integration Examples using Integrate2D Class\n");

        // --- Proof that our thing works 2D Integration Examples ---
        // Example 1: ∫₀¹ dx ∫₀¹ dy (x * y) = (1/2)*(1/2) = 0.25
        double result1 = Integrate2D.Simpson2D(func1, 0, 1, x => 0, x => 1, 1e-6, 1e-6);
        Console.WriteLine("\n----------\nExample 1: ∫₀¹ dx ∫₀¹ dy (x*y)");
        Console.WriteLine($"Simpson: {result1}, Expected: 0.25, Error: {Math.Abs(result1 - 0.25)}, Function calls: {Integrate2D.simpsonCallCount()}\n");
        Integrate2D.ResetsimpsonCallCount(); // Reset call count for next example


        // Example 2: ∫₀¹ dx ∫₀ˣ dy (1) = area of triangle = 1/2
        double result2 = Integrate2D.Simpson2D(func2, 0, 1, x => 0, x => x, 1e-6, 1e-6 );
        Console.WriteLine("\n----------\nExample 2: ∫₀¹ dx ∫₀ˣ dy (1)");
        Console.WriteLine($"Simpson: {result2}, Expected: 0.5, Error: {Math.Abs(result2 - 0.5)}, Function calls: {Integrate2D.simpsonCallCount()}\n");
        Integrate2D.ResetsimpsonCallCount(); // Reset call count for next example

        Console.WriteLine("COMMENT: Very few function calls (20) due to early convergence with adaptive Simpson!");

        // Example 3: ∫₀^π dx ∫₀¹ dy (sin(x)) = ∫₀^π sin(x) dx = 2 (independent of y)
        double result3 = Integrate2D.Simpson2D(func3, 0, Math.PI, x => 0, x => 1, 1e-6, 1e-6);
        Console.WriteLine("\n----------\nExample 3: ∫₀^π dx ∫₀¹ dy (sin(x))");
        Console.WriteLine($"Simpson: {result3}, Expected: 2, Error: {Math.Abs(result3 - 2)}, Function calls: {Integrate2D.simpsonCallCount()}\n");
        Integrate2D.ResetsimpsonCallCount(); // Reset call count for next example

        Console.WriteLine("COMMENT: Again, low count of function calls and great accuracy!");

        Console.WriteLine("\nB (and C?): More examples, more methods!\n");
        // Oscillatory test: cos(10(x + y)) over [0,1]x[0,1]
        double result4 = Integrate2D.Simpson2D(func4, 0, 1, x => 0, x => 1, 1e-6, 1e-6);
        double expected4 = (-Math.Cos(20) + 2 * Math.Cos(10) - 1.0) / 100.0;
        Console.WriteLine("\n----------\nExample 4: ∫₀¹ dx ∫₀¹ dy cos(10(x+y))");
        Console.WriteLine($"Simpson: {result4}, Expected: {expected4}, Error: {Math.Abs(result4 - expected4)}, Function calls: {Integrate2D.simpsonCallCount()}\n");
        Integrate2D.ResetsimpsonCallCount(); // Reset call count for next example

        double result4mid = Integrate2D.Midpoint(func4, 0, 1, 0, 1, 1e-6, 1e-6);
        Console.WriteLine($"midpoint: {result4mid}, Expected: {expected4}, Error: {Math.Abs(result4mid - expected4)}, Function calls: {Integrate2D.midpointCallCount()}\n");
        Integrate2D.ResetMidpointCallCount(); // Reset call count for next example

        double result4quad = Integrate2D.QuadIntegrate(func4, 0, 1, 0, 1, 1e-6, 1e-6);
        Console.WriteLine($"quad: {result4quad}, Expected: {expected4}, Error: {Math.Abs(result4quad - expected4)}, Function calls: {Integrate2D.quadCallCount()}\n");
        Integrate2D.ResetQuadCallCount(); // Reset call count for next example

        Console.WriteLine("COMMENT: This is an interesting one, as it has a fast oscillating cosine in diagonal direction. \n Simpson manages this, but with high cost.\n Midpoint and Quad both achieve less accuracy but especcially Quad uses much less function calls.\n This highlights how the Simpson implementation struggles with oscillating funcs like this.");

        // Gaussian bump: e^{-(x^2 + y^2)} over [-2,2]x[-2,2]
        double result5 = Integrate2D.Simpson2D(func5,-2, 2, x => -2, x => 2,1e-6, 1e-6);
        double erf2 = SpecialErf(2); // we'll define this below
        double expected5 = Math.PI * erf2 * erf2;
        Console.WriteLine("\n----------\nExample 5: ∫_{-2}^{2} dx ∫_{-2}^{2} dy e^{-(x^2 + y^2)}");
        Console.WriteLine($"Simpson: {result5}, Expected: {expected5}, Error: {Math.Abs(result5 - expected5)}, Function calls: {Integrate2D.simpsonCallCount()}\n");
        Integrate2D.ResetsimpsonCallCount(); // Reset call count for next example

        double result5mid = Integrate2D.Midpoint(func5, -2, 2, -2, 2, 1e-6, 1e-6);
        Console.WriteLine($"Midpoint: {result5mid}, Expected: {expected5}, Error: {Math.Abs(result5mid - expected5)}, Function calls: {Integrate2D.midpointCallCount()}\n");
        Integrate2D.ResetMidpointCallCount(); // Reset call count for next example

        double result5quad = Integrate2D.QuadIntegrate(func5, -2, 2, -2, 2, 1e-6, 1e-6);
        Console.WriteLine($"Quad: {result5quad}, Expected: {expected5}, Error: {Math.Abs(result5quad - expected5)}, Function calls: {Integrate2D.quadCallCount()}\n");
        Integrate2D.ResetQuadCallCount(); // Reset call count for next example

        Console.WriteLine("COMMENT: This is a Gaussian bump, which is a common test case for numerical integration. \n Simpson does well here, but we see that Midpoint and Quad are both more efficient in terms of function calls.");


        // Triangular domain: ∫₀¹ dx ∫₀^{1-x} xy dy
        double result6 = Integrate2D.Simpson2D(func6, 0, 1, x => 0, x => 1 - x, 1e-6, 1e-6);
        double expected6 = 1.0 / 24;
        Console.WriteLine("\n----------\nExample 6: ∫₀¹ dx ∫₀^{1-x} x*y dy");
        Console.WriteLine($"Simpson: {result6}, Expected: {expected6}, Error: {Math.Abs(result6 - expected6)}, Function calls: {Integrate2D.simpsonCallCount()}\n");
        Integrate2D.ResetsimpsonCallCount(); // Reset call count for next example

        double result6mid = Integrate2D.Midpoint(func6, 0, 1, 0, 1, 1e-6, 1e-6);
        Console.WriteLine($"Midpoint: {result6mid}, Expected: {expected6}, Error: {Math.Abs(result6mid - expected6)}, Function calls: {Integrate2D.midpointCallCount()}\n");
        Integrate2D.ResetMidpointCallCount(); // Reset call count for next example

        double result6quad = Integrate2D.QuadIntegrate(func6, 0, 1, x => 0, x => 1 - x, 1e-6, 1e-6);
        Console.WriteLine($"Quad: {result6quad}, Expected: {expected6}, Error: {Math.Abs(result6quad - expected6)}, Function calls: {Integrate2D.quadCallCount()}\n");
        Integrate2D.ResetQuadCallCount(); // Reset call count for next example
        
        Console.WriteLine("COMMENT: This is a triangular domain integral. \n Simpson does well here. Midpoint I had to call on square cause wrapper made it not converge. Midpoint does worse than Simpson.");

        // Integral over unit disk of exp(-(x^2 + y^2))
        double result7 = Integrate2D.Simpson2D(func7, -1, 1, x => -Math.Sqrt(1 - x * x), x =>  Math.Sqrt(1 - x * x), 1e-6, 1e-6);
        double expected7 = Math.PI * (1 - Math.Exp(-1));
        Console.WriteLine("\n----------\nExample 7: Integral over unit disk of exp(-(x^2 + y^2)):");
        Console.WriteLine($"Simpson: {result7}, Expected: {expected7}, Error: {Math.Abs(result7 - expected7)}, Function calls: {Integrate2D.simpsonCallCount()}\n");
        Integrate2D.ResetsimpsonCallCount(); // Reset call count for next example

        double result7mid = Integrate2D.Midpoint(func7,-1, 1, x => -Math.Sqrt(1 - x * x), x =>  Math.Sqrt(1 - x * x), 1e-6, 1e-6);
        Console.WriteLine($"Midpoint: {result7mid}, Expected: {expected7}, Error: {Math.Abs(result7mid - expected7)}, Function calls: {Integrate2D.midpointCallCount()}\n");
        Integrate2D.ResetMidpointCallCount(); // Reset call count for next example

        double result7quad = Integrate2D.QuadIntegrate(func7, -1, 1, x => -Math.Sqrt(1 - x * x), x =>  Math.Sqrt(1 - x * x), 1e-6, 1e-6);
        Console.WriteLine($"Quad: {result7quad}, Expected: {expected7}, Error: {Math.Abs(result7quad - expected7)}, Function calls: {Integrate2D.quadCallCount()}\n");
        Integrate2D.ResetQuadCallCount(); // Reset call count for next example
    
        Console.WriteLine("COMMENT: This is a circular domain integral. \n Simpson again does well here, with acceptabel cost. \n Midpoint has huge cost, does not do well with the masking.\n Quad is not as accurate as simpson but very efficient.");
    
        Console.WriteLine("\nCONCLUSION: Curved domains work well with Simpson. Midpoint struggles with curved things and the masking, either not converging or taking many function calls. \nQuad is efficient but less accurate, especially with oscillatory functions. \n Simpson is the best all-rounder for these examples, but Quad is a good alternative for efficiency.");
    }
    // Approximation to erf(x), accurate enough for our test
    public static double SpecialErf(double x)
    {
        // Abramowitz and Stegun formula
        double t = 1.0 / (1.0 + 0.3275911 * x);
        double tau = t * (0.254829592 +
                        t * (-0.284496736 +
                        t * (1.421413741 +
                        t * (-1.453152027 +
                        t * 1.061405429))));
        return 1.0 - tau * Math.Exp(-x * x);
    }

}
