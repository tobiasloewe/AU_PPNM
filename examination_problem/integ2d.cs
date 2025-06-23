using System;

public static class Integ2D
{
    public static double Simpson1D(
        Func<double, double> f, double a, double b, double acc, double eps, double f2 = double.NaN, double f3 = double.NaN, int limit = 1000)
    {
        double h = b - a;
        double f1 = f(a + h / 6);
        double f4 = f(a + 5 * h / 6);

        if (double.IsNaN(f2)) f2 = f(a + 2 * h / 6);
        if (double.IsNaN(f3)) f3 = f(a + 4 * h / 6);

        double Q = (2 * f1 + f2 + f3 + 2 * f4) * h / 6;
        double q = (f1 + f2 + f3 + f4) * h / 4;
        double err = Math.Abs(Q - q);

        if (err < acc + eps * Math.Abs(Q) || limit <= 0)
            return Q;
        else
        {
            double mid = (a + b) / 2;
            return Integrate1D(f, a, mid, acc / Math.Sqrt(2), eps, f1, f2, limit - 1)
                 + Integrate1D(f, mid, b, acc / Math.Sqrt(2), eps, f3, f4, limit - 1);
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
            Integrate1D(
                y => f(x, y),
                d(x), u(x),
                acc / 2, eps / 2
            );

        return Integrate1D(outerIntegrand, a, b, acc / 2, eps / 2);
    }

    public static double IntegrateGrid(
        Func<double, double, double> f,
        double a, double b,
        Func<double, double> d,
        Func<double, double> u,
        double acc, double eps,
        int maxDepth = 10)
    {
        return IntegrateTile(f, a, b, d, u, acc, eps, maxDepth);
    }

    private static double IntegrateTile(
        Func<double, double, double> f,
        double x0, double x1,
        Func<double, double> d,
        Func<double, double> u,
        double acc, double eps,
        int depth)
    {
        // Midpoints in x
        double xm = 0.5 * (x0 + x1);

        // Compute y-range at three x values
        double y0min = d(x0), y0max = u(x0);
        double y1min = d(xm), y1max = u(xm);
        double y2min = d(x1), y2max = u(x1);

        

        double I_full = 0.0;
        int n_subdiv = 2;

        // Split x range into two subintervals and apply Simpson in each half
        for (int i = 0; i < n_subdiv; i++)
        {
            double xa = (i == 0) ? x0 : xm;
            double xb = (i == 0) ? xm : x1;

            double ya = d(xa);
            double yb = u(xa);
            double yc = d(xb);
            double yd = u(xb);

            double ylo = Math.Min(ya, yc);
            double yhi = Math.Max(yb, yd);

            double I_part = approx(xa, xb, ylo, yhi);
            I_full += I_part;
        }

        // Coarse approximation
        double I_coarse = approx(x0, x1,
            Math.Min(y0min, Math.Min(y1min, y2min)),
            Math.Max(y0max, Math.Max(y1max, y2max))
        );

        double err = Math.Abs(I_coarse - I_full);

        if (err < acc + eps * Math.Abs(I_full) || depth == 0)
            return I_full;
        else
        {
            // Subdivide horizontally
            double left = IntegrateTile(f, x0, xm, d, u, acc / Math.Sqrt(2), eps, depth - 1);
            double right = IntegrateTile(f, xm, x1, d, u, acc / Math.Sqrt(2), eps, depth - 1);
            return left + right;
        }
    }

    // Integrate each vertical strip using 1D Simpson's rule (3 y slices)
    public static double approx(double xA, double xB, double yA, double yB)
        {
            double xmA = 0.5 * (xA + xB);
            double ymA = 0.5 * (yA + yB);
            double f00 = f(xA, yA);
            double f01 = f(xA, yB);
            double f10 = f(xB, yA);
            double f11 = f(xB, yB);
            double f0m = f(xA, ymA);
            double f1m = f(xB, ymA);
            double fm0 = f(xmA, yA);
            double fm1 = f(xmA, yB);
            double fmm = f(xmA, ymA);
            double hx = xB - xA;
            double hy = yB - yA;

            return (hx * hy / 36.0) * (
                f00 + f01 + f10 + f11 +
                4 * (f0m + f1m + fm0 + fm1) +
                16 * fmm
            );
        }

}
