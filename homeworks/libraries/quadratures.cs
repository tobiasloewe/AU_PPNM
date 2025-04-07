using System;
public static class Quadratures{   
    public static double integrate(Func<double,double> f, double a, double b,
    double acc = 0.001, double eps = 0.001, double f2 = double.NaN, double f3 = double.NaN) // NaN indicates first call
    {
        double h=b-a;
        if(double.IsNaN(f2)){ f2=f(a+2*h/6); f3=f(a+4*h/6); } // first call, no points to reuse
        double f1=f(a+h/6), f4=f(a+5*h/6);
        double Q = (2*f1+f2+f3+2*f4)/6*(b-a); // higher order rule
        double q = (  f1+f2+f3+  f4)/4*(b-a); // lower order rule
        double err = Math.Abs(Q - q); // use Math.Abs for absolute value
        if (err <= acc + eps * Math.Abs(Q)) return Q;
        else return integrate(f, a, (a + b) / 2, acc / Math.Sqrt(2), eps, f1, f2) +
                    integrate(f, (a + b) / 2, b, acc / Math.Sqrt(2), eps, f3, f4);
    }
}