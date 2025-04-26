using System;
using static System.Math;

public class Montecarlo
{
    public static (double, double) plain(Func<vector, double> f, vector a, vector b, int N)
    {
        int dim = a.size;
        double V = 1;
        for (int i = 0; i < dim; i++) V *= b[i] - a[i];
        double sum = 0, sum2 = 0;
        var x = new vector(dim);
        var rnd = new Random();
        for (int i = 0; i < N; i++)
        {
            for (int k = 0; k < dim; k++) x[k] = a[k] + rnd.NextDouble() * (b[k] - a[k]);
            double fx = f(x);
            sum += fx;
            sum2 += fx * fx;
        }
        double mean = sum / N, sigma = Sqrt(sum2 / N - mean * mean);
        var result = (mean * V, sigma * V / Sqrt(N));
        return result;
    }

    // Updated function: Quasi-random Monte Carlo integrator
    public static (double, double) quasi(Func<vector, double> f, vector a, vector b, int N)
    {
        int dim = a.size;
        double V = 1;
        for (int i = 0; i < dim; i++) V *= b[i] - a[i];

        double sum1 = 0, sum2 = 0;
        var x1 = new vector(dim);
        var x2 = new vector(dim);

        for (int i = 0; i < N; i++)
        {
            // First Halton sequence
            for (int k = 0; k < dim; k++) x1[k] = a[k] + Halton(i, k + 2) * (b[k] - a[k]);
            double fx1 = f(x1);
            sum1 += fx1;

            // Second Halton sequence (shifted by 1 to ensure independence)
            for (int k = 0; k < dim; k++) x2[k] = a[k] + Halton(i + 1, k + 2) * (b[k] - a[k]);
            double fx2 = f(x2);
            sum2 += fx2;
        }

        double mean1 = sum1 / N;
        double mean2 = sum2 / N;

        double result = mean1 * V;
        double estimatedError = Abs(mean1 - mean2) * V;

        return (result, estimatedError);
    }

    // Helper function: Halton sequence generator
    private static double Halton(int index, int baseNum)
    {
        double result = 0;
        double f = 1.0 / baseNum;
        int i = index;
        while (i > 0)
        {
            result += f * (i % baseNum);
            i /= baseNum;
            f /= baseNum;
        }
        return result;
    }
}