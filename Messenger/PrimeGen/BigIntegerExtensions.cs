using System.Numerics;
using System.Security.Cryptography;
namespace PrimeGen;

/// <summary>
/// Title: BigIntegerExtensions
/// Description: BigInteger extension to figure out if a BigInteger is prime and a function
/// to generate BigInts.
/// Author: Ben Jordan
/// 
/// </summary>
public static class BigIntegerExtensions
{
    /// <summary>
    /// Determines if a BigInteger is prime or not
    /// </summary>
    /// <param name="value"> The BigInteger </param>
    /// <param name="k"> The number of witness loops </param>
    /// <returns> Whether the value is prime or not </returns>
    public static Boolean IsProbablyPrime(this BigInteger value, int k = 10)
    {
        var d = value - 1;
        var r = 0;
        while (d % 2 == 0)
        {
            d /= 2;
            r++;
        }

        for (int i = 0; i < k; i++)
        {
            var a = GenerateBigIntInRange(2, value - 2, value.GetByteCount());
            BigInteger x = BigInteger.ModPow(a, d, value);
            if (!(x == 1 || x == value - 1))
            {
                bool foundX = false;
                int j = 1;
                while (j < r && foundX == false)
                {
                    x = BigInteger.Remainder(BigInteger.Multiply(x, x), value); 
                    if (x == value - 1)
                    {
                        foundX = true;
                    }

                    j++;
                }

                if (!foundX)
                {
                    return false;   
                }
            }
        }
        
        return true;
    }

    /// <summary>
    /// Generates a BigInteger inside a given range and with a specified byte count.
    /// </summary>
    /// <param name="lowerBounds"> The lower bounds of the BigInteger </param>
    /// <param name="upperBounds"> The upper bounds of the BigInteger </param>
    /// <param name="byteCount"> The byte count </param>
    /// <returns> The pseudorandom BigInteger </returns>
    public static BigInteger GenerateBigIntInRange(BigInteger lowerBounds, BigInteger upperBounds, int byteCount)
    {
        byte[] bytes = { };
        BigInteger x = new BigInteger(bytes);
        if (upperBounds == -1) // no upper bounds
        {
            bytes = RandomNumberGenerator.GetBytes(byteCount);
            x = new BigInteger(bytes);
        }
        else
        {
            while(x < lowerBounds || x > upperBounds)
            {
                bytes = RandomNumberGenerator.GetBytes(byteCount);
                x = new BigInteger(bytes);
            }    
        }

        return x;
    }
}

