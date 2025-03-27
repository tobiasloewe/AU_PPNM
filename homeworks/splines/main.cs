using static System.Console;
using System.IO;
using System;

class main{
static void Main(string[] args){
    double[] x = null;
    double[] y = null;
    int res = 5;
    for (int i = 0; i < args.Length; i++) {
        if (args[i] == "-data" && i + 1 < args.Length){
            string filename = args[i+1];
            string[] lines = File.ReadAllLines(filename);
            x = new double[lines.Length];
            y = new double[lines.Length];

            for (int j = 0; j < lines.Length; j++)
            {
                string[] parts = lines[j].Split(' ');
                x[j] = double.Parse(parts[0]);
                y[j] = double.Parse(parts[1]);
            }
        
        }
        if (args[i] == "-splineres" && i+1<args.Length){
            res = int.Parse(args[i+1]);
        }
    }
    double [] bout, cout;
    var lspline = Fit.linSplines(x,y);
    var qspline = Fit.quadSplines(x,y, out bout, out cout);
    var cspline = Fit.cubicSplines(x,y);
    var linint = Fit.linSplineInt(x,y);
    var quadint = Fit.quadSplineInt(x,y);
    var cubicint = Fit.cubicSplineInt(x,y);

    // manually calculate the coefficients for the quadratic spline
    int n = x.Length;
    if (y.Length != n) throw new ArgumentException("x and y must be same length");

    double[] b = new double[n - 1];
    double[] c = new double[n - 1];
    double[] h = new double[n - 1];
    double[] p = new double[n - 1];

    for (int i = 0; i < n - 1; i++) {
        h[i] = x[i + 1] - x[i];
        p[i] = (y[i + 1] - y[i]) / h[i];
    }

    c[0] = 0; // boundary condition
    for (int i = 0; i < n - 2; i++)
        c[i + 1] = (p[i + 1] - p[i] - c[i] * h[i]) / h[i + 1];

    c[n - 2] /= 2;
    for (int i = n - 3; i >= 0; i--)
        c[i] = (p[i + 1] - p[i] - c[i + 1] * h[i + 1]) / h[i];

    for (int i = 0; i < n - 1; i++)
        b[i] = p[i] - c[i] * h[i];

    bool pass = true;

    for (int i=0; i<bout.Length; i++){
        // compare b,c with bout,cout up to afew decimals
        if (System.Math.Abs(b[i]-bout[i])>1e-10 || System.Math.Abs(c[i]-cout[i])>1e-10){
            pass = false;
        }
    }
    Error.WriteLine($"Test of quadratic spline coefficients: {pass}");

    double[] splinex = new double[x.Length*res];
    double splinexStep = (x[x.Length-1]-x[0])/splinex.Length;
    for (int i = 0; i<splinex.Length; i++){
        splinex[i] = x[0] + i * splinexStep;
    }
    
    for (int i = 0; i < splinex.Length; i++){
        WriteLine($"{splinex[i]} {lspline(splinex[i])} {qspline(splinex[i])} {cspline(splinex[i])} {linint(splinex[i])} {quadint(splinex[i])} {cubicint(splinex[i])}");
    }
}    
}