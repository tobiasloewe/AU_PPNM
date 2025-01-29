using static System.Console;
using static System.Math;

public class math{
   public static void testSomeMath(){
      double sqrt2=Sqrt(2.0);
      Write($"sqrt2^2 = {sqrt2*sqrt2} (should equal 2)\n");
      Write($"Gamma(1) = {sfuns.fgamma(1.0)}\n");
      Write($"Gamma(3) = {sfuns.fgamma(3.0)}\n");
   }
}
