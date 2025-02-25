using static System.Console;

class main{
static void Main(){
	int n = 10;
	int m = 3;
	var rnd = new System.Random(1); /* or any other seed */
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
	solver.solve();

	(solver.b).print("solved?");
	var isThisB1 = isThisA*solver.b;
	var isThisB2 = A*solver.b;
	isThisB1.print("Q*R*x (should be b)");
	WriteLine($"Equals b? {b.approx(isThisB1)}");
	isThisB2.print("A*x (should be b)");
	WriteLine($"Equals b? {b.approx(isThisB2)}");
}
}