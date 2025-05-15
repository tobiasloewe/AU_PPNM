using System;
using static System.Math;
using System.Linq;


class Program
{
    static void Main(string[] args)
    {
        int samples = 100;
        vector xinputs = new vector(samples);
        vector yinputs = new vector(samples);
        
        ANN myNetwork = new ANN(5); // Create a neural network with 3 hidden neurons 
        Console.WriteLine("Neural Network created with 3 hidden neurons.");

        Func<double, double> g = x => Math.Cos(5 * x - 1) * Math.Exp(-x * x);
        Random rand = new Random();
        for (int i =0; i < samples; i++)
        {
            xinputs[i] = rand.NextDouble() * 2 - 1; 
            yinputs[i] = g(xinputs[i]); // Generate y values using the function g
        }
        
        myNetwork.train(xinputs, yinputs);
        myNetwork.train(xinputs, yinputs);
        myNetwork.train(xinputs, yinputs); // Train the network with the generated data
        double cost = 0.0;
        for (int i = 0; i < samples; i++)
        {
            cost += (yinputs[i] - myNetwork.response(xinputs[i])) * (yinputs[i] - myNetwork.response(xinputs[i]));
            Console.WriteLine($"x: {xinputs[i]}, y: {yinputs[i]}, response: {myNetwork.response(xinputs[i])}");
        }
        cost /= samples;
        Console.WriteLine($"Mean Squared Error: {cost}");
    }
}