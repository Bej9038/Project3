using System.Numerics;
namespace PrimeGen;


/// <summary>
/// Title: Generator
/// Description: generator object used to generate large prime integers
/// Author: Ben Jordan
/// 
/// </summary>
public class Generator
{
    private const uint LoopIterations =  4294967295; // number of loop iterations
    private static Object _myLock = new Object(); // lock object

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="bitCount"> Integer bit length </param>
    public Generator()
    {
    }
    
    /// <summary>
    /// Runs the generator.
    /// </summary>
    public BigInteger RunGenerator(int bitCount)
    {
        BigInteger prime = new BigInteger();
        int byteCount = BitsToBytes(bitCount);
        
        Parallel.For(0, LoopIterations, (i, state) => {
            BigInteger x = BigIntegerExtensions.GenerateBigIntInRange(0, -1, byteCount);
            
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

    /// <summary>
    /// Converts a value in bits to the equivalent value in bytes.
    /// </summary>
    /// <param name="bits"> The number of bits </param>
    /// <returns> The number of bytes </returns>
    public int BitsToBytes(int bits)
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