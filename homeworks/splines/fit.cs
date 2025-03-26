using System;

public class Fit{
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


}