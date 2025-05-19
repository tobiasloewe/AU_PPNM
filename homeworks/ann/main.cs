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
        vector xtrain = new vector(trainSamples);
        vector ytrain = new vector(trainSamples);
        vector xval = new vector(valSamples);
        vector yval = new vector(valSamples);
        vector xtest = new vector(testSamples);
        vector ytest = new vector(testSamples);
        

        // Suggestion: Use a smaller learning rate, normalize input, and initialize weights carefully.
        // Also, try increasing the number of neurons or epochs if underfitting.
        Func<double, double> g = x => Math.Cos(5 * x - 1) * Math.Exp(-x * x);

        // Normalize input data to mean 0, std 1 (if not already)
        // Consider using a smaller learning rate in your ANN class (e.g., 0.01 or 0.001)
        // Try more neurons (e.g., ANN myNetwork = new ANN(10);)
        // Optionally, add regularization or early stopping
        Random rand = new Random();
        for (int i =0; i < trainSamples; i++)
        {
            xtrain[i] = rand.NextDouble() * 2 - 1; 
            ytrain[i] = g(xtrain[i]); // Generate y values using the function g
        }
        for (int i =0; i < valSamples; i++)
        {
            xval[i] = rand.NextDouble() * 2 - 1; 
            yval[i] = g(xval[i]); // Generate y values using the function g
        }
        for (int i =0; i < testSamples; i++)
        {
            xtest[i] = rand.NextDouble() * 2 - 1; 
            ytest[i] = g(xtest[i]); // Generate y values using the function g
        }
        // Train the network
        ANN myNetwork = new ANN(4, xtrain, ytrain);

        int epochs = 2;
        double maxLoss = 10.0;
        double minLoss = 0.01;
        for (int epoch = 0; epoch < epochs; epoch++)
        {
            myNetwork.train();
            double trainLoss = myNetwork.currentLoss();
            double valLoss = myNetwork.currentLoss(xval, yval);
            Console.WriteLine($"Epoch {epoch + 1}\ntrain loss: {trainLoss}  val loss: {valLoss}");
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