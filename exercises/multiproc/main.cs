using static System.Console;
using System.Linq;

class main{

public class datum {public long start,stop; public double sum;};

public static void harm(object obj){
	datum d = (datum)obj;
	d.sum=0;
	for(long i=d.start+1;i<=d.stop;i++)d.sum+=1.0/i;
	}

public static int Main(string[] argv){
    int mode = 0;
	int argc=argv.Length;
	int nthreads=1;
    long nterms=(long)1e8;
	for(int i=0;i<argc;i++){
		string arg = argv[i];
		if(arg=="-threads" && i+1<argc) nthreads=int.Parse(argv[i+1]);
		if(arg=="-terms" && i+1<argc) nterms=(long)double.Parse(argv[i+1]);
        if(arg=="-mode" && i+1<argc) mode= 0;
	}
    if (mode==0){
        Error.WriteLine($"nthreads={nthreads} nterms={nterms}");
        var threads = new System.Threading.Thread[nthreads];
        var data = new datum[nthreads];
        for(int i=0;i<nthreads;i++){
            data[i] = new datum();
            data[i].start = i*nterms/nthreads;
            data[i].stop = (i+1)*nterms/nthreads;
            }
        for(int i=0;i<nthreads;i++){
            threads[i]=new System.Threading.Thread(harm);
            threads[i].Start(data[i]);
            }
        foreach(var thread in threads) thread.Join();
        double sum=0;
        foreach(var d in data) sum+=d.sum;
        WriteLine($"sum={sum}");
    }
    else if(mode == 1){
        double sum=0;
        System.Threading.Tasks.Parallel.For( 1, nterms+1, (long i) => sum+=1.0/i );
        WriteLine($"sum={sum} , with mode 1");
    }
    else if (mode == 2){        
        var sum = new System.Threading.ThreadLocal<double>( ()=>0, trackAllValues:true);
        System.Threading.Tasks.Parallel.For( 1, nterms+1, (long i)=>sum.Value+=1.0/i );
        double totalsum=sum.Values.Sum();
        WriteLine($"sum={sum} , with mode 2");
    }
return 0;
}//Main

}//class main