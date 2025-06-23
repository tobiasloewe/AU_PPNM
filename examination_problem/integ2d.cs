using System;

public static class Integ2D
{
    public static double Integrate1D(
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
            return Integrate1D(f, a, mid, acc / Math.Sqrt(2), eps, f1, f2, limit - 1)
                 + Integrate1D(f, mid, b, acc / Math.Sqrt(2), eps, f3, f4, limit - 1);
        }
    }

    public static double Integrate2D(
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
}
