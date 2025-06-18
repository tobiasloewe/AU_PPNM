using System;
using static System.Console;
using static System.Math;

public class epsilon
{
    public static bool approx(double a, double b, double acc=1e-9, double eps=1e-9){
        if(Abs(b-a) <= acc) return true;
        if(Abs(b-a) <= Max(Abs(a),Abs(b))*eps) return true;
        return false;
    }
    // Find the largest integer before overflow
    public static int testMaxInt()
    {
        int i=1; while(i+1>i) {i++;}
        return i; // Last valid positive int
    }
    public static int testMinInt()
    {
        int i=-1; while(i-1<i) {i--;}
        return i; // Last valid negative int
    }

    // Compute machine epsilon for double
    public static double testMinDouble()
    {
        double epsilon = 1.0;
        while (1.0 + epsilon != 1.0)
        {
            epsilon /= 2.0;
        }
        return epsilon * 2.0;
    }

    // Compute machine epsilon for float
    public static float testMinFloat()
    {
        float epsilon = 1.0F;
        while (1.0F + epsilon != 1.0F)
        {
            epsilon /= 2.0F;
        }
        return epsilon * 2.0F;
    }
    public static void tinyTest(){
        double epsilon=Pow(2,-52);
        double tiny=epsilon/2;
        double a=1+tiny+tiny;
        double b=tiny+tiny+1;
        Write($"a==b ? {a==b}\n");
        Write($"a>1  ? {a>1}\n");
        Write($"b>1  ? {b>1}\n");
        WriteLine($"tiny = {tiny}\nepsilon = {epsilon}\ntiny/2 = {tiny/2}");
        WriteLine($"a = {a} and b = {b}");
    }
    public static void doubleComparisons(){
        double d1 = 0.1+0.1+0.1+0.1+0.1+0.1+0.1+0.1;
        double d2 = 8*0.1; 
        WriteLine($"d1={d1:e15}");
        WriteLine($"d2={d2:e15}");
        WriteLine($"d1==d2 ? => {d1==d2}"); 
        WriteLine("0.1 cannot be accurately represented");
        WriteLine($"using new approx function for comparison we get: {approx(d1,d2)}");
    }
    
}

