using System;

public class epsilon
{
    // Find the largest integer before overflow
    public static int testMaxInt()
    {
        int max = 1;
        while (max > 0) // Stop when overflow makes it negative
        {
            max *= 2;
        }
        return max - 1; // Last valid positive int
    }

    // Compute machine epsilon for double
    public static double testMinDouble()
    {
        double epsilon = 1.0;
        while (1.0 + epsilon != 1.0)
        {
            epsilon /= 2.0;
        }
        return epsilon * 2.0;
    }

    // Compute machine epsilon for float
    public static float testMinFloat()
    {
        float epsilon = 1.0F;
        while (1.0F + epsilon != 1.0F)
        {
            epsilon /= 2.0F;
        }
        return epsilon * 2.0F;
    }
}
