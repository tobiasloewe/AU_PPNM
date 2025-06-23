using System;

public static class Integ2D
{
    public static double Simpson1D(
        Func<double, double> f, double a, double b, double acc, double eps, double f2 = double.NaN, double f3 = double.NaN, int limit = 100)
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
        double xm = 0.5 * (x0 + x1);

        // Compute y-range at key x-points
        double y0min = d(x0), y0max = u(x0);
        double y1min = d(xm), y1max = u(xm);
        double y2min = d(x1), y2max = u(x1);

        double ylo = Math.Min(y0min, Math.Min(y1min, y2min));
        double yhi = Math.Max(y0max, Math.Max(y1max, y2max));

        // Estimate over full tile
        double I_coarse = Simpson2D(f, x0, x1, x => ylo, x => yhi, acc, eps);

        // Estimate over two halves
        double I_fine =
            Simpson2D(f, x0, xm, x => ylo, x => yhi, acc / Math.Sqrt(2), eps) +
            Simpson2D(f, xm, x1, x => ylo, x => yhi, acc / Math.Sqrt(2), eps);

        double err = Math.Abs(I_coarse - I_fine);

        if (err < acc + eps * Math.Abs(I_fine) || depth == 0)
            return I_fine;
        else
        {
            double left = IntegrateTile(f, x0, xm, d, u, acc / Math.Sqrt(2), eps, depth - 1);
            double right = IntegrateTile(f, xm, x1, d, u, acc / Math.Sqrt(2), eps, depth - 1);
            return left + right;
        }
    }


}
