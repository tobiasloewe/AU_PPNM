using static System.Console;
using System.IO;


class main{
static void Main(string[] args){
    double[] x = null;
    double[] y = null;
    int res = 5;
    for (int i = 0; i < args.Length; i++) {
        if (args[i] == "-data" && i + 1 < args.Length){
            string filename = args[i+1];
            string[] lines = File.ReadAllLines(filename);
            x = new double[lines.Length];
            y = new double[lines.Length];

            for (int j = 0; j < lines.Length; j++)
            {
                string[] parts = lines[j].Split(' ');
                x[j] = double.Parse(parts[0]);
                y[j] = double.Parse(parts[1]);
            }
        
        }
        if (args[i] == "-splineres" && i+1<args.Length){
            res = int.Parse(args[i+1]);
        }
    }
    double [] bout, cout;
    var lspline = Fit.linSplines(x,y);
    var qspline = Fit.quadSplines(x,y, out bout, out cout);
    var cspline = Fit.cubicSplines(x,y);
    for (int i=0; i<bout.Length; i++){
        Error.WriteLine($"{bout[i]} {cout[i]}");
    }
    
    double[] splinex = new double[x.Length*res];
    double splinexStep = (x[x.Length-1]-x[0])/splinex.Length;
    for (int i = 0; i<splinex.Length; i++){
        splinex[i] = x[0] + i * splinexStep;
    }
    
    for (int i = 0; i < splinex.Length; i++){
        WriteLine($"{splinex[i]} {lspline(splinex[i])} {qspline(splinex[i])} {cspline(splinex[i])}");
    }
}    
}