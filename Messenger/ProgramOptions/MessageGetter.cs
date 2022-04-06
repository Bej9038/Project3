using System.Numerics;
using Newtonsoft.Json;

namespace Messenger.ProgramOptions;

public class MessageGetter
{
    private string email;
    public MessageGetter(string email)
    {
        this.email = email;
    }

    public void GetMsg()
    {
        HttpClient client = new HttpClient();
        PrivateKey pk = LoadPrivateKey(Program.PrivateKeyPath);
        List<BigInteger> keyValues = ExtractKeyValues(pk);
        
        try
        {
            HttpResponseMessage response = client.GetAsync("http://kayrun.cs.rit.edu:5000/Message/" + email).Result;
            response.EnsureSuccessStatusCode();
            string responseBody = response.Content.ReadAsStringAsync().Result;
            var message = JsonConvert.DeserializeObject<Message>(responseBody);

            if (message != null)
            {
                byte[] content = Convert.FromBase64String(message.Content);
                BigInteger cipertextInt = new BigInteger(content);
                BigInteger plaintextInt = DecryptMessage(cipertextInt, keyValues[1], keyValues[3]);
                Console.WriteLine(Convert.ToBase64String(plaintextInt.ToByteArray()));   
            }
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
    
    private BigInteger DecryptMessage(BigInteger ciphertextInt, BigInteger rsaD, BigInteger rsaN)
    {
        return BigInteger.ModPow(ciphertextInt, rsaD, rsaN);
    }
    
    private List<BigInteger> ExtractKeyValues(PrivateKey pk)
    {
        List<BigInteger> list = new List<BigInteger>();
        
        byte[] decodedKey = Convert.FromBase64String(pk.Key);
        
        int d = BitConverter.ToInt32(decodedKey.Take(4).Reverse().ToArray());
        BigInteger rsaD = new BigInteger(decodedKey.Skip(4).Take(d).ToArray());
        int n = BitConverter.ToInt32(decodedKey.Skip(4 + d).Take(4).Reverse().ToArray());
        BigInteger rsaN = new BigInteger(decodedKey.Skip(8 + d).Take(n).ToArray());

        list.Add(d);
        list.Add(rsaD);
        list.Add(n);
        list.Add(rsaN);
        return list;
    }
}