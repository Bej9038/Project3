// Author - Benjamin Jordan, bej9038
// File - KeyGenerator.cs
// Description - Home to the KeyGenerator class

using System.Numerics;
using Messenger.PrimeGen;
namespace Messenger.ProgramOptions;

/// <summary>
/// KeyGenerator class to instantiate the functionality needed for the keyGen argument.
/// </summary>
public class KeyGenerator
{
    private int keysize;
    private const int ESize = 16;
    private const double KeysizePercent = .25;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="keysize"> the size of the key to generate </param>
    public KeyGenerator(string keysize)
    {
        try
        {
            this.keysize = int.Parse(keysize);   
        }
        catch (Exception e)
        {
            Console.WriteLine("Usage: keyGen <keysize>");
            Console.WriteLine("Error, keysize must be of type int");
            Environment.Exit(1);
        }
    }

    /// <summary>
    /// Generates the key pair and saves it to the current directory
    /// </summary>
    public void KeyGen()
    {
        int pSize = GetPSize();
        int qSize = keysize - pSize;
        
        Generator generator = new Generator();
        BigInteger p = generator.RunGenerator(pSize);
        BigInteger q = generator.RunGenerator(qSize);
        
        BigInteger rsaN = BigInteger.Multiply(p, q);
        BigInteger r = BigInteger.Multiply(BigInteger.Subtract(p, 1), 
                                            BigInteger.Subtract(q, 1));
        BigInteger rsaE = generator.RunGenerator(ESize);
        BigInteger rsaD = ModInverse(rsaE, r);

        PublicKey publicKey = new PublicKey(AssemblePublicKeyphrase(rsaN, rsaE));
        PrivateKey privateKey = new PrivateKey(AssemblePrivateKeyphrase(rsaN, rsaD));
        Program.SavePublicKey(publicKey);
        Program.SavePrivateKey(privateKey);
    }
    
    /// <summary>
    /// Chooses the size of P for RSA
    /// </summary>
    /// <returns> the size of P </returns>
    public int GetPSize()
    {
        int pSize;
        Random rnd = new Random();
        int plusOrMinus = rnd.Next(0, 2);
        double percentage = KeysizePercent;
        if (plusOrMinus == 0)
        {
            pSize = keysize / 2 + (int) (keysize * percentage);
        }
        else
        {
            pSize = keysize / 2 - (int) (keysize * percentage);
        }
        return pSize;
    }

    /// <summary>
    /// Organizes the RSA values into a valid public keyphrase for the server
    /// </summary>
    /// <param name="rsaN"> the N value for RSA </param>
    /// <param name="rsaE">the E valkue for RSA</param>
    /// <returns> the keyphrase </returns>
    public string AssemblePublicKeyphrase(BigInteger rsaN, BigInteger rsaE)
    {
        byte[] e = BitConverter.GetBytes(rsaE.GetByteCount()).Reverse().ToArray();
        byte[] eBytes = rsaE.ToByteArray();
        byte[] n = BitConverter.GetBytes(rsaN.GetByteCount()).Reverse().ToArray();
        byte[] nBytes = rsaN.ToByteArray();
        byte[] keyphrase = new byte[e.Length + eBytes.Length + n.Length + nBytes.Length];
        
        Array.Copy(e, 0, keyphrase, 0, e.Length);
        Array.Copy(eBytes, 0,  keyphrase, e.Length, eBytes.Length);
        Array.Copy(n, 0, keyphrase, e.Length + eBytes.Length, n.Length);
        Array.Copy(nBytes, 0, keyphrase, e.Length + eBytes.Length + n.Length, nBytes.Length);

        return Convert.ToBase64String(keyphrase);
    }

    /// <summary>
    /// Organizes the RSA values into a valid private keyphrase for the server
    /// </summary>
    /// <param name="rsaN"> the N value for RSA </param>
    /// <param name="rsaD">the D value for RSA</param>
    /// <returns> the keyphrase </returns>
    public string AssemblePrivateKeyphrase(BigInteger rsaN, BigInteger rsaD)
    {
        byte[] d = BitConverter.GetBytes(rsaD.GetByteCount()).Reverse().ToArray();
        byte[] dBytes = rsaD.ToByteArray();
        byte[] n = BitConverter.GetBytes(rsaN.GetByteCount()).Reverse().ToArray();
        byte[] nbytes = rsaN.ToByteArray();
        byte[] keyphrase = new byte[d.Length + dBytes.Length + n.Length + nbytes.Length];
        
        Array.Copy(d, 0, keyphrase, 0, d.Length);
        Array.Copy(dBytes, 0,  keyphrase, d.Length, dBytes.Length);
        Array.Copy(n, 0, keyphrase, d.Length + dBytes.Length, n.Length);
        Array.Copy(nbytes, 0, keyphrase, d.Length + dBytes.Length + n.Length, nbytes.Length);

        return Convert.ToBase64String(keyphrase);
    }

    /// <summary>
    /// Modular inverse function of BigInteger a mod n
    /// </summary>
    /// <param name="a"> the BigInteger to the find inverse of </param>
    /// <param name="n"> the mod value </param>
    /// <returns> the mod inverse of a mod n </returns>
    public BigInteger ModInverse(BigInteger  a,  BigInteger  n) 
    { 
        BigInteger  i  =  n ,  v  =  0,  d  =  1; 
        while  (a > 0)  { 
            BigInteger  t  =  i / a,  x  =  a; 
            a  =  i  %  x; 
            i  =  x; 
            x  =  d; 
            d  =  v  - t * x; 
            v  =  x; 
        } 
        v  %=  n; 
        if  (v < 0)  v  =  ( v + n ) % n; 
        return  v; 
    }
}