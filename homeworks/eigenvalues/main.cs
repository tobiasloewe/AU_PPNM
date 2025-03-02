using static System.Console;

class main{
static void Main(string[] args){
	// init vars
	int n=2;
	int verbose = 0;
	string mode = "";

	// IO
	for (int i = 0; i < args.Length; i++) {
        if (args[i] == "-dim" && i + 1 < args.Length){
            n=int.Parse(args[i+1]);
        }
		if (args[i] == "-verbose" && i+1 < args.Length){
			verbose = int.Parse(args[i+1]);
        }
    }
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

    A.print();
}
}//main