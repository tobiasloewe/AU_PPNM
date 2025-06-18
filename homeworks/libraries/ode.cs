using System;
using static System.Math;

public class ODE
{
    public static (vector,vector) rkstep12(
        Func<double,vector,vector> f,/* the f from dy/dx=f(x,y) */
        double x,                    /* the current value of the variable */
        vector y,                    /* the current value y(x) of the sought function */
        double h                     /* the step to be taken */
        )
    {
        vector k0 = f(x,y);              /* embedded lower order formula (Euler) */
        vector k1 = f(x+h/2,y+k0*(h/2)); /* higher order formula (midpoint) */
        vector yh = y+k1*h;              /* y(x+h) estimate */
        vector δy = (k1-k0)*h;           /* error estimate */
        return (yh,δy);
    }

    // Runge-Kutta 2nd/3rd order step function
    public static (vector, vector) rkstep23(
        Func<double, vector, vector> F, // Function representing the system: dy/dx = F(x, y)
        double x,  // Current x-value (independent variable)
        vector y,  // Current y-value (state of the system)
        double h   // Step size
    ) { 
        // Compute the Runge-Kutta increments (intermediate steps)
        vector k0 = F(x, y);                       // k0: Evaluate function at (x, y)
        vector k1 = F(x + h / 2, y + k0 * (h / 2)); // k1: Midpoint evaluation
        vector k2 = F(x + 3 * h / 4, y + k1 * (3 * h / 4)); // k2: 3/4 step evaluation

        // Compute the next value approximation using weighted sum
        vector ka = k0 * (2.0 / 9) + k1 * (3.0 / 9) + k2 * (4.0 / 9); 
        vector kb = k1; // 2nd-order estimate
        
        // Compute the next state estimate
        vector yh = y + ka * h; 
        
        // Estimate error using difference between two different approximations
        vector er = (ka - kb) * h; 

        return (yh, er); // Return estimated next value and error
    }

    public static (genlist<double>,genlist<vector>) driver(
        Func<double,vector,vector> F,/* the f from dy/dx=f(x,y) */
        (double,double) interval,    /* (initial-point,final-point) */
        vector yinit,                /* y(initial-point) */
        double h=0.125,              /* initial step-size */
        double acc=0.01,             /* absolute accuracy goal */
        double eps=0.01,              /* relative accuracy goal */
        int minsteps=30              /* minimum number of steps */
    ){
    var (a,b)=interval; double x=a; vector y=yinit.copy();
    var xlist=new genlist<double>(); xlist.add(x);
    var ylist=new genlist<vector>(); ylist.add(y);
    double hmax= (b-a)/minsteps; // maximum step size
    do{
        if(x>=b) return (xlist,ylist); /* job done */
        if(x+h>b) h=b-x;               /* last step should end at b */
        var (yh,δy) = rkstep23(F,x,y,h);
        double tol = (acc+eps*yh.norm()) * Sqrt(h/(b-a));
        double err = δy.norm();
        if(err<=tol){ // accept step
            x+=h; y=yh;
            xlist.add(x);
            ylist.add(y);
            }
        if (h<hmax){
            if(err>0) h *= Min( Pow(tol/err,0.25)*0.95 , 2); // readjust stepsize
            else h*=2;
        }
        else{
            h=hmax; // reset to maximum step size
        }

        }while(true);
    }//driver


}