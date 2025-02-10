using System;
using static System.Math;
using static System.Console;
class main{
    public static void Main(string[] args){
        foreach(var arg in args){
            var words = arg.Split(':');
            if(words[0]=="-numbers"){
                var numbers=words[1].Split(',');
                foreach(var number in numbers){
                    double x = double.Parse(number);
                    WriteLine($"{x} {Sin(x)} {Cos(x)}");
                    }
                }
            if(words[0]=="-weirdNumbers"){
                Error.WriteLine("Numbers extracted from weird input");
                char[] split_delimiters = {' ','\t','\n'};
                var split_options = StringSplitOptions.RemoveEmptyEntries;
                for( string line = In.ReadLine(); line != null; line = In.ReadLine() ){
	                var numbers = line.Split(split_delimiters,split_options);
	                foreach(var number in numbers){
		                double x = double.Parse(number);
		                Error.WriteLine($"{x} {Sin(x)} {Cos(x)}");
                    }
                }
            }
        }
    }
}
