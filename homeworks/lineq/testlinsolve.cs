using static System.Console;

class TestLinSolve{
static void Main(string[] args){
	// init vars
	int n=2;
	int m=2;
	// IO
	for (int i = 0; i < args.Length; i++) {
        if (args[i] == "-dims" && i + 1 < args.Length) {
            var values = args[i + 1].Split(',');
            if (values.Length == 2){
                n=int.Parse(values[0]);
				m=int.Parse(values[1]);
            }
        }
	}

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
	vector x = QR.solve(A,b);
	
	vector isThisB1 = (A*x);
	matrix Ainv = (QR.inverse(A));
	matrix isThisId2 = (A*Ainv);

	WriteLine($"This is tests Linear Solver for Dims {n} x {m}");
	WriteLine($"A*x=b? {b.approx(isThisB1)}");
	WriteLine($"A*Ainv=Id? {matrix.id(m).approx(isThisId2)}");

	isThisId2.print("A*Ainv (should be Id)");
	b.print("This is b");
	isThisB1.print($"A*x= ");
	}
}
	
