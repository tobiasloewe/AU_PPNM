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

        // With E0 do another ODE run to find wavefunc
        Func<double, vector, vector> F_E0 = (r, y) => ShootingH.SchrodingerODE(r, y, E0);
        vector y0 = new vector(rmin - rmin * rmin, 1 - 2 * rmin);
        var (rValues,yValues) = ODE.driver(F_E0, (rmin, rmax), y0, hstart, acc, eps);
        
        // Write the out wavefunc
        using (StreamWriter wfWriter = new StreamWriter("wavefunc.txt"))
        {
            wfWriter.WriteLine("r\tnumeric\texact");
            for (int i = 0; i < rValues.size; i++)
            {
                wfWriter.WriteLine($"{rValues[i]:G5}\t{yValues[i][0]:G5}\t{rValues[i]*Math.Exp(-rValues[i]):G5}");
            }
        }
        Console.WriteLine("Wavefunction written to wavefunc.txt");

        // convergence tests
        using (StreamWriter convwrite = new StreamWriter("convergence.txt"))
        {
            double[] rmax_values = new double[] {1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16};
            foreach (double rm in rmax_values)
            {
                vector Eguess = new vector(-0.5);
                Func<vector, vector> F_E_rm = X => ShootingH.M(X, rmin, rm, acc, eps, hstart);
                vector E_root_rm = Roots.newton(F_E_rm, Eguess, 1e-6);
                convwrite.WriteLine($"{rm} {E_root_rm[0]:G7}");
            }
            
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