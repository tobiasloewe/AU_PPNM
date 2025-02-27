using static System.Console;

class main{
static void Main(string[] args){
	int n=2;
	int m=2;
	int argc=args.Length;
	for (int i = 0; i < args.Length; i++) {
        if (args[i] == "-dims" && i + 1 < args.Length) {
            var values = args[i + 1].Split(',');
            if (values.Length == 2){
                n=int.Parse(values[0]);
				m=int.Parse(values[1]);
            }
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

	// do checks with Q and R
	var isThisId = Q.T * Q;
	var isThisA = Q*R;
	R.print("this is R (should be upper triangular)");
	//isThisId.print("\nthis is Q.T times Q");
	WriteLine($"Q.T*Q = Id? {isThisId.approx(matrix.id(m))}\n");
	//isThisA.print("this is Q times R");
	WriteLine($"Q*R= A? {A.approx(isThisA)}\n");

	// solve Lineq
	b.print("This is b");

	solver.solve();
	var isThisB1 = (A*solver.b);
	var isThisB2 = (isThisA*solver.b);
	isThisB1.print($"A*x=b? {b.approx(isThisB1)}");
	isThisB2.print($"Q*R*x=b? {b.approx(isThisB2)}");
}
}
