using System;
using static System.Console;
using System.IO;
using System.Linq;

class Testing{
    static void Main(string[] args){
    // init vars
    string[] labels = null;
    double[][] fitdata = null;

    // IO
	for (int i = 0; i < args.Length; i++) {
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
    var fs = new Func<double,double>[] { z => 1.0, z => z, z => z*z }; 

    (vector par, matrix cov) = Fit.ls(fs, fitdata[0],fitdata[1],fitdata[2]);
    var fittedFunc = Fit.CreateFittedFunction(fs,par);
    Write("# ");
    foreach (string label in labels){
        Write($"\"{label}\" ");
    }
    Write($"\"{"Test"}\" ");
    
    Write("\n");
    for (int i = 0; i<fitdata[0].Length; i++){
        WriteLine($"{fitdata[0][i]} {fitdata[1][i]} {fitdata[2][i]} {fittedFunc(fitdata[0][i])}");
    }
    

}
}