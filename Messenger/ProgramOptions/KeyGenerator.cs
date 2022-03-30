using System.Numerics;
using PrimeGen;

namespace Messenger;

public class KeyGenerator
{
    private int keysize;
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
        Random rnd = new Random();
        int plusOrMinus = rnd.Next(0, 2);
        double percentage = .25;
        int pSize;
        if (plusOrMinus == 0)
        {
            pSize = keysize / 2 + (int) (keysize * percentage);
        }
        else
        {
            pSize = keysize / 2 - (int) (keysize * percentage);
        }
        int qSize = keysize - pSize;
        
        Generator generator = new Generator();
        BigInteger p = generator.RunGenerator(pSize);
        byte[] pBytes = p.ToByteArray();
        for(int i = 0; i < pBytes.Length; i++)
        {
            int value = pBytes[i];
            if (value % 2 == 0)
            {
                pBytes[i] = (byte) (value + 1);
            }
        }
        p = new BigInteger(pBytes);

        BigInteger q = generator.RunGenerator(qSize);
        byte[] qBytes = q.ToByteArray();
        for(int i = 0; i < qBytes.Length; i++)
        {
            int value = qBytes[i];
            if (value % 2 == 0)
            {
                qBytes[i] = (byte) (value + 1);
            }
        }
        q = new BigInteger(qBytes);

        BigInteger N = BigInteger.Multiply(p, q);
        BigInteger r = BigInteger.Multiply(BigInteger.Subtract(p, 1), 
                                            BigInteger.Subtract(q, 1));
        BigInteger E = generator.RunGenerator(16);
        BigInteger D = ModInverse(E, r);
        
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