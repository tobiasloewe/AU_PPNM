using System;
using static System.Math;

public class ANN{
    private int n; /* number of hidden neurons */
    private Func<double,double> f = x => x*Exp(-x*x); /* activation function */
    private vector pars; /* network parameters */

    public ANN(int n){
        this.n=n; this.pars=new vector(n*3);
        for (int i=0;i<this.n;i++){
            this.pars[3*i]=1.0; /* input scaling */
            this.pars[1+3*i]=1.0; /* output scaling */
            this.pars[2+3*i]=1.0; /* weight */
        }
    }
    public double response(double x){
      /* return the response of the network to the input signal x */
        double yguess =0.0;
        for (int i=0;i<this.n;i++){
            yguess+=f((x-this.pars[3*i])/this.pars[1+3*i])*this.pars[2+3*i];
        }
        return yguess;
    }
    public void train(vector x,vector y){
        /* train the network to interpolate the given table {x,y} */
        Func<vector,double> f = (vector input) => this.mseCost(input, y); /* activation function */

        var (newpars, steps) = Min.newton(f,this.pars,1e-3,1000);
        this.pars=newpars;
    }
    double mseCost(vector x,vector y){
        double c=0;
        for (int i=0;i<x.size;i++){
            double d=y[i]-response(x[i]);
            c+=d*d;
        }
        return c/x.size;
    }
}