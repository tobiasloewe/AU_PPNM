using System;

public static class Quadratures
{
    private static int evaluationCount = 0;

    public static int EvaluationCount => evaluationCount;

    public static void ResetEvaluationCount()
    {
        evaluationCount = 0;
    }

    public static double integrate(Func<double, double> f, double a, double b,
        double acc = 0.001, double eps = 0.001, double f2 = double.NaN, double f3 = double.NaN)
    {
        // Handle infinite limits
        if (double.IsInfinity(a) || double.IsInfinity(b))
        {
            var (Fnew, anew, bnew) = InfiniteTransform(f, a, b);
            return CCintegrate(Fnew, anew, bnew, acc, eps);
        }

        // Standard finite limit integration
        double h = b - a;
        if (double.IsNaN(f2))
        {
            f2 = f(a + 2 * h / 6);
            f3 = f(a + 4 * h / 6);
            evaluationCount += 2; // First call, no points to reuse
        }
        double f1 = f(a + h / 6), f4 = f(a + 5 * h / 6);
        evaluationCount += 2;
        double Q = (2 * f1 + f2 + f3 + 2 * f4) / 6 * (b - a); // Higher order rule
        double q = (f1 + f2 + f3 + f4) / 4 * (b - a);         // Lower order rule
        double err = Math.Abs(Q - q);                        // Absolute error
        if (err <= acc + eps * Math.Abs(Q)) return Q;
        else return integrate(f, a, (a + b) / 2, acc / Math.Sqrt(2), eps, f1, f2) +
                    integrate(f, (a + b) / 2, b, acc / Math.Sqrt(2), eps, f3, f4);
    }

    public static double CCintegrate(Func<double, double> f, double a, double b,
        double acc = 0.001, double eps = 0.001)
    {
        Func<double, double> transformedF = theta =>
        {
            double x = (a + b) / 2 + (b - a) / 2 * Math.Cos(theta);
            return f(x) * Math.Sin(theta) * (b - a) / 2;
        };
        return integrate(transformedF, 0, Math.PI, acc, eps);
    }

    private static (Func<double, double>, double, double) InfiniteTransform(Func<double, double> f, double a, double b)
    {
        if (double.IsNegativeInfinity(a) && double.IsPositiveInfinity(b))
        {
            // Transform ∫_{-∞}^{∞} f(x) dx to ∫_{-1}^{1} f(t / (1 - t^2)) * (1 + t^2) / (1 - t^2)^2 dt
            return (t =>
            {
                double x = t / (1 - t * t);
                double dxdt = (1 + t * t) / Math.Pow(1 - t * t, 2);
                return f(x) * dxdt;
            }, -1, 1);
        }
        else if (double.IsNegativeInfinity(a))
        {
            // Transform ∫_{-∞}^b f(x) dx to ∫_{0}^{1} f(b + (1 - t) / t) / t^2 dt
            return (t =>
            {
                double x = b + (1 - t) / t;
                double dxdt = 1 / (t * t);
                return f(x) * dxdt;
            }, 0, 1);
        }
        else if (double.IsPositiveInfinity(b))
        {
            // Transform ∫_{a}^{∞} f(x) dx to ∫_{0}^{1} f(a + t / (1 - t)) / (1 - t)^2 dt
            return (t =>
            {
                double x = a + t / (1 - t);
                double dxdt = 1 / Math.Pow(1 - t, 2);
                return f(x) * dxdt;
            }, 0, 1);
        }
        else
        {
            throw new ArgumentException("At least one limit must be infinite.");
        }
    }
}