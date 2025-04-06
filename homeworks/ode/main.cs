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
        Console.WriteLine("Harmonic Oscillator");
        vector u0_1 = new vector(1.0, 0.0); // Initial conditions: u(0) = 1, u'(0) = 0
        double t0_1 = 0, tEnd_1 = 10;         // Time range: t = [0, 10]
        var (ts1, us1) = ODE.driver(harmonicOscillator, (t0_1, tEnd_1), u0_1);
        for (int i = 0; i < ts1.size; i++) {
            Console.WriteLine($"{ts1[i]} {us1[i][0]} {us1[i][1]}");
        }

        // Second example: Damped pendulum
        double b = 0.25; // Damping coefficient
        double c = 5.0;  // Gravitational constant
        Func<double, vector, vector> pendulum = (t, y) => {
            double theta = y[0];
            double omega = y[1];
            return new vector(omega, -b * omega - c * Math.Sin(theta)); // theta' = omega, omega' = -b*omega - c*sin(theta)
        };

        Console.WriteLine("\n\nPendulum");
        double t0_2 = 0, tEnd_2 = 10;
        vector u0_2 = new vector(Math.PI - 0.1, 0.0); // Initial conditions: theta(0) = pi - 0.1, omega(0) = 0
        var (ts2, us2) = ODE.driver(pendulum, (t0_2, tEnd_2), u0_2);
        for (int i = 0; i < ts2.size; i++) {
            Console.WriteLine($"{ts2[i]} {us2[i][0]} {us2[i][1]}");
        }

        // Third example: Relativistic precession of planetary orbit
        Func<double, vector, vector> planetaryOrbit12 = (phi, y) => {
            double y0 = y[0]; // u(phi)
            double y1 = y[1]; // u'(phi)
            return new vector(y1, 1 - y0); // y0' = y1, y1' = 1 - y0 + epsilon * y0^2
        };
        Func<double, vector, vector> planetaryOrbit3 = (phi, y) => {
            double y0 = y[0]; // u(phi)
            double y1 = y[1]; // u'(phi)
            double epsilon = 0.015; // Relativistic correction
            return new vector(y1, 1 - y0 + epsilon * y0 * y0); // y0' = y1, y1' = 1 - y0 + epsilon * y0^2
        };

        Console.WriteLine("\n\nPlanetary Orbit 1");
        // Case 1: Newtonian circular motion (epsilon = 0, u(0) = 1, u'(0) = 0)
        vector u0_3 = new vector(1.0, 0.0);
        var (phi1, orbit1) = ODE.driver(planetaryOrbit12, (0, 20 * Math.PI), u0_3);
        for (int i = 0; i < phi1.size; i++) {
            Console.WriteLine($"{phi1[i]} {orbit1[i][0]} {orbit1[i][1]}");
        }
        
        Console.WriteLine("\n\nPlanetary Orbit 2");
        // Case 2: Newtonian elliptical motion (epsilon = 0, u(0) = 1, u'(0) ≈ -0.5)
        vector u0_4 = new vector(1.0, -0.5);
        var (phi2, orbit2) = ODE.driver(planetaryOrbit12, (0, 20 * Math.PI), u0_4);
        for (int i = 0; i < phi2.size; i++) {
            Console.WriteLine($"{phi2[i]} {orbit2[i][0]} {orbit2[i][1]}");
        }

        Console.WriteLine("\n\nPlanetary Orbit 3");
        vector u0_5 = new vector(1.0, -0.5);
        var (phi3, orbit3) = ODE.driver(planetaryOrbit3, (0, 20 * Math.PI), u0_5);
        //Console.WriteLine("\nPlanetary Orbit (Relativistic Precession):");
        for (int i = 0; i < phi3.size; i++) {
            Console.WriteLine($"{phi3[i]} {orbit3[i][0]} {orbit3[i][1]}");
        }
        
    }
}