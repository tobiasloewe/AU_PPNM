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

}