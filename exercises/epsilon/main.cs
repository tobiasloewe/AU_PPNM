class main{
static int Main(){
        System.Console.WriteLine($"Int.MaxValue = {int.MaxValue}");
		System.Console.WriteLine($"My max int: {epsilon.testMaxInt()}");

        System.Console.WriteLine($"My machine epsilon for float: {epsilon.testMinDouble()}");
        System.Console.WriteLine($"Compare with 2^(-52) = {System.Math.Pow(2,-52)}");
        System.Console.WriteLine($"My machine epsilon for double: {epsilon.testMinFloat()}");
        System.Console.WriteLine($"Compare with 2^(-23) = {System.Math.Pow(2,-23)}");
		return 0;
	}
}