using System;

class MCErrorTests
{
    static void Main(string[] args){
    // init vars
        int N = 1000;
       
       // IO
        for (int i = 0; i < args.Length; i++) {
            if (args[i] == "-N" && i + 1 < args.Length) {
        		N = int.Parse(args[i+1]);
            }
        }
    
        Func<vector, double> integrand = (vector x) => {
            if (x.size != 3) throw new ArgumentException("Input vector x must have a size of 3.");
            double cosProduct = Math.Cos(x[0]) * Math.Cos(x[1]) * Math.Cos(x[2]);
            return 1.0 / (1.0 - cosProduct) *(1/Math.Pow(Math.PI, 3));
        };
        vector lowerBound = new vector("0,0,0");
        vector upperBound = new vector($"{Math.PI},{Math.PI},{Math.PI}");
        double idealresult = Math.Pow(sfuns.fgamma(0.25), 4) / (4 * Math.Pow(Math.PI, 3));

        var (result, error) = Montecarlo.plain(integrand, lowerBound, upperBound, N);
        var (result2, error2) =  Montecarlo.stratified(integrand, lowerBound, upperBound, N, 1e-6);
        Console.WriteLine("\nMonte Carlo Integration Result for hard integral:");
        Console.WriteLine($"N: {N}");
        Console.WriteLine($"Plain, Stratified results:");
        Console.WriteLine($"Result: {result} {result2}");
        Console.WriteLine($"Estimated Error: {error} {error2}");
        Console.WriteLine($"Actual Error: {Math.Abs(idealresult - result)} {Math.Abs(idealresult - result2)}");
}
}