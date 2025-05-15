using static System.Math;
using System;

public static class Min
{
    public static (vector,int) newton(
        Func<vector, double> φ,       // Function to minimize
        vector x,                     // guess for minimum
        double acc = 1e-3,
        int maxsteps = 1000
    )
    {
        int steps = 0;
        while (steps < maxsteps) // iterations
        {
            vector g = gradient(φ, x);
            if (g.norm() < acc) break;

            matrix H = hessian(φ, x);
            vector dx = QR.solve(H,-g); // Solve H * dx = -g

            double λ = 1.0; 
            while (λ >= 1.0 / 1024.0)
            {
                if (φ(x + λ * dx) < φ(x)) break;
                λ /= 2.0;
            }

            x = x + λ * dx;
            steps++;
        }

        return (x,steps);
    }

    // Updated gradient computation
    private static vector gradient(Func<vector, double> φ, vector x)
    {
        double φx = φ(x); // Evaluate φ at the current point
        vector gφ = new vector(x.size); // Initialize gradient vector

        for (int i = 0; i < x.size; i++)
        {
            double dxi = (1 + Abs(x[i])) * Pow(2, -26); // Compute step size
            x[i] += dxi; // Increment x[i] by dxi
            gφ[i] = (φ(x) - φx) / dxi; // Compute partial derivative
            x[i] -= dxi; // Restore x[i] to its original value
        }

        return gφ; // Return the gradient vector
    }

    // Updated Hessian computation
    private static matrix hessian(Func<vector, double> φ, vector x)
    {
        int n = x.size;
        matrix H = new matrix(n, n); // Initialize Hessian matrix
        vector gφx = gradient(φ, x); // Compute gradient at x

        for (int j = 0; j < n; j++)
        {
            double dxj = (1 + Abs(x[j])) * Pow(2, -13); // step size
            x[j] += dxj; // Increment x[j]
            vector dgφ = gradient(φ, x) - gφx; // change in gradient
            for (int i = 0; i < n; i++)
            {
                H[i, j] = dgφ[i] / dxj; // Compute second derivative
            }
            x[j] -= dxj; // Restore x[j]
        }

        return H;
    }
}
