using System.Numerics;
using Newtonsoft.Json;

namespace Messenger;

public class MessageGetter
{
    private string email;
    private string path;
    public MessageGetter(string email)
    {
        this.email = email;
        this.path = "./" + email + ".key"; 
    }

    public void GetMsg()
    {
        HttpClient client = new HttpClient();
        PrivateKey pk = LoadPrivateKey("private.key");
        List<BigInteger> keyValues = ExtractKeyValues(pk);
        
        try
        {
            HttpResponseMessage response = client.GetAsync("http://kayrun.cs.rit.edu:5000/Message/" + email).Result;
            response.EnsureSuccessStatusCode();
            string responseBody = response.Content.ReadAsStringAsync().Result;
            var message = JsonConvert.DeserializeObject<Message>(responseBody);

            byte[] content = Convert.FromBase64String(message.Content);
            BigInteger cipertextInt = new BigInteger(content);
            BigInteger plaintextInt = DecryptMessage(cipertextInt, keyValues[1], keyValues[3]);
            Console.WriteLine(Convert.ToBase64String(plaintextInt.ToByteArray()));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
    
    public static PrivateKey LoadPrivateKey(string path)
    {
        string privateKeystring = File.ReadAllText(path);
        var privKey = JsonConvert.DeserializeObject<PrivateKey>(privateKeystring);
        return privKey;
    }
    
    public BigInteger DecryptMessage(BigInteger ciphertextInt, BigInteger D, BigInteger N)
    {
        return BigInteger.ModPow(ciphertextInt, D, N);
    }
    
    public List<BigInteger> ExtractKeyValues(PrivateKey pk)
    {
        List<BigInteger> list = new List<BigInteger>();
        
        byte[] decodedKey = Convert.FromBase64String(pk.Key);
        
        int d = BitConverter.ToInt32(decodedKey.Take(4).Reverse().ToArray());
        BigInteger D = new BigInteger(decodedKey.Skip(4).Take(d).ToArray());
        int n = BitConverter.ToInt32(decodedKey.Skip(4 + d).Take(4).Reverse().ToArray());
        BigInteger N = new BigInteger(decodedKey.Skip(8 + d).Take(n).ToArray());

        list.Add(d);
        list.Add(D);
        list.Add(n);
        list.Add(N);
        return list;
    }
}