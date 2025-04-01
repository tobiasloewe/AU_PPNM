using System;

class Program {
    static void Main() {
        // Define the system of ODEs
        Func<double, vector, vector> odeSystem = (t, u) => {
            double u1 = u[0];
            double u2 = u[1];
            return new vector(u2, -u1); // u1' = u2, u2' = -u1
        };

        // Initial conditions: u(0) = 1, u'(0) = 0
        vector u0 = new vector(1.0, 0.0);

        // Time range: t = [0, 10]
        double t0 = 0;
        double tEnd = 10;

        // Solve the ODE
        var (ts, us) = ODE.driver(odeSystem, (t0, tEnd), u0);

        // Output the results
        for (int i = 0; i < ts.size; i++) {
            Console.WriteLine($"{ts[i]} {us[i][0]} {us[i][1]}");
        }
    }
}