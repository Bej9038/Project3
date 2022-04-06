using System.Numerics;
namespace Messenger.PrimeGen;


/// <summary>
/// Title: Generator
/// Description: generator object used to generate large prime integers
/// Author: Ben Jordan
/// 
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

        // Boolean primeFound = false;
        // while (!primeFound)
        // {
        //     BigInteger x = BigIntegerExtensions.GenerateBigIntInRange(0, -1, bitCount);
        //     
        //     if (x == 2 || x == 3)
        //     {
        //         prime = x;
        //         primeFound = true;
        //     }
        //     else 
        //     { 
        //         if (BigInteger.Compare(x, 3) > 0 && 
        //             BigInteger.Remainder(x, 2) != 0 &&
        //             BigInteger.Remainder(x, 3) != 0 &&
        //             BigInteger.Remainder(x, 5) != 0) 
        //         { 
        //             if (x.IsProbablyPrime())
        //             {
        //                 prime = x;
        //                 primeFound = true;
        //             }
        //         } 
        //     }
        // }

        return prime;
    }
}