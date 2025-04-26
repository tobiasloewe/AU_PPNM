using System;

class MCErrorTests
{
    static void Main(string[] args){
    // init vars
        int minN = 1000;
        int maxN = 10000000;
       
       // IO
        for (int i = 0; i < args.Length; i++) {
            if (args[i] == "-minN" && i + 1 < args.Length) {
        		minN = int.Parse(args[i+1]);
            }
            if (args[i] == "-maxN" && i + 1 < args.Length) {
        		maxN = int.Parse(args[i+1]);
            }
        }
        Func<vector,double> unitCircle = (vector x) => {
            // check that x.size is 2, otherwise throw error
            if (x.size != 2)throw new ArgumentException("Input vector x must have a size of 2.");
            if (Math.Sqrt(Math.Pow(x[0],2) + Math.Pow(x[1],2))<1) return 1;
            else return 0;
        };
        Func<vector, double> gaussian2D = (vector x) => {
            // check that x.size is 2, otherwise throw error
            if (x.size != 2) throw new ArgumentException("Input vector x must have a size of 2.");
            double sigma = 1.0; // Standard deviation
            double normalization = 1.0 / (2 * Math.PI * sigma * sigma);
            return normalization * Math.Exp(-(x[0] * x[0] + x[1] * x[1]) / (2 * sigma * sigma));
        };
        vector circleLower = new vector("-1,-1");
        vector circleUpper = new vector("1,1");
        MonteCarloTest(unitCircle, circleLower, circleUpper, minN, maxN, Math.PI, "unitCircle.txt");
        vector gaussianLower = new vector("-5,-5");
        vector gaussianUpper = new vector("5,5");
        MonteCarloTest(gaussian2D, gaussianLower, gaussianUpper, minN, maxN, 1.0, "gaussian2D.txt");
    }

    public static void MonteCarloTest(
        Func<vector, double> func, vector lowerBound, vector upperBound,
        int minN, int maxN, double idealresult, string filename, double minLogValue = 1e-308)
    {
        using (var writer = new System.IO.StreamWriter(filename))
        {
            writer.WriteLine("N Result EstimatedError ActualError QuasiEstimatedError QuasiActualError");
        for (int N = minN; N <= maxN; N *= 10)
        {
            var (result, estimatedError) = Montecarlo.plain(func, lowerBound, upperBound, N);
            var (resultQ, estimatedErrorQ) = Montecarlo.quasi(func, lowerBound, upperBound, N);
            
            double actualError = Math.Abs(idealresult - result);
            double actualErrorQ = Math.Abs(idealresult - resultQ);

            // Replace zeros with the smallest positive double
            if (estimatedError == 0) estimatedError = minLogValue;
            if (actualError == 0) actualError = minLogValue;
            if (estimatedErrorQ == 0) estimatedErrorQ = minLogValue;
            if (actualErrorQ == 0) actualErrorQ = minLogValue;

            writer.WriteLine($"{N} {result} {estimatedError} {actualError} {estimatedErrorQ} {actualErrorQ}");
        }
        writer.Close();
    }
    Console.WriteLine($"Results from montecarlo plain and quasi test written to {filename}.");
    }
}