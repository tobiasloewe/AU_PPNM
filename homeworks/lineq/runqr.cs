using static System.Console;

class runqr{
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

    (matrix Q, matrix R) = QR.decomp(A);

}
	

}