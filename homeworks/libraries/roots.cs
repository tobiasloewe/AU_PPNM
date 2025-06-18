
using static System.Math;
using System;

public static class Roots
{
    public static vector newton(
        Func<vector,vector> f /* the function to find the root of */
        ,vector start        /* the start point */
        ,double acc=1e-2     /* accuracy goal: on exit ‖f(x)‖ should be <acc */
        ,vector δx=null      /* optional δx-vector for calculation of jacobian */
        ,double λmin=1e-7   /* minimum step length for linesearch */
    ){
    vector x=start.copy();
    vector fx=f(x),z,fz;
    Console.Error.WriteLine("before do");
    do{ /* Newton's iterations */
        x.print("x: ", file: System.Console.Error);
        if(fx.norm() < acc) break; /* job done */
        matrix J=jacobian(f,x,fx,δx);
        Console.Error.WriteLine("after jacobian");
        vector Dx = QR.solve(J,-fx); /* Newton's step */
        Console.Error.WriteLine("after QR.solve");
        double λ=1;
        do{ /* linesearch */
            z=x+λ*Dx;
            Console.Error.WriteLine("doloop");
            z.print("z: ", file: System.Console.Error);
            fz=f(z);
            Console.Error.WriteLine("after fz");
            if( fz.norm() < (1-λ/2)*fx.norm() ) break;
            if( λ < λmin ) break;
            λ/=2;
            }while(true);
        x=z; fx=fz;
        }while(true);
    return x;
    }
    public static matrix jacobian(
        Func<vector,vector> f,
        vector x,
        vector fx=null,
        vector dx=null)
        {
        if(dx == null) dx = x.map(xi => Abs(xi)*Pow(2,-26));
        if(fx == null) fx = f(x);
        matrix J=new matrix(x.size);
        for(int j=0;j < x.size;j++){
            x[j]+=dx[j];
            vector df=f(x)-fx;
            for(int i=0;i < x.size;i++) J[i,j]=df[i]/dx[j];
            x[j]-=dx[j];
            }
        return J;
    }
    
}
