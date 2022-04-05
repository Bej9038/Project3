using System.Collections;
using System.Numerics;
using System.Text;
using Newtonsoft.Json;
using PrimeGen;

namespace Messenger;

public class KeyGenerator
{
    private int keysize;
    private const int ESize = 16;
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
        BigInteger q = generator.RunGenerator(qSize);
        BigInteger N = BigInteger.Multiply(p, q);
        BigInteger r = BigInteger.Multiply(BigInteger.Subtract(p, 1), 
                                            BigInteger.Subtract(q, 1));
        BigInteger E = generator.RunGenerator(ESize);
        BigInteger D = ModInverse(E, r);

        PublicKey publicKey = new PublicKey(AssemblePublicKeyphrase(N, E));
        ArrayList emails = new ArrayList();
        // emails.Add("bej9038@rit.edu");
        PrivateKey privateKey = new PrivateKey(emails, AssemblePrivateKeyphrase(N, D));
        SaveKeys(publicKey, privateKey);

    }

    public void SaveKeys(PublicKey publicKey, PrivateKey privateKey)
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

    public string AssemblePublicKeyphrase(BigInteger N, BigInteger E)
    {
        byte[] e = BitConverter.GetBytes(E.GetByteCount()).Reverse().ToArray();
        byte[] Ebytes = E.ToByteArray();
        byte[] n = BitConverter.GetBytes(N.GetByteCount()).Reverse().ToArray();
        byte[] Nbytes = N.ToByteArray();
        byte[] keyphrase = new byte[e.Length + Ebytes.Length + n.Length + Nbytes.Length];
        
        Array.Copy(e, 0, keyphrase, 0, e.Length);
        Array.Copy(Ebytes, 0,  keyphrase, e.Length, Ebytes.Length);
        Array.Copy(n, 0, keyphrase, e.Length + Ebytes.Length, n.Length);
        Array.Copy(Nbytes, 0, keyphrase, e.Length + Ebytes.Length + n.Length, Nbytes.Length);

        return Convert.ToBase64String(keyphrase);
    }

    public string AssemblePrivateKeyphrase(BigInteger N, BigInteger D)
    {
        byte[] d = BitConverter.GetBytes(D.GetByteCount()).Reverse().ToArray();
        byte[] Dbytes = D.ToByteArray();
        byte[] n = BitConverter.GetBytes(N.GetByteCount()).Reverse().ToArray();
        byte[] Nbytes = N.ToByteArray();
        byte[] keyphrase = new byte[d.Length + Dbytes.Length + n.Length + Nbytes.Length];
        
        Array.Copy(d, 0, keyphrase, 0, d.Length);
        Array.Copy(Dbytes, 0,  keyphrase, d.Length, Dbytes.Length);
        Array.Copy(n, 0, keyphrase, d.Length + Dbytes.Length, n.Length);
        Array.Copy(Nbytes, 0, keyphrase, d.Length + Dbytes.Length + n.Length, Nbytes.Length);

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

    public string ReverseSting(string s)
    {
        char[] charArray = s.ToCharArray();
        Array.Reverse( charArray );
        return new string( charArray );
    }

    private string AddKeysizePadding(string num)
    {
        while (num.Length < 4)
        {
            num += "0";
        }

        return num;
    }

}