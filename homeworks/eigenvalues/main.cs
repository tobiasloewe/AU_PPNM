using static System.Console;

class main{
static void Main(string[] args){
	// init vars
	int n=2; int verbose = 0;
    double rmax = 5; double dr = 1;
    string mode = "test";
	
    // IO
	for (int i = 0; i < args.Length; i++) {
        if (args[i] == "-dim" && i + 1 < args.Length){
            n=int.Parse(args[i+1]);
        }
		if (args[i] == "-verbose" && i+1 < args.Length){
			verbose = int.Parse(args[i+1]);
        }
        if (args[i] == "-mode" && i+1 < args.Length){
            mode = args[i+1];
        }
        if (args[i] == "-rmax" && i+1 < args.Length){
            rmax = double.Parse(args[i+1]);
        }
        if (args[i] == "-dr" && i+1 < args.Length){
            dr = double.Parse(args[i+1]);
        }
    }
System.Console.Error.WriteLine($"rmax={rmax} dr={dr}");
    if (mode == "test"){
        // gen symmetric  matrix
        var rnd = new System.Random(1); 
        matrix A = new matrix(n, n);    
        for (int i = 0; i < n; i++) {
            for (int j = 0; j <= i; j++) { // Only iterate over lower triangle including diagonal
                double value = rnd.NextDouble(); // Random value between 0 and 1
                A.set(i, j, value);
                if (i != j) A.set(j, i, value); // Copy to upper triangle
            }
        }
        (vector w,matrix V) = Jacobi.cyclic(A);

        matrix D = new matrix(w);
        matrix isThisA = V*D*(V.T);
        WriteLine($"Is V*D*V.T = A? {A.approx(isThisA)}");
    }
    if (mode == "Hswave"){
        WriteLine("This is Hswave");
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
        WriteLine($"Jacobi worked on H? {H.approx(isThisH)}");
        WriteLine($"lowest energy is : {eps[0]}");
    }
}
}//main
