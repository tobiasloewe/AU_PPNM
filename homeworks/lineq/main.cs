using static System.Console;

class main{
static void Main(){
	/*
	vector ve;
	ve=new vector(n:1); ve.print("ve=");
	ve=new vector(1,2,3); ve.print("ve=");
	ve=new vector(7,8,9,8,7); ve.print("ve=");
	var ma=new matrix("1 2 ; 5 6");
	ma.print();
	(ma+ma.T).print();
	(matrix.id(2)).print();
	*/
	var A = new matrix("1 2 3; 4 5 6; 7 8 9; 10 11 12; 13 14 15; 16 17 18");
	var solver = new QR(A);
	A.print();

	matrix Q = (solver.Q).copy();
	matrix R = (solver.R).copy();
	var Mat = Q.T * Q;
	Mat.print();
	WriteLine($"{Q.size()[0]} {Q.size()[1]}");
}
}