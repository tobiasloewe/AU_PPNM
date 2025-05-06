using System;
using static System.Math;

public class Montecarlo
{
    // Plain Monte Carlo integrator
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

    // Quasi-random Monte Carlo integrator
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

            // Second Halton
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

    // Halton sequence generator
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
    public static (double, double) stratified(Func<vector, double> f, vector a, vector b, int N, double tolerance = 1e-3, int minPoints = 2)
    {
        int dim = a.size;
        double V = 1;
        for (int i = 0; i < dim; i++) V *= b[i] - a[i];

        // Base case: If N is too small, return a plain Monte Carlo estimate
        if (N < minPoints)
        {
                        return plain(f, a, b, N);
        }

        // Sample N random points
        var rndPoints = new genlist<vector>();
        var values = new genlist<double>();
        var rnd = new Random();
        for (int i = 0; i < N; i++)
        {
            var x = new vector(dim);
            for (int k = 0; k < dim; k++)
            {
                x[k] = a[k] + rnd.NextDouble() * (b[k] - a[k]);
            }
            rndPoints.add(x);
            values.add(f(x));
        }

        // Estimate the average and the error
        double sum = 0, sum2 = 0;
        for (int i = 0; i < N; i++)
        {
            sum += values[i];
            sum2 += values[i] * values[i];
        }
        double mean = sum / N;
        double variance = sum2 / N - mean * mean;
        double error = Sqrt(Max(variance, 0) / N) * V; // Ensure variance is non-negative

        // If the error is acceptable, return the result
        if (error < tolerance)
        {
            return (mean * V, error);
        }

        // Find the dimension with the largest sub-variance
        int splitDim = 0;
        double maxVariance = 0;
        var mid = new vector(dim);
        for (int i = 0; i < dim; i++) mid[i] = (a[i] + b[i]) / 2;

        for (int i = 0; i < dim; i++)
        {
            double leftSum = 0, leftSum2 = 0, leftCount = 0;
            double rightSum = 0, rightSum2 = 0, rightCount = 0;

            for (int j = 0; j < N; j++)
            {
                if (rndPoints[j][i] < mid[i])
                {
                    leftSum += values[j];
                    leftSum2 += values[j] * values[j];
                    leftCount++;
                }
                else
                {
                    rightSum += values[j];
                    rightSum2 += values[j] * values[j];
                    rightCount++;
                }
            }

            if (leftCount > 0 && rightCount > 0)
            {
                double leftVariance = leftSum2 / leftCount - (leftSum / leftCount) * (leftSum / leftCount);
                double rightVariance = rightSum2 / rightCount - (rightSum / rightCount) * (rightSum / rightCount);
                double totalVariance = leftVariance * leftCount + rightVariance * rightCount;

                if (totalVariance > maxVariance)
                {
                    maxVariance = totalVariance;
                    splitDim = i;
                }
            }
        }

        // Subdivide the volume along the dimension with the largest variance
        var aLeft = a.copy();
        var bLeft = b.copy();
        bLeft[splitDim] = mid[splitDim];

        var aRight = a.copy();
        var bRight = b.copy();
        aRight[splitDim] = mid[splitDim];

        // Allocate points to sub-volumes
        var leftPoints = new genlist<vector>();
        var rightPoints = new genlist<vector>();
        var leftValues = new genlist<double>();
        var rightValues = new genlist<double>();

        for (int i = 0; i < N; i++)
        {
            if (rndPoints[i][splitDim] < mid[splitDim])
            {
                leftPoints.add(rndPoints[i]);
                leftValues.add(values[i]);
            }
            else
            {
                rightPoints.add(rndPoints[i]);
                rightValues.add(values[i]);
            }
        }

        // Dispatch recursive calls to each of the sub-volumes
        var (meanLeft, errorLeft) = stratified(f, aLeft, bLeft, leftPoints.size, tolerance, minPoints);
        var (meanRight, errorRight) = stratified(f, aRight, bRight, rightPoints.size, tolerance, minPoints);

        // Estimate the grand average and grand error
        double leftVolume = 1, rightVolume = 1;
        for (int i = 0; i < dim; i++)
        {
            leftVolume *= bLeft[i] - aLeft[i];
            rightVolume *= bRight[i] - aRight[i];
        }

        double combinedMean = (meanLeft * leftVolume + meanRight * rightVolume) / V;
        double combinedError = Sqrt((errorLeft * leftVolume) * (errorLeft * leftVolume) +
                                    (errorRight * rightVolume) * (errorRight * rightVolume)) / V;

        return (combinedMean, combinedError);
    }
}