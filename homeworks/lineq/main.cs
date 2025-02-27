using static System.Console;

class main{
static void Main(string[] args){
	// init vars
	int n=2;
	int m=2;
	int verbose = 0;
	string mode = "qr";

	// IO
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
		if (args[i] == "-mode" && i+1 < args.Length){
			mode = args[i+1];
		}
    }

	if (mode == "qr"){
		// start
		var rnd = new System.Random(1); 
		var A = new matrix(n,m);
		var b = new vector(n);
		for (int i=0; i<n; i++){
			for (int j=0; j<m; j++){
				A.set(i,j,rnd.NextDouble());
			}
		}
		for (int i=0; i<n; i++){
			b[i]=rnd.NextDouble();
		}

		// init solver, do QR decomp
		var solver = new QR(A,b);
		matrix Q = (solver.Q).copy();
		matrix R = (solver.R).copy();

		// do checks with Q and R
		matrix isThisId = Q.T * Q;
		matrix isThisA = Q*R;

		if (verbose !=0){
			WriteLine($"This is mode QR for Dims {n} x {m}");
			if (verbose >= 2){
				WriteLine($"Q.T*Q = Id? {matrix.id(m).approx(isThisId)}");
				WriteLine($"Q*R= A? {A.approx(isThisA)}");
				if(verbose == 3){
					R.print("this is R (should be upper triangular)");
				}
			}
		}
	}
	else if (mode == "all"){
		// start
		var rnd = new System.Random(1); 
		var A = new matrix(n,m);
		var b = new vector(n);
		for (int i=0; i<n; i++){
			for (int j=0; j<m; j++){
				A.set(i,j,rnd.NextDouble());
			}
		}
		for (int i=0; i<n; i++){
			b[i]=rnd.NextDouble();
		}

		// init solver, do QR decomp
		var solver = new QR(A,b);
		matrix Q = (solver.Q).copy();
		matrix R = (solver.R).copy();

		// do checks with Q and R
		matrix isThisId = Q.T * Q;
		matrix isThisA = Q*R;
		
		solver.solve();
		
		vector isThisB1 = (A*solver.b);
		vector isThisB2 = (isThisA*solver.b);
		matrix Ainv = (QR.inverse(A));
		matrix isThisId2 = (A*Ainv);

		if (verbose >=0){
			WriteLine($"This is mode All for Dims {n} x {m}");
			if (verbose >= 2){
				WriteLine($"Q.T*Q = Id? {matrix.id(m).approx(isThisId)}");
				WriteLine($"Q*R= A? {A.approx(isThisA)}");
				WriteLine($"A*x=b? {b.approx(isThisB1)}");
				WriteLine($"Q*R*x=b? {b.approx(isThisB2)}");
				WriteLine($"A*Ainv=b? {matrix.id(m).approx(isThisId2)}");
			
				if(verbose == 3){
					R.print("this is R (should be upper triangular)");
					isThisId2.print("A*Ainv (should be Id)");
	
					b.print("This is b");
					isThisB1.print($"A*x= ");
					isThisB2.print($"Q*R*x= ");
				}
			}
		
		}
	}
}
	

}

