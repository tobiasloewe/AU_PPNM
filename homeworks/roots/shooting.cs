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
            //min
            double[] rmax_values = new double[] {1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16};
            foreach (double rm in rmax_values)
            {
                vector Eguess = new vector(-0.5);
                Func<vector, vector> F_E_rm = X => ShootingH.M(X, rmin, rm, acc, eps, hstart);
                vector E_root_rm = Roots.newton(F_E_rm, Eguess, 1e-6);
                convwrite.WriteLine($"{rm} {E_root_rm[0]:G7}");
            }
            convwrite.WriteLine();
            convwrite.WriteLine();
            double[] rmin_values = new double[] {1,0.8,0.6,0.5,0.45,0.35,0.3,0.2,1e-1,1e-2,1e-3,1e-4,1e-5};
            foreach (double rminval in rmin_values)
            {
                vector Eguess = new vector(-0.5);
                Func<vector, vector> F_E_rmin = X => ShootingH.M(X, rminval, rmax, acc, eps, hstart);
                vector E_root_rmin = Roots.newton(F_E_rmin, Eguess, 1e-6);
                convwrite.WriteLine($"{rminval} {E_root_rmin[0]:G7}");
            }
            Console.Error.WriteLine("Convergence test for rmax and rmin done");
            convwrite.WriteLine();
            convwrite.WriteLine();
            double[] acc_values = new double[] {1e-1,1e-2,1e-3,1e-4,1e-5,1e-6};
            foreach (double accval in acc_values)
            {
                vector Eguess = new vector(-0.5);
                Func<vector, vector> F_E_acc = X => ShootingH.M(X, rmin, rmax, accval, eps, hstart);
                vector E_root_acc = Roots.newton(F_E_acc, Eguess, 1e-6);
                convwrite.WriteLine($"{accval} {E_root_acc[0]:G7}");
            }
            Console.Error.WriteLine("Convergence tests written to convergence.txt");
            convwrite.WriteLine();
            convwrite.WriteLine();  
            double[] eps_values = new double[] { 1e-2, 1e-3, 1e-4, 1e-5, 1e-6, 1e-7 };
            foreach (double eps_val in eps_values)
            {
                vector Eguess = new vector(-0.5);
                Func<vector, vector> F_E_eps = X => ShootingH.M(X, rmin, rmax, acc, eps_val, hstart);
                Console.Error.WriteLine("debug 1" );
                vector E_root_eps = Roots.newton(F_E_eps, Eguess, 1e-2);
                Console.Error.WriteLine("debug 2" );
                convwrite.WriteLine($"{eps_val}, {E_root_eps[0]:G7}");
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