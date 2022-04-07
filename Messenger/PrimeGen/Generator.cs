// Author - Benjamin Jordan, bej9038
// File - Generator.cs
// Description - Contains the prime Generator class.

using System.Numerics;
namespace Messenger.PrimeGen;

/// <summary>
/// Generator object used to generate large prime integers
/// </summary>
public class Generator
{
    private const uint LoopIterations =  4294967295; // number of loop iterations

    /// <summary>
    /// Runs the generator.
    /// </summary>
    public BigInteger RunGenerator(int bitCount)
    {
        BigInteger prime = new BigInteger();

        Parallel.For(0, LoopIterations, (i, state) => {
            
            BigInteger x = BigIntegerExtensions.GenerateBigIntInRange(0, -1, bitCount);
            
            if (x == 2 || x == 3)
            {
                prime = x;
                state.Stop();
            }
            else 
            { 
                if (BigInteger.Compare(x, 3) > 0 && 
                    BigInteger.Remainder(x, 2) != 0 &&
                    BigInteger.Remainder(x, 3) != 0 &&
                    BigInteger.Remainder(x, 5) != 0) 
                { 
                    if (x.IsProbablyPrime())
                    {
                        prime = x;
                        state.Stop();
                    }
                } 
            }
        });

        return prime;
    }
}