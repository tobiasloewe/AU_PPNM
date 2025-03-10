using System;
using static System.Console;

class Testing{
    static void Main(string[] args){
    // init vars
	int n=2; int m=2;
    // IO
	for (int i = 0; i < args.Length; i++) {
        if (args[i] == "-dim" && i + 1 < args.Length){
            var values = args[i + 1].Split(',');
            if (values.Length == 2){
                n=int.Parse(values[0]);
				m=int.Parse(values[1]);
            }
        }
    }
    var rnd = new System.Random(1); 
    
    var Atall = new matrix(n,m);
    var Awide = new matrix(m,n);
    var Asq = new matrix(n,n);
    
    
    for (int i=0; i<n; i++){
        for (int j=0; j<m; j++){
            Atall.set(i,j,rnd.NextDouble());
        }
    }
    for (int i=0; i<m; i++){
        for (int j=0; j<n; j++){
            Awide.set(i,j,rnd.NextDouble());
        }
    }
    for (int i = 0; i<n; i++){
        for (int j=0; j<n; j++){
            Asq[i][j] = rnd.NextDouble();
        }
    }

}
}