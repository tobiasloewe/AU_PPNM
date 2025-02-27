using static System.Console;

class main{
static void Main(string[] args){
	int n=2;
	int m=2;
	int verbose = 0;
	for (int i = 0; i < args.Length; i++) {
        if (args[i] == "-dims" && i + 1 < args.Length) {
            var values = args[i + 1].Split(',');
            if (values.Length == 2){
                n=int.Parse(values[0]);
				m=int.Parse(values[1]);
            }
        }
		if (args[i] == "-verbose" && i+1 < args.Length){
			verbose = int.Parse(args[i+1]);
		}
    }
	
	var rnd = new System.Random(1); 
	var A = new matrix(n,m);
	var b = new vector(n);
	for (int i=0; i<n; i++){
		for (int j=0; j<m; j++){
			A.set(i,j,rnd.NextDouble());
		}
	}
	//A.print("random matrix A");
	for (int i=0; i<n; i++){
		b[i]=rnd.NextDouble();
	}
	//b.print("random vector b:");

	// init solver, do QR decomp
	var solver = new QR(A,b);

	matrix Q = (solver.Q).copy();
	matrix R = (solver.R).copy();
	solver.solve();
	
	
	// do checks with Q and R
	var isThisId = Q.T * Q;
	var isThisA = Q*R;
	
	var isThisB1 = (A*solver.b);
	var isThisB2 = (isThisA*solver.b);

	if (verbose ==1){
		WriteLine($"Q.T*Q = Id? {isThisId.approx(matrix.id(m))}");
		WriteLine($"Q*R= A? {A.approx(isThisA)}");
		WriteLine($"A*x=b? {b.approx(isThisB1)}");
		WriteLine($"Q*R*x=b? {b.approx(isThisB2)}");
		if(verbose == 2){
			R.print("this is R (should be upper triangular)");
			b.print("This is b");
			isThisB1.print($"A*x= ");
			isThisB2.print($"Q*R*x= ");
		}
	}
	if (isThisId.approx(matrix.id(m)) && A.approx(isThisA) && b.approx(isThisB1) && b.approx(isThisB2) && verbose == 1){
		WriteLine(0);
	}
	else if (verbose == 1){
		WriteLine(1);
	}

}
}
