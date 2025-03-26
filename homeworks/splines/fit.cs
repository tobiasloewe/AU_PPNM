using System;

public class Fit{
    public static int binsearch(double[] x, double z)
        {/* locates the interval for z by bisection */ 
        if( z<x[0] || z>x[x.Length-1] ) throw new Exception("binsearch: bad z");
        int i=0, j=x.Length-1;
        while(j-i>1){
            int mid=(i+j)/2;
            if(z>x[mid]) i=mid; else j=mid;
            }
        return i;
	}
    public static (vector ,matrix) ls (Func<double,double>[] fs , vector x, vector y, vector dy){
        int n = x.size, m=fs .Length; 
        var A = new matrix(n,m); 
        var b = new vector(n); 
        for(int i=0;i<n; i++){
            b[ i]=y[i]/dy[i];
            for(int k=0;k<m;k++)A[i,k]=fs [k](x[i])/dy[i]; 
        } 
        vector c = QR.solve(A,b); // solves ||A∗c−b||−>min matrix AI = A.inverse (); // calculates pseudoinverse matrix Σ = AI∗AI.T; return (c, Σ); }
        matrix sigma = QR.inverse(A.T*A);

        return (c, sigma);
    }
    public static Func<double, double> CreateFittedFunction(Func<double, double>[] fs, vector par) {
        return z => {
            double result = 0;
            for (int i = 0; i < fs.Length; i++) {
                result += par[i] * fs[i](z);
            }
            return result;
        };
    }
    public static Func<double,double> linSplines(vector x, vector y){
        return delegate(double z){
            int i=binsearch(x,z);
            double dx=x[i+1]-x[i]; if(!(dx>0)) throw new Exception("uups...");
            double dy=y[i+1]-y[i];
            return y[i]+dy/dx*(z-x[i]);
        };
    }
    public static Func<double, double> quadSplines(double[] x, double[] y, out double[] bcopy, out double[] ccopy) {
        int n = x.Length;
        if (y.Length != n) throw new ArgumentException("x and y must have the same length");

        double[] b = new double[n - 1];
        double[] c = new double[n - 1];
        double[] h = new double[n - 1];
        double[] p = new double[n - 1];

        for (int i = 0; i < n - 1; i++) {
            h[i] = x[i + 1] - x[i];
            p[i] = (y[i + 1] - y[i]) / h[i];
        }

        c[0] = 0;
        for (int i = 0; i < n - 2; i++)
            c[i + 1] = (p[i + 1] - p[i] - c[i] * h[i]) / h[i + 1];

        c[n - 2] /= 2;
        for (int i = n - 3; i >= 0; i--)
            c[i] = (p[i + 1] - p[i] - c[i + 1] * h[i + 1]) / h[i];

        for (int i = 0; i < n - 1; i++)
            b[i] = p[i] - c[i] * h[i];
        
        ccopy = (double[])c.Clone();
        bcopy = (double[])b.Clone(); 

        return z => {
            if (z < x[0] || z > x[n - 1]) throw new ArgumentException("z out of bounds");

            // binary search
            int i = binsearch(x,z);

            double dz = z - x[i];
            return y[i] + dz * (b[i] + dz * c[i]);
        };
    }
    public static Func<double, double> quadSplines(double[] x, double[] y) {
        int n = x.Length;
        if (y.Length != n) throw new ArgumentException("x and y must have the same length");

        double[] b = new double[n - 1];
        double[] c = new double[n - 1];
        double[] h = new double[n - 1];
        double[] p = new double[n - 1];

        for (int i = 0; i < n - 1; i++) {
            h[i] = x[i + 1] - x[i];
            p[i] = (y[i + 1] - y[i]) / h[i];
        }

        c[0] = 0;
        for (int i = 0; i < n - 2; i++)
            c[i + 1] = (p[i + 1] - p[i] - c[i] * h[i]) / h[i + 1];

        c[n - 2] /= 2;
        for (int i = n - 3; i >= 0; i--)
            c[i] = (p[i + 1] - p[i] - c[i + 1] * h[i + 1]) / h[i];

        for (int i = 0; i < n - 1; i++)
            b[i] = p[i] - c[i] * h[i];

        return z => {
            if (z < x[0] || z > x[n - 1]) throw new ArgumentException("z out of bounds");

            // binary search
            int i = 0, j = n - 1;
            while (j - i > 1) {
                int m = (i + j) / 2;
                if (z > x[m]) i = m;
                else j = m;
            }

            double dz = z - x[i];
            return y[i] + dz * (b[i] + dz * c[i]);
        };
    }
    public static Func<double, double> cubicSplines(double[] x, double[] y) {
        int n = x.Length;
        if (y.Length != n) throw new ArgumentException("x and y must have the same length");

        double[] b = new double[n];
        double[] c = new double[n - 1];
        double[] d = new double[n - 1];
        double[] h = new double[n - 1];
        double[] p = new double[n - 1];

        for (int i = 0; i < n - 1; i++) {
            h[i] = x[i + 1] - x[i];
            if (h[i] <= 0) throw new Exception("x must be strictly increasing");
            p[i] = (y[i + 1] - y[i]) / h[i];
        }

        double[] D = new double[n];
        double[] Q = new double[n - 1];
        double[] B = new double[n];

        D[0] = 2;
        Q[0] = 1;
        B[0] = 3 * p[0];

        for (int i = 0; i < n - 2; i++) {
            D[i + 1] = 2 * h[i] / h[i + 1] + 2;
            Q[i + 1] = h[i] / h[i + 1];
            B[i + 1] = 3 * (p[i] + p[i + 1] * h[i] / h[i + 1]);
        }

        D[n - 1] = 2;
        B[n - 1] = 3 * p[n - 2];

        // Forward elimination
        for (int i = 1; i < n; i++) {
            D[i] -= Q[i - 1] / D[i - 1];
            B[i] -= B[i - 1] / D[i - 1];
        }

        b[n - 1] = B[n - 1] / D[n - 1];

        // Back substitution
        for (int i = n - 2; i >= 0; i--)
            b[i] = (B[i] - Q[i] * b[i + 1]) / D[i];

        for (int i = 0; i < n - 1; i++) {
            c[i] = (-2 * b[i] - b[i + 1] + 3 * p[i]) / h[i];
            d[i] = (b[i] + b[i + 1] - 2 * p[i]) / (h[i] * h[i]);
        }

        return z => {
            if (z < x[0] || z > x[n - 1]) throw new ArgumentException("z out of bounds");

            int i = 0, j = n - 1;
            while (j - i > 1) {
                int m = (i + j) / 2;
                if (z > x[m]) i = m;
                else j = m;
            }

            double dz = z - x[i];
            return y[i] + dz * (b[i] + dz * (c[i] + dz * d[i]));
        };
    }
    public static Func<double, double> linSplineInt(double[] x, double[] y)
    {
        int n = x.Length;
        if (y.Length != n) throw new ArgumentException("x and y must be same length");

        double[] h = new double[n - 1];
        double[] a = new double[n - 1];
        double[] b = new double[n - 1];
        double[] cumulative = new double[n];
        cumulative[0] = 0;

        for (int i = 0; i < n - 1; i++)
        {
            h[i] = x[i + 1] - x[i];
            a[i] = y[i];
            b[i] = (y[i + 1] - y[i]) / h[i];
            cumulative[i + 1] = cumulative[i] + a[i] * h[i] + 0.5 * b[i] * h[i] * h[i];
        }

        return z => {
            if (z < x[0]) return 0;
            if (z > x[n - 1]) return cumulative[n - 1];

            int i = 0, j = n - 1;
            while (j - i > 1)
            {
                int m = (i + j) / 2;
                if (z > x[m]) i = m;
                else j = m;
            }

            double dz = z - x[i];
            return cumulative[i] + a[i] * dz + 0.5 * b[i] * dz * dz;
        };
    }
    public static Func<double, double> quadSplineInt(double[] x, double[] y) {
        int n = x.Length;
        if (y.Length != n) throw new ArgumentException("x and y must be the same length");

        double[] h = new double[n - 1];
        double[] p = new double[n - 1];
        double[] b = new double[n - 1];
        double[] c = new double[n - 1];
        double[] integral = new double[n];
        integral[0] = 0;

        for (int i = 0; i < n - 1; i++) {
            h[i] = x[i + 1] - x[i];
            p[i] = (y[i + 1] - y[i]) / h[i];
        }

        c[0] = 0;
        for (int i = 0; i < n - 2; i++)
            c[i + 1] = (p[i + 1] - p[i] - c[i] * h[i]) / h[i + 1];

        c[n - 2] /= 2;
        for (int i = n - 3; i >= 0; i--)
            c[i] = (p[i + 1] - p[i] - c[i + 1] * h[i + 1]) / h[i];

        for (int i = 0; i < n - 1; i++) {
            b[i] = p[i] - c[i] * h[i];
            integral[i + 1] = integral[i]
                + y[i] * h[i]
                + 0.5 * b[i] * h[i] * h[i]
                + (1.0 / 3.0) * c[i] * h[i] * h[i] * h[i];
        }

        return z => {
            if (z < x[0]) return 0;
            if (z > x[n - 1]) return integral[n - 1];

            int i = 0, j = n - 1;
            while (j - i > 1) {
                int m = (i + j) / 2;
                if (z > x[m]) i = m;
                else j = m;
            }

            double dz = z - x[i];
            return integral[i]
                + y[i] * dz
                + 0.5 * b[i] * dz * dz
                + (1.0 / 3.0) * c[i] * dz * dz * dz;
        };
    }

    public static Func<double, double> cubicSplineInt(double[] x, double[] y) {
        int n = x.Length;
        if (y.Length != n) throw new ArgumentException("x and y must have the same length");

        double[] b = new double[n];
        double[] c = new double[n - 1];
        double[] d = new double[n - 1];
        double[] h = new double[n - 1];
        double[] p = new double[n - 1];

        for (int i = 0; i < n - 1; i++) {
            h[i] = x[i + 1] - x[i];
            if (h[i] <= 0) throw new Exception("x must be strictly increasing");
            p[i] = (y[i + 1] - y[i]) / h[i];
        }

        double[] D = new double[n];
        double[] Q = new double[n - 1];
        double[] B = new double[n];

        D[0] = 2;
        Q[0] = 1;
        B[0] = 3 * p[0];

        for (int i = 0; i < n - 2; i++) {
            D[i + 1] = 2 * h[i] / h[i + 1] + 2;
            Q[i + 1] = h[i] / h[i + 1];
            B[i + 1] = 3 * (p[i] + p[i + 1] * h[i] / h[i + 1]);
        }

        D[n - 1] = 2;
        B[n - 1] = 3 * p[n - 2];

        for (int i = 1; i < n; i++) {
            D[i] -= Q[i - 1] / D[i - 1];
            B[i] -= B[i - 1] / D[i - 1];
        }

        b[n - 1] = B[n - 1] / D[n - 1];

        for (int i = n - 2; i >= 0; i--)
            b[i] = (B[i] - Q[i] * b[i + 1]) / D[i];

        for (int i = 0; i < n - 1; i++) {
            c[i] = (-2 * b[i] - b[i + 1] + 3 * p[i]) / h[i];
            d[i] = (b[i] + b[i + 1] - 2 * p[i]) / (h[i] * h[i]);
        }

        double[] cumulative = new double[n];
        cumulative[0] = 0;
        for (int i = 0; i < n - 1; i++) {
            double hi = h[i];
            double bi = b[i], ci = c[i], di = d[i];
            cumulative[i + 1] = cumulative[i]
                + y[i] * hi
                + 0.5 * bi * hi * hi
                + (1.0 / 3.0) * ci * hi * hi * hi
                + 0.25 * di * hi * hi * hi * hi;
        }

        return z => {
            if (z < x[0]) return 0;
            if (z > x[n - 1]) return cumulative[n - 1];

            int i = 0, j = n - 1;
            while (j - i > 1) {
                int m = (i + j) / 2;
                if (z > x[m]) i = m;
                else j = m;
            }

            double dz = z - x[i];
            return cumulative[i]
                + y[i] * dz
                + 0.5 * b[i] * dz * dz
                + (1.0 / 3.0) * c[i] * dz * dz * dz
                + 0.25 * d[i] * dz * dz * dz * dz;
        };
    }


}