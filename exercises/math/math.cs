using static System.Console;
using static System.Math;

public class math{
   public static void testSomeMath(){
      double sqrt2=Sqrt(2.0);
      Write($"sqrt2^2 = {sqrt2*sqrt2} (should equal 2)\n");
      for (double i=1; i<11.0; i++){
         Write($"Gamma({i}) = {sfuns.fgamma(i)}\n");
      }

   }
}
