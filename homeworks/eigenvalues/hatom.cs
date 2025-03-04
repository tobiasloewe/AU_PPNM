using static System.Console;

class hatom{
static void Main(string[] args){
	// init vars
	int n=2; int verbose = 0;
    double rmax = 5; double dr = 1;	
    // IO
	for (int i = 0; i < args.Length; i++) {
        if (args[i] == "-dim" && i + 1 < args.Length){
            n=int.Parse(args[i+1]);
        }
		if (args[i] == "-verbose" && i+1 < args.Length){
			verbose = int.Parse(args[i+1]);
        }
        if (args[i] == "-rmax" && i+1 < args.Length){
            rmax = double.Parse(args[i+1]);
        }
        if (args[i] == "-dr" && i+1 < args.Length){
            dr = double.Parse(args[i+1]);
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
    matrix D = new matrix(eps);
    matrix isThisH = (V*D*V.T);
    //WriteLine($"{H.approx(isThisH, 1e-5, 1e-5)}");
    WriteLine($"{eps[0]}");
}
}//main
