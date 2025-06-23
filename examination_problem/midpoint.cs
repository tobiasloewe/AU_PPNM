using System;

public static class GridMidpoint2D
{
    public static double Integrate(
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
                    sum += f(x, y);
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
}
