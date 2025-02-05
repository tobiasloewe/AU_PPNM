using static System.Console;

static class main{
    static int Main(){
        var rnd=new System.Random();
        var u=new vec(rnd.NextDouble(),rnd.NextDouble(),rnd.NextDouble());
        var v=new vec(rnd.NextDouble(),rnd.NextDouble(),rnd.NextDouble());
        u.print("u=");
        v.print("u=");
        WriteLine($"u={u}");
        WriteLine($"v={v}");
        WriteLine();
        return 0;
    }
}