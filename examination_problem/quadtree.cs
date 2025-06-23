using System;

public static class QuadtreeMidpoint2D
{
    public static double QuadIntegrate(
        Func<double, double, double> f,
        double x0, double x1,
        double y0, double y1,
        double acc, double eps,
        int maxDepth = 10)
    {
        return IntegrateRecursive(f, x0, x1, y0, y1, acc, eps, maxDepth);
    }

    private static double IntegrateRecursive(
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
        double f_mid = f(mx, my);
        double full = f_mid * hx * hy;

        // Midpoint estimates over 4 quadrants
        double q1 = f((x0 + mx) / 2, (y0 + my) / 2); // bottom-left
        double q2 = f((mx + x1) / 2, (y0 + my) / 2); // bottom-right
        double q3 = f((x0 + mx) / 2, (my + y1) / 2); // top-left
        double q4 = f((mx + x1) / 2, (my + y1) / 2); // top-right
        double sum4 = (q1 + q2 + q3 + q4) * (hx * hy / 4.0);

        double err = Math.Abs(full - sum4);
        if (err < acc + eps * Math.Abs(sum4) || depth == 0)
        {
            return sum4;
        }
        else
        {
            return
                IntegrateRecursive(f, x0, mx, y0, my, acc / 2, eps, depth - 1) + // Q1
                IntegrateRecursive(f, mx, x1, y0, my, acc / 2, eps, depth - 1) + // Q2
                IntegrateRecursive(f, x0, mx, my, y1, acc / 2, eps, depth - 1) + // Q3
                IntegrateRecursive(f, mx, x1, my, y1, acc / 2, eps, depth - 1);  // Q4
        }
    }
}
