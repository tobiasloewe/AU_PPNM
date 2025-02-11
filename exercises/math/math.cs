using static System.Console;
using static System.Math;

public class math{
   public static void testSomeMath(){
      double sqrt2=Sqrt(2.0);
      WriteLine($"sqrt2^2 = {sqrt2*sqrt2} (should equal 2)");
      double twoToOneFifth = Pow(2,0.2);
      WriteLine($"2^(1/5) = {twoToOneFifth} (should be 1.148698)");
      double eToPi = Pow(E,PI);
      WriteLine($"e^pi = {eToPi} (should be 23.140692)");
      double piToE = Pow(PI,E);
      WriteLine($"pi^e = {piToE} (should be 22.459158)");

      for (double i=1; i<11.0; i++){
         Write($"Gamma({i}) = {sfuns.fgamma(i):F6}\n");
      }
      for (double j=50.0; j<111.0; j += 20){
         Write($"LnGamma({j}) = {sfuns.lngamma(j):F6}\n");
      }
   }
}
