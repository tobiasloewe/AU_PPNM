using System;

class Program
{
    static void Main(string[] args)
    {
        // init vars
    
        int N = 10000;
        // IO
        for (int i = 0; i < args.Length; i++) {
            if (args[i] == "-N" && i + 1 < args.Length) {
        		N = int.Parse(args[i+1]);
            }
        }

        Func<vector, double> integrand = (vector x) => { return x[0] * x[0] + x[1] * x[1]; };

        vector lowerBound = new vector("0.0,0.0");
        vector upperBound = new vector($"1.0,1.0");

        var (result,err) = Montecarlo.stratified(integrand, lowerBound, upperBound, N, 1e-5);

        Console.WriteLine("\nMonte Carlo Integration Result with stratified sampling:");
        Console.WriteLine($"Easy integral error {Math.Abs(result-(2.0/3.0))}, estimated error {err}");
    }
}
