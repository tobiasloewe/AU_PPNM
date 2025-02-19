class main{
    public static int Main(){
        for(double x=-3;x<=3;x+=1.0/8){
		    System.Console.WriteLine($"{x} {sfuns.erf(x)}");
		}
        System.Console.WriteLine("\n");
        for(double x=10.0/64; x<=10; x+=10.0/64){
            System.Console.WriteLine($"{x} {sfuns.sgamma(x)}");
        }
        return 0;
    }
}