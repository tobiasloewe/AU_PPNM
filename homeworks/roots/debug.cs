using System;
using System.IO;

class Program
{
    static void Main()
    {
       // Params
        double rmin = 1e-3;
        double rmax = 8;
        double acc = 1e-6;
        double eps = 1e-6;
        double hstart = 0.001;  

        
        // use newton to find E0
        vector Einit = new vector(-0.5);
        Func<vector, vector> F_E = X => ShootingH.M(X, rmin, rmax, acc, eps, hstart);
        vector Eroot = Roots.newton(F_E, Einit, 1e-6);
        double E0 = Eroot[0];

        // Write the computed energy to out
        Console.WriteLine("Computed E0 = " + E0);
                
        double[] eps_values = new double[] { 1e-2, 1e-3, 1e-4, 1e-5, 1e-6, 1e-7 };
        foreach (double eps_val in eps_values)
        {
            vector Eguess = new vector(-0.4);
            Func<vector, vector> F_E_eps = X => ShootingH.M(X, rmin, rmax, acc, eps_val, hstart);
            Console.Error.WriteLine("f before call");
            Console.WriteLine($"{F_E_eps(new vector(-0.5))}");
            Console.Error.WriteLine("after f call");
            Console.WriteLine($"rmin: {rmin}, rmax: {rmax}, acc: {acc}, eps_val: {eps_val}, hstart: {hstart}");
            Eguess.print("Eguess: ");
            vector E_root_eps = Roots.newton(F_E_eps, Eguess, 1e-2);
            Console.WriteLine("debug 2" );
            Console.WriteLine($"{eps_val}, {E_root_eps[0]:G7}");
        }
        
    }

    
}
public static class ShootingH
{
    // f''(r) = -2*(E + 1/r)*f(r)
    public static vector SchrodingerODE(double r, vector y, double E)
    {
        vector dy = new vector(2);
        dy[0] = y[1];
        dy[1] = -2 * (E + 1 / r) * y[0];
        return dy;
    }

    public static vector M(vector Evec, double rmin, double rmax, double acc, double eps, double hstart)
    {
        double E = Evec[0];
        vector y0 = new vector(rmin - rmin * rmin, 1 - 2 * rmin );
        Func<double, vector, vector> F = (r, y) => SchrodingerODE(r, y, E);
        var (xlist,ylist) = ODE.driver(F, (rmin, rmax), y0, hstart, acc, eps);
        vector yEnd = ylist[ylist.size - 1];
        return new vector(yEnd[0]);
    }

}