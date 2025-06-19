using System;
using static System.Math;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        int trainSamples = 1000;
        int valSamples = 300;
        int testSamples = 300;
        string lossfile = null;
        string plotdatafile = null;

        for(int i=0;i<args.Length;i++){
            string arg = args[i];
            if(arg=="-samples" && i+1<args.Length) {
                trainSamples=int.Parse(args[i+1]);
                valSamples = trainSamples / 3;
                testSamples = trainSamples / 3;
            }
            if(arg=="-lossfile" && i+1<args.Length) lossfile=args[i+1];
            if(arg=="-plotdatafile" && i+1<args.Length) plotdatafile=args[i+1];
        }   

        vector xtrain = new vector(trainSamples);
        vector ytrain = new vector(trainSamples);
        vector xval = new vector(valSamples);
        vector yval = new vector(valSamples);

        vector xtest = new vector(testSamples);
        vector ytest = new vector(testSamples);
        vector ytestprime = new vector(testSamples);
        vector ytestdoubleprime = new vector(testSamples);

        // Define the function g and its derivatives
        Func<double, double> g = x => Math.Cos(5 * x - 1) * Math.Exp(-x * x);
        Func<double, double> g_prime = x => -2 * x * Math.Cos(5 * x - 1) * Math.Exp(-x * x)
                            - 5 * Math.Sin(5 * x - 1) * Math.Exp(-x * x);
        Func<double, double> g_doubleprime = x =>
            (4 * x * x - 2) * Math.Cos(5 * x - 1) * Math.Exp(-x * x)
            + (20 * x * Math.Sin(5 * x - 1) - 25 * Math.Cos(5 * x - 1)) * Math.Exp(-x * x);

        for (int i =0; i < trainSamples; i++)
        {
            xtrain[i] = (2.0/trainSamples)*i - 1.0; 
            ytrain[i] = g(xtrain[i]); // Generate y values using the function g
        }
        for (int i =0; i < valSamples; i++)
        {
            xval[i] = (2.0/valSamples)*i - 1.0; 
            yval[i] = g(xval[i]); // Generate y values using the function g
        }
        for (int i =0; i < testSamples; i++)
        {
            xtest[i] = (2.0/testSamples)*i - 1.0; 
            ytest[i] = g(xtest[i]);
            ytestprime[i] = g_prime(xtest[i]);
            ytestdoubleprime[i] = g_doubleprime(xtest[i]);
        }

        // Train the network
        int nNeurons = 7; // Number of neurons in the hidden layer
        int epochs = 100;
        double maxLoss = 10.0;
        double minLoss = 0.1;
        double lrate = 0.05;
        vector initweights = new vector(1.0,1.0,1.0);

        Console.WriteLine($"Training parameters:");
        Console.WriteLine($" neurons = {nNeurons}");
        Console.WriteLine($"  epochs   = {epochs}");
        Console.WriteLine($"  maxLoss  = {maxLoss}");
        Console.WriteLine($"  minLoss  = {minLoss}");
        Console.WriteLine($"  lrate    = {lrate}");
        Console.WriteLine($"  trainSamples = {trainSamples}");
        Console.WriteLine($"  valSamples   = {valSamples}");
        Console.WriteLine($"  testSamples  = {testSamples}");
        
        ANN myNetwork = new ANN(nNeurons, xtrain, ytrain, lrate, "gausswave", initweights);

        if (lossfile != null)
        {
            using (var writer = new System.IO.StreamWriter(lossfile))
            {
                Console.WriteLine($"Writing loss to {lossfile}");
                for (int epoch = 0; epoch < epochs; epoch++)
                {
                    myNetwork.train();
                    double trainLoss = myNetwork.currentLoss();
                    double valLoss = myNetwork.currentLoss(xval, yval);
                    writer.WriteLine($"{epoch + 1} {trainLoss} {valLoss}");
                    if (trainLoss < minLoss || trainLoss > maxLoss || double.IsNaN(trainLoss))
                    {
                        Console.WriteLine($"Training stopped at epoch {epoch + 1}");
                        double testLoss = myNetwork.currentLoss(xtest, ytest);
                        Console.WriteLine($"Test loss: {testLoss}");
                        break;
                    }
                }
            }
        }
        if (plotdatafile != null)
        {
            using (var writer = new System.IO.StreamWriter(plotdatafile))
            {
                Console.WriteLine($"Writing plot data to {plotdatafile}");
                for (int i = 0; i < testSamples; i++)
                {
                    double ypred = myNetwork.response(xtest[i]);
                    double ypredprime = myNetwork.firstDerivative(xtest[i]);
                    double ypreddoubleprime = myNetwork.secondDerivative(xtest[i]);
                    writer.WriteLine($"{xtest[i]} {ytest[i]} {ypred} {ytestprime[i]} {ypredprime} {ytestdoubleprime[i]} {ypreddoubleprime}"); 
                }
            }
        }
}
}