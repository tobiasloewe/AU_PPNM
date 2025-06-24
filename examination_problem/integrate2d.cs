using System;

public static class Integrate2D
{
    // 4 inital calls, 2 per subdivision
    private static int simpsonCallCount_ = 0;
    private static int quadCallCount_ = 0;
    private static int midpointCallCount_ = 0;

    public static int simpsonCallCount() {return simpsonCallCount_;}
    public static int quadCallCount() {return quadCallCount_;}
    public static int midpointCallCount() {return midpointCallCount_;}

    public static void ResetsimpsonCallCount() {simpsonCallCount_ = 0;}
    public static void ResetQuadCallCount()  {quadCallCount_ = 0;}
    public static void ResetMidpointCallCount()  {midpointCallCount_ = 0;}

    public static double Simpson1D(
        Func<double, double> f, double a, double b, double acc, double eps, double f2 = double.NaN, double f3 = double.NaN, int limit = 100)
    {

        double h = b - a;
        double f1 = f(a + h / 6);
        double f4 = f(a + 5 * h / 6);
        simpsonCallCount_++;
        simpsonCallCount_++;

        if (double.IsNaN(f2)) f2 = f(a + 2 * h / 6); simpsonCallCount_++;
        if (double.IsNaN(f3)) f3 = f(a + 4 * h / 6); simpsonCallCount_++;

        double Q = (2 * f1 + f2 + f3 + 2 * f4) * h / 6;
        double q = (f1 + f2 + f3 + f4) * h / 4;
        double err = Math.Abs(Q - q);

        if (err < acc + eps * Math.Abs(Q) || limit <= 0)
            return Q;
        else
        {
            double mid = (a + b) / 2;
            return Simpson1D(f, a, mid, acc / Math.Sqrt(2), eps, f1, f2, limit - 1)
                 + Simpson1D(f, mid, b, acc / Math.Sqrt(2), eps, f3, f4, limit - 1);
        }
    }

    public static double Simpson2D(
        Func<double, double, double> f,
        double a, double b,
        Func<double, double> d,
        Func<double, double> u,
        double acc, double eps)
    {
        Func<double, double> outerIntegrand = x =>
            Simpson1D(
                y => f(x, y),
                d(x), u(x),
                acc / 2, eps / 2
            );

        return Simpson1D(outerIntegrand, a, b, acc / 2, eps / 2);
    }

    public static double QuadIntegrate(
        Func<double, double, double> f,
        double x0, double x1,
        double y0, double y1,
        double acc, double eps,
        int maxDepth = 10)
    {
        return QuadRecursive(f, x0, x1, y0, y1, acc, eps, maxDepth);
    }

    public static double QuadIntegrate(
        Func<double, double, double> f,
        double x0, double x1,
        Func<double, double> d,
        Func<double, double> u,
        double acc, double eps,
        int maxDepth = 10)
    {
        Func<double, double, double> fWrapped = (x, y) =>
        {
            double yLower = d(x);
            double yUpper = u(x);
            if (y < yLower || y > yUpper)
                return 0.0;
            return f(x, y);
        };

        // Find global y-limits for the given x-range
        double yMin = double.PositiveInfinity;
        double yMax = double.NegativeInfinity;
        int samples = 32;
        for (int i = 0; i <= samples; i++)
        {
            double xi = x0 + (x1 - x0) * i / samples;
            double yl = d(xi);
            double yu = u(xi);
            if (yl < yMin) yMin = yl;
            if (yu > yMax) yMax = yu;
        }

        return QuadRecursive(fWrapped, x0, x1, yMin, yMax, acc, eps, maxDepth);
    }

    private static double QuadRecursive(
        Func<double, double, double> f,
        double x0, double x1,
        double y0, double y1,
        double acc, double eps,
        int depth)
    {
        double hx = x1 - x0;
        double hy = y1 - y0;
        double mx = (x0 + x1) / 2;
        double my = (y0 + y1) / 2;

        // Midpoint estimate over full box
        double f_mid = f(mx, my); quadCallCount_++;
        double full = f_mid * hx * hy;

        // Midpoint estimates over 4 quadrants
        double q1 = f((x0 + mx) / 2, (y0 + my) / 2); quadCallCount_++; // bottom-left
        double q2 = f((mx + x1) / 2, (y0 + my) / 2); quadCallCount_++;// bottom-right
        double q3 = f((x0 + mx) / 2, (my + y1) / 2); quadCallCount_++;// top-left
        double q4 = f((mx + x1) / 2, (my + y1) / 2); quadCallCount_++;// top-right
        double sum4 = (q1 + q2 + q3 + q4) * (hx * hy / 4.0);

        double err = Math.Abs(full - sum4);
        if (err < acc + eps * Math.Abs(sum4) || depth == 0)
        {
            return sum4;
        }
        else
        {
            return
                QuadRecursive(f, x0, mx, y0, my, acc / 2, eps, depth - 1) + // Q1
                QuadRecursive(f, mx, x1, y0, my, acc / 2, eps, depth - 1) + // Q2
                QuadRecursive(f, x0, mx, my, y1, acc / 2, eps, depth - 1) + // Q3
                QuadRecursive(f, mx, x1, my, y1, acc / 2, eps, depth - 1);  // Q4
        }
    }

    public static double Midpoint(
        Func<double, double, double> f,
        double ax, double bx,
        double ay, double by,
        double acc, double eps,
        int maxSteps = 12)
    {
        double prev = double.NaN;
        int n = 4;

        for (int step = 0; step < maxSteps; step++)
        {
            double sum = 0.0;
            double dx = (bx - ax) / n;
            double dy = (by - ay) / n;

            for (int i = 0; i < n; i++)
            {
                double x = ax + (i + 0.5) * dx;
                for (int j = 0; j < n; j++)
                {
                    double y = ay + (j + 0.5) * dy;
                    sum += f(x, y); midpointCallCount_++;
                }
            }

            double result = sum * dx * dy;

            if (!double.IsNaN(prev))
            {
                double err = Math.Abs(result - prev);
                if (err < acc + eps * Math.Abs(result))
                    return result;
            }

            prev = result;
            n *= 2;
        }

        throw new Exception("GridMidpoint2D did not converge within maxSteps.");
    }

    // Overload for variable y-limits
    public static double Midpoint(
        Func<double, double, double> f,
        double ax, double bx,
        Func<double, double> d,
        Func<double, double> u,
        double acc, double eps,
        int maxSteps = 12)
    {
        // Find global y-limits for the given x-range
        double yMin = double.PositiveInfinity;
        double yMax = double.NegativeInfinity;
        int samples = 32;
        for (int i = 0; i <= samples; i++)
        {
            double xi = ax + (bx - ax) * i / samples;
            double yl = d(xi);
            double yu = u(xi);
            if (yl < yMin) yMin = yl;
            if (yu > yMax) yMax = yu;
        }

        Func<double, double, double> fWrapped = (x, y) =>
        {
            double yLower = d(x);
            double yUpper = u(x);
            if (y < yLower || y > yUpper)
                return 0.0;
            return f(x, y);
        };

        return Midpoint(fWrapped, ax, bx, yMin, yMax, acc, eps, maxSteps);
    }

}
