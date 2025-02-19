class main{
    public static int Main(){
        for(double x=-3;x<=3;x+=1.0/8){
		    System.Console.WriteLine($"{x} {sfuns.erf(x)}");
		}
        System.Console.WriteLine("\n");
        for(double x=-5; x<=10; x+=0.002){
            System.Console.WriteLine($"{x} {sfuns.sgamma(x)}");
        }
        System.Console.WriteLine("\n");
        for(double x=1; x<=100; x+=0.5){
            System.Console.WriteLine($"{x} {sfuns.lngamma2(x)}");
        }
        return 0;
    }
}