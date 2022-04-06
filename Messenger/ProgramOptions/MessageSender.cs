using System.Numerics;
using System.Text;
using Newtonsoft.Json;

namespace Messenger.ProgramOptions;

public class MessageSender
{
    private string email;
    private string plaintext;
    private string path;
    
    public MessageSender(string email, string plaintext)
    {
        this.email = email;
        this.plaintext = plaintext;
        this.path = "./" + email + ".key"; 
    }

    public void SendMsg()
    {
        byte[] ptBytes = Encoding.UTF8.GetBytes(this.plaintext);
        BigInteger plaintextInt = new BigInteger(ptBytes);
        
        PublicKey pk = LoadPublicKey(path);
        List<BigInteger> keyValues = ExtractKeyValues(pk);
        
        BigInteger ciphertextInt = EncryptMessage(plaintextInt, keyValues[1], keyValues[3]);
        byte[] ctBytes = ciphertextInt.ToByteArray();

        Message m = new Message(email, Convert.ToBase64String(ctBytes));
        string message = JsonConvert.SerializeObject(m);

        HttpClient client = new HttpClient();
        try
        {
            HttpResponseMessage response = client.PutAsync("http://kayrun.cs.rit.edu:5000/Message/" + email,
                    new StringContent(message, Encoding.UTF8, "application/json")).Result;
            response.EnsureSuccessStatusCode();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

    }
    
    public BigInteger EncryptMessage(BigInteger plaintextInt, BigInteger E, BigInteger N)
    {
        return BigInteger.ModPow(plaintextInt, E, N);
    }
    
    public static PublicKey LoadPublicKey(string path)
    {
        string publicKeystring = File.ReadAllText(path);
        var pubKey = JsonConvert.DeserializeObject<PublicKey>(publicKeystring);
        return pubKey;
    }
    public static List<BigInteger> ExtractKeyValues(PublicKey pk)
    {
        List<BigInteger> list = new List<BigInteger>();
        
        byte[] decodedKey = Convert.FromBase64String(pk.Key);
        
        int e = BitConverter.ToInt32(decodedKey.Take(4).Reverse().ToArray());
        BigInteger E = new BigInteger(decodedKey.Skip(4).Take(e).ToArray());
        int n = BitConverter.ToInt32(decodedKey.Skip(4 + e).Take(4).Reverse().ToArray());
        BigInteger N = new BigInteger(decodedKey.Skip(8 + e).Take(n).ToArray());

        list.Add(e);
        list.Add(E);
        list.Add(n);
        list.Add(N);
        return list;
    }
}