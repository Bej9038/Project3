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
        //CONSTRUCT BYTE ARRAY FIRST THE ENCODE!!!
        
        
        string keyphrase = "";
        string e = ReverseSting(AddKeysizePadding(E.GetByteCount().ToString()));
        string n = ReverseSting(AddKeysizePadding(N.GetByteCount().ToString()));
        keyphrase += e;
        keyphrase += Convert.ToBase64String(E.ToByteArray());
        keyphrase += n;
        keyphrase += Convert.ToBase64String(N.ToByteArray());

        return keyphrase;
    }

    public string AssemblePrivateKeyphrase(BigInteger N, BigInteger D)
    {
        string keyphrase = "";
        string d = ReverseSting(AddKeysizePadding(D.GetByteCount().ToString()));
        string n = ReverseSting(AddKeysizePadding(N.GetByteCount().ToString()));
        keyphrase += d;
        keyphrase += Convert.ToBase64String(D.ToByteArray());
        keyphrase += n;
        keyphrase += Convert.ToBase64String(N.ToByteArray());

        return keyphrase;
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