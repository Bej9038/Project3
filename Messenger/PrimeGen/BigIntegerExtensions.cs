// Author - Benjamin Jordan, bej9038
// File - BigIntegerExtensions.cs
// Description - Contains helper functions for BigInteger objects related to prime generation.

using System.Numerics;
using System.Security.Cryptography;
namespace Messenger.PrimeGen;

/// <summary>
/// BigInteger extension to figure out if a BigInteger is prime and a function
/// to generate BigInts
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
    /// <param name="bitCount"> The bit count </param>
    /// <returns> The pseudorandom BigInteger </returns>
    public static BigInteger GenerateBigIntInRange(BigInteger lowerBounds, BigInteger upperBounds, int bitCount)
    {
        int byteCount = BitsToBytes(bitCount);
        byte[] bytes = {};
        BigInteger x = new BigInteger(bytes);
        
        if (upperBounds == -1) // no upper bounds
        {
            // while (x.GetBitLength() != bitCount)
            // {
                bytes = RandomNumberGenerator.GetBytes(byteCount);
                // byte temp = 255;
                // bytes[bytes.Length - 1] = temp;
                x = new BigInteger(bytes);   
            // }
        }
        else
        {
            while (x < lowerBounds || x > upperBounds)
            {
                bytes = RandomNumberGenerator.GetBytes(byteCount);
                x = new BigInteger(bytes); 
            }
        }

        return x;
    }
    
    /// <summary>
    /// Converts a value in bits to the equivalent value in bytes.
    /// </summary>
    /// <param name="bits"> The number of bits </param>
    /// <returns> The number of bytes </returns>
    private static int BitsToBytes(int bits)
    {
        if (bits % 8 == 0)
        {
            return bits / 8;   
        }
        else // arg checks to make sure bits is divisible by 8 so this is redundant but just in case
        {
            return bits / 8 + 1;
        }
    }
}

