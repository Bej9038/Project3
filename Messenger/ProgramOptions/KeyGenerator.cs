using System.Collections;
using System.Numerics;
using System.Text;
using Newtonsoft.Json;
using PrimeGen;

namespace Messenger.ProgramOptions;

public class KeyGenerator
{
    private int keysize;
    private const int ESize = 16;
    private const double KeysizePercent = .25;
    private const string PublicPath = "./public.key";
    private const string PrivatePath = "./private.key";
    
    public KeyGenerator(string keysize)
    {
        try
        {
            this.keysize = int.Parse(keysize);   
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

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
        ArrayList emails = new ArrayList();
        PrivateKey privateKey = new PrivateKey(emails, AssemblePrivateKeyphrase(rsaN, rsaD));
        SaveLocalKeys(publicKey, privateKey);
    }

    public void SaveLocalKeys(PublicKey publicKey, PrivateKey privateKey)
    {
        FileStream fspub = File.Create(PublicPath);
        FileStream fspriv = File.Create(PrivatePath);
        string pubJson = JsonConvert.SerializeObject(publicKey);
        string privJson = JsonConvert.SerializeObject(privateKey);
        
        fspub.Write(new UTF8Encoding(true).GetBytes(pubJson));
        fspriv.Write(new UTF8Encoding(true).GetBytes(privJson));
        fspub.Close();
        fspriv.Close();
    }

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