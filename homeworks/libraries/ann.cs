using System;
using static System.Math;

public class ANN{
    private int n;                                      /* number of hidden neurons */
    private Func<double,double> activation;   /* activation function */
    private vector pars_;
    private double learnRate;                                /* network parameters */
    public readonly vector x;
    public readonly vector y;
    public static Func<vector, vector, double> mseLoss = (vector x, vector y) => {
        double loss = 0.0;
        for(int i=0;i<x.size;i++){
            double d=y[i]-x[i];
            loss += d*d;
        }
        return loss/x.size;
    };

    public ANN(int n, vector xinit, vector yinit, double initialLearnRate = 0.1, string acti = "gausswave", vector defaultPars = null){
        if (defaultPars == null) defaultPars = new vector(1.0,1.0,1.0);

        if (acti == "tanh") this.activation = (double x) => { return (1.0 - Exp(-2*x))/(1.0 + Exp(-2*x)); };
        else if (acti == "sigmoid") this.activation = (double x) => { return 1.0/(1.0 + Exp(-x)); };
        else if (acti == "relu") this.activation = (double x) => { return Max(0,x); };
        else if (acti == "linear") this.activation = (double x) => { return x; };
        else if (acti == "gausswave") this.activation = (double x) => {return x*Exp(-x*x);};
        else throw new ArgumentException("Unknown activation function");

        this.n=n; this.pars_=new vector(n*3);
        this.x=xinit; this.y=yinit;
        this.learnRate=initialLearnRate;
        var rand = new Random();
        for (int i=0;i<this.n;i++){
            this.pars_[3*i]=defaultPars[0]*2*rand.NextDouble()-1; /* input scaling */
            this.pars_[1+3*i]=defaultPars[1]*2*rand.NextDouble()-1; /* output scaling */
            this.pars_[2+3*i]=defaultPars[2]*2*rand.NextDouble()-1; /* weight */
        }
    }
    public double response(double x, vector pars = null){
        if (pars == null) pars = this.pars_;
        double yguess =0.0;
        for (int i=0;i<this.n;i++){
            yguess+=activation((x-pars[3*i])/pars[1+3*i])*pars[2+3*i];
        }
        return yguess;
    }
    public vector response(vector x, vector pars = null){
        if (pars == null) pars = this.pars_;
        vector yguess = new vector(x.size);
        for (int i=0;i<this.n;i++){
            yguess[i] = response(x[i], pars);
        }
        return yguess;
    }
    public double currentLoss(vector pars = null){
        if (pars == null) pars = this.pars_;
        return ANN.mseLoss(this.response(this.x, pars), this.y);
    }
    public void train(int n = 1)
    {
        for (int i = 0; i < n; i++)
        {
            vector gradVal = gradient(this.currentLoss, this.pars_);
        gradVal /= gradVal.norm(); // Normalize the gradient
        this.pars_ -= this.learnRate * gradVal;
        gradVal.print("Gradient");
        }
    }
    public double currentLoss(vector x, vector y){
        return ANN.mseLoss(this.response(x), y);
    }
    private static vector gradient(Func<vector, double> φ, vector x)
    {
        double φx = φ(x); // Evaluate φ at the current point
        vector gφ = new vector(x.size); // Initialize gradient vector

        for (int i = 0; i < x.size; i++)
        {
            double dxi = Max(1e-8, Abs(x[i]) * 1e-5); // Relative step size
            vector xCopy = x.copy();
            xCopy[i] += dxi;
            gφ[i] = (φ(xCopy) - φx) / dxi; // Compute partial derivative
        }

        return gφ; // Return the gradient vector
    }
    public void setLearnRate(double newLearnRate)
    {
        this.learnRate = newLearnRate;
    }
    public double firstDerivative(double x, vector pars = null) {
        if (pars == null) pars = this.pars_;
        double result = 0.0;
        for (int i = 0; i < this.n; i++) {
            double a = pars[3 * i];       // Output scaling
            double b = pars[3 * i + 1];   // Input scaling
            double c = pars[3 * i + 2];   // Weight
            double z = b * x + c;
            result += a * (Exp(-z * z) - 2 * z * z * Exp(-z * z)) * b; // φ'(z) * b
        }
        return result;
    }

    public double secondDerivative(double x, vector pars = null) {
        if (pars == null) pars = this.pars_;
        double result = 0.0;
        for (int i = 0; i < this.n; i++) {
            double a = pars[3 * i];       // Output scaling
            double b = pars[3 * i + 1];   // Input scaling
            double c = pars[3 * i + 2];   // Weight
            double z = b * x + c;
            result += a * (-6 * z * Exp(-z * z) + 4 * z * z * z * Exp(-z * z)) * b * b; // φ''(z) * b^2
        }
        return result;
    }

    public double antiDerivative(double x, vector pars = null) {
        if (pars == null) pars = this.pars_;
        double result = 0.0;
        for (int i = 0; i < this.n; i++) {
            double a = pars[3 * i];       // Output scaling
            double b = pars[3 * i + 1];   // Input scaling
            double c = pars[3 * i + 2];   // Weight
            double z = b * x + c;
            result += a * (-0.5 * Exp(-z * z)) / b; // Φ(z) / b
        }
        return result;
    }
}