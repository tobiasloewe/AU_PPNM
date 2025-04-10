using System;

class main
{
    static void Main()
    {
        Func<vector,double> unitCircle = (vector x) =>{
            // check that x.size is 2, otherwise throw error
            if (x.size != 2)throw new ArgumentException("Input vector x must have a size of 2.");
            if (Math.Sqrt(Math.Pow(x[0],2) + Math.Pow(x[1],2))<1) return 1;
            else return 0;
        };
        vector start = new vector("0,0");
        vector end = new vector("1,1");
        int N = 100000;
        var (quarterCircle, qCerror) = Montecarlo.plain(unitCircle,start,end,N);
        Console.WriteLine($"{quarterCircle} +- {qCerror}");
        Console.WriteLine($"Pi/4 is {Math.PI/4}");
    }

}