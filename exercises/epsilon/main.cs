class main{
static int Main(){
        int myMaxInt = epsilon.testMaxInt();
        System.Console.WriteLine($"Int.MaxValue = {int.MaxValue}");
		System.Console.WriteLine($"My max int: {myMaxInt}");
        
        double myMinDouble = epsilon.testMinDouble();
        System.Console.WriteLine($"My machine epsilon for double: {myMinDouble}}");
        System.Console.WriteLine($"Compare with 2^(-52) = {System.Math.Pow(2,-52)}");
        
        float myMinFloat = epsilon.testMinFloat();
        System.Console.WriteLine($"My machine epsilon for float: {myMinFloat}");
        System.Console.WriteLine($"Compare with 2^(-23) = {System.Math.Pow(2,-23)}");
		return 0;
	}
}