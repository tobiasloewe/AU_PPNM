using System;

class Program
{
    static void Main()
    {
        vector x0  = new vector(-1.2, 1.0);
        vector root = Roots.newton(RosenbrockGradient, x0, 1e-4);
        Console.WriteLine("Rosenbrock function:");
        root.print("Root found: ");
        RosenbrockGradient(root).print("Gradient at root: ");

        vector x1 = new vector(2.5, 1.5);
        vector x2 = new vector(-2.5, 2.5);
        vector x3 = new vector(-2.5, -2.5);
        vector x4 = new vector(3.0, -1.5);
        vector root1 = Roots.newton(HimmelblauGradient, x1, 1e-4);
        vector root2 = Roots.newton(HimmelblauGradient, x2, 1e-4);
        vector root3 = Roots.newton(HimmelblauGradient, x3, 1e-4);
        vector root4 = Roots.newton(HimmelblauGradient, x4, 1e-4);
        
        Console.WriteLine("Himmelblau function:");
        root1.print("Root 1 found at: ");
        HimmelblauGradient(root1).print("Grad value: ");
        root2.print("Root 2 found at: ");
        HimmelblauGradient(root2).print("Grad value: ");
        root3.print("Root 3 found at: ");
        HimmelblauGradient(root3).print("Grad value: ");
        root4.print("Root 4 found at: ");
        HimmelblauGradient(root4).print("Grad value: ");
    }
    public static vector RosenbrockGradient(vector x)
    {
        double dx = -2 * (1 - x[0]) - 400 * x[0] * (x[1] - x[0] * x[0]);
        double dy = 200 * (x[1] - x[0] * x[0]);
        vector result = new vector(dx, dy);
        return result;
    }
    public static vector HimmelblauGradient(vector x)
    {
        double f1 = x[0] * x[0] + x[1] - 11;
        double f2 = x[0] + x[1] * x[1] - 7;

        double dx = 4 * x[0] * f1 + 2 * f2;
        double dy = 2 * f1 + 4 * x[1] * f2;
        vector result = new vector(dx, dy);
        return result;
    }
}