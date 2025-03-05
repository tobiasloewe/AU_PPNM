using static System.Console;
using static System.Math;

class hatom{
static void Main(string[] args){
	// init vars
	int verbose = 0;
    double rmax = 5; double dr = 1;	
    int returnfunc = -1;
    // IO
	for (int i = 0; i < args.Length; i++) {
		if (args[i] == "-verbose" && i+1 < args.Length){
			verbose = int.Parse(args[i+1]);
        }
        if (args[i] == "-rmax" && i+1 < args.Length){
            rmax = double.Parse(args[i+1]);
        }
        if (args[i] == "-dr" && i+1 < args.Length){
            dr = double.Parse(args[i+1]);
        }
        if (args[i] == "-returnfunc" && i+1 < args.Length){
            returnfunc = int.Parse(args[i+1]);
        }
    }
    System.Console.Error.WriteLine($"rmax={rmax} dr={dr}");

    int npoints = (int)(rmax/dr)-1;
    vector r = new vector(npoints);
    for(int i=0;i<npoints;i++)r[i]=dr*(i+1);

    matrix H = new matrix(npoints,npoints);
    for(int i=0;i<npoints-1;i++){
        H[i,i]  =-2*(-0.5/dr/dr);
        H[i,i+1]= 1*(-0.5/dr/dr);
        H[i+1,i]= 1*(-0.5/dr/dr);
    }
    H[npoints-1,npoints-1]=-2*(-0.5/dr/dr);
    for(int i=0;i<npoints;i++)H[i,i]+=-1/r[i];

    (vector eps, matrix V) = Jacobi.cyclic(H);
    vector psi_ground = new vector(V.size1);

    if (returnfunc >= 0){
        for (int i = 0; i < V.size1; i++) {
        WriteLine($"{i*dr} {Abs(V[i, returnfunc])} {Abs(V[i, returnfunc])*Abs(V[i, returnfunc])}");
    }
    }
    else {
        WriteLine($"{rmax} {dr} {eps[0]}");
    }
}
}//main
