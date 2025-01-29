using static System.Console;
using sfuns;

class math{
   static void Main(){
      double sqrt2=Math.Sqrt(2.0);
      Write($"sqrt2^2 = {sqrt2*sqrt2} (should equal 2)\n");
      Write($"Gamma(1) = {fgamma(1.0)}\n");
      Write($"Gamma(3) = {fgamma(3.0)}\n");
   }
}
