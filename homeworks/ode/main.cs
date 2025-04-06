using System;
using static System.Console;
class Program {
    static void Main() {
        // First example: Harmonic oscillator (u'' = -u)
        Func<double, vector, vector> harmonicOscillator = (t, u) => {
            double u1 = u[0];
            double u2 = u[1];
            return new vector(u2, -u1); // u1' = u2, u2' = -u1
        };

        // Second example: Damped pendulum
        Func<double, vector, vector> pendulum = (t, y) => {
            double theta = y[0];
            double omega = y[1];
            return new vector(omega, -0.25 * omega - 5.0 * Math.Sin(theta)); // theta' = omega, omega' = -b*omega - c*sin(theta)
        };

        // Third example: Relativistic precession of planetary orbit
        Func<double, vector, vector> planetaryOrbit12 = (phi, y) => {
            double y0 = y[0]; // u(phi)
            double y1 = y[1]; // u'(phi)
            return new vector(y1, 1 - y0); // y0' = y1, y1' = 1 - y0 + epsilon * y0^2
        };
        Func<double, vector, vector> planetaryOrbit3 = (phi, y) => {
            double y0 = y[0]; // u(phi)
            double y1 = y[1]; // u'(phi)
            return new vector(y1, 1 - y0 + 0.015 * y0 * y0); // y0' = y1, y1' = 1 - y0 + epsilon * y0^2
        };

        // Orbits
        
        double tolerance = 0.01; // Tolerance for integration
        // Initial conditions for planetary motion
        vector u0_1 = new vector(1.0, 0.0); // Initial conditions: u(0) = 1, u'(0) = 0
        vector u0_2 = new vector(Math.PI - 0.1, 0.0); // Initial conditions: theta(0) = pi - 0.1, omega(0) = 0
        vector u0_3 = new vector(1.0, 0.0);   // Newtonian circular orbit
        vector u0_4 = new vector(1.0, -0.5); // Newtonian elliptical orbit
        vector u0_5 = new vector(1.0, -0.5); // Relativistic precession

        // Solve for multiple orbits
        var lims1 = (0, 10);
        var limsOrbs = (0, 20 * Math.PI); // Simulate 10 full orbits

        SolveAndWrite(harmonicOscillator, lims1, u0_1, "harmonic.txt", tolerance, 0.01);
        Error.WriteLine("harmonic done");
        SolveAndWrite(pendulum, lims1, u0_2, "pendulum.txt", tolerance, 0.01);
        Error.WriteLine("pendulum done");
        SolveAndWrite(planetaryOrbit12, (0,4*Math.PI), u0_3, "orbit1.txt", tolerance, 0.001, true);
        Error.WriteLine("orbit1 done");
        SolveAndWrite(planetaryOrbit12, limsOrbs, u0_4, "orbit2.txt", tolerance, 0.001, true);
        Error.WriteLine("orbit2 done");
        SolveAndWrite(planetaryOrbit3, limsOrbs, u0_5, "orbit3.txt", tolerance, 0.001, true);
        Error.WriteLine("orbit3 done");
    }

    public static void SolveAndWrite(
        Func<double, vector, vector> F, (double, double) limits, vector u0, string filename,
        double tolerance, double stepSize0, bool orbit = false)
    {

        var (xlist, ylist) = ODE.driver(F, limits, u0, stepSize0, tolerance, tolerance);
        // Write
        using (var outfile = new System.IO.StreamWriter(filename))
        {
            if(orbit){Error.WriteLine("orbit true");};
            for (int i = 0; i < xlist.size; i++)
            {
                if(orbit){
                    outfile.WriteLine($"{(1/ylist[i][0])*Math.Cos(xlist[i])} {(1/ylist[i][0])*Math.Sin(xlist[i])} {xlist[i]} {ylist[i][0]}");
                }
                else {
                    outfile.WriteLine($"{xlist[i]} {ylist[i][0]} {ylist[i][1]}");
                }
            }
        }
    }
}
