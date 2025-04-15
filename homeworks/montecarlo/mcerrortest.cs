using System;

class MCErrorTest
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
        Func<vector,double> unitCircle = (vector x) => {
            // check that x.size is 2, otherwise throw error
            if (x.size != 2)throw new ArgumentException("Input vector x must have a size of 2.");
            if (Math.Sqrt(Math.Pow(x[0],2) + Math.Pow(x[1],2))<1) return 1;
            else return 0;
        };
        vector start = new vector("-1,-1");
        vector end = new vector("1,1");
        var (circArea, qCerror) = Montecarlo.plain(unitCircle,start,end,N);
        Console.WriteLine($"{N} {circArea} {qCerror} {Math.Abs(Math.PI-circArea)}");
    }

}