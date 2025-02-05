using static System.Console;

class main{
static int Main(){
        int myMaxInt = epsilon.testMaxInt();
        WriteLine($"Int.MaxValue = {int.MaxValue}");
		WriteLine($"My max int: {myMaxInt}");
        
        double myMinDouble = epsilon.testMinDouble();
        WriteLine($"My machine epsilon for double: {myMinDouble}");
        WriteLine($"Compare with 2^(-52) = {System.Math.Pow(2,-52)}");
        
        float myMinFloat = epsilon.testMinFloat();
        WriteLine($"My machine epsilon for float: {myMinFloat}");
        WriteLine($"Compare with 2^(-23) = {(float)System.Math.Pow(2,-23)}");
        
        WriteLine("\nTesting with tiny=epsilon/2");
        epsilon.tinyTest();
        WriteLine("By definition, tiny is half the machine epsilon, which is the smallest difference from 1.0, that\nis representable. Adding half doesn't change anything, also addition is associative.");
    
        WriteLine("\nNow onto double comparisons");
        epsilon.doubleComparisons();
		return 0;
	}
}