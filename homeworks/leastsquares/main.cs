using System;
using static System.Console;
using System.IO;
using System.Linq;
using static System.Math;

class main{
    static void Main(string[] args){
    // init vars
    string[] labels = null;
    double[][] fitdata = null;
    int n = 2; int m = 2;

    // IO
	for (int i = 0; i < args.Length; i++) {
        if (args[i] == "-dims" && i + 1 < args.Length) {
            var values = args[i + 1].Split(',');
            if (values.Length == 2){
                n=int.Parse(values[0]);
				m=int.Parse(values[1]);
            }
        }
        if (args[i] == "-fitdata" && i + 1 < args.Length){
            string filename = args[i+1];
            string[] lines = File.ReadAllLines(filename);
            labels = new string[lines.Length];
            fitdata = new double[lines.Length][];

            for (int j = 0; j < lines.Length; j++) {
                string[] parts = lines[j].Split(':');
                if (parts.Length == 2) {
                    labels[j] = parts[0].Trim();
                    fitdata[j] = parts[1]
                        .Split(',')
                        .Select(s => double.Parse(s.Trim()))
                        .ToArray();
                }
            }
        }
    }
    
    var rnd = new System.Random(1); 
    var A = new matrix(n,m);
    for (int i=0; i<n; i++){
        for (int j=0; j<m; j++){
            A.set(i,j,rnd.NextDouble());
        }
    }
    (matrix Q, matrix R) = QR.decomp(A);
    WriteLine($"Tall {n} x {m} matrix:");
    WriteLine($"Is Q.T*Q = Id? {matrix.id(m).approx(Q.T*Q)}");
    R.print("R should be upper triangular: ","{0,10:g3} ");



    double[] logfitdata = (double[])fitdata[1].Clone();
    double[] logfiterror = (double[])fitdata[2].Clone();

    for (int i = 0; i<logfitdata.Length; i++){
        logfiterror[i] = logfiterror[i]/logfitdata[i];
        logfitdata[i] = Log(logfitdata[i]);
    }
    var fs = new Func<double,double>[] { z => 1.0, z => -z}; 

    (vector par, matrix cov) = Fit.ls(fs, fitdata[0],logfitdata,logfiterror);
    double[] stds = new double[2] {Sqrt(cov[0][0]),Sqrt(cov[1][1])};

    var fittedFunc = new Func<double, double>(z => Math.Exp(par[0]) * Math.Exp(-par[1] * z));
    var fittedFunc1 = new Func<double, double>(z => Math.Exp(par[0]+stds[0]) * Math.Exp(-(par[1]+stds[1]) * z));
    var fittedFunc2 = new Func<double, double>(z => Math.Exp(par[0]-stds[0]) * Math.Exp(-(par[1]-stds[1]) * z));
    var fittedFunc3 = new Func<double, double>(z => Math.Exp(par[0]-stds[0]) * Math.Exp(-(par[1]+stds[1]) * z));
    var fittedFunc4 = new Func<double, double>(z => Math.Exp(par[0]+stds[0]) * Math.Exp(-(par[1]-stds[1]) * z));
    
    using (StreamWriter writer = new StreamWriter("plotdata.txt"))
    {
        for (int i = 0; i < fitdata[0].Length; i++)
        {
            writer.WriteLine($"{fitdata[0][i]} {fitdata[1][i]} {fitdata[2][i]} {fittedFunc(fitdata[0][i])} {fittedFunc1(fitdata[0][i])} {fittedFunc2(fitdata[0][i])} {fittedFunc3(fitdata[0][i])} {fittedFunc4(fitdata[0][i])}");
        }
    }


    WriteLine($"Half life: {Log(2)/par[1]:F4} +- {stds[1]/par[1]/par[1]*Log(2):F4} days");
    WriteLine($"modern value: 3.6313 +- 0.0012 days");
    cov.print("\nCov matrix: ", "{0,10:g3} ");
}
}