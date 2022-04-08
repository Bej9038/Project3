// Author - Benjamin Jordan, bej9038
// File - MessageGetter.cs
// Description - Home to the MessageGetter class

using System.Numerics;
using System.Text;
using Newtonsoft.Json;
namespace Messenger.ProgramOptions;

/// <summary>
/// MessageGetter class to instantiate the functionality needed for the getMsg argument.
/// </summary>
public class MessageGetter
{
    private string email;
    
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="email"> the user to get the message for </param>
    public MessageGetter(string email)
    {
        this.email = email;
    }

    /// <summary>
    /// Gets the message from the server
    /// </summary>
    public void GetMsg()
    {
        HttpClient client = new HttpClient();
        try
        {
            PrivateKey pk = LoadPrivateKey(Program.PrivateKeyPath);
            if (pk.key.Equals("nullkey"))
            {
                Console.WriteLine("Error: key for " + email + " is not valid");
                Environment.Exit(1);
            }
            List<BigInteger> keyValues = ExtractKeyValues(pk);
        
            try
            {
                HttpResponseMessage response = client.GetAsync("http://kayrun.cs.rit.edu:5000/Message/" + email).Result;
                response.EnsureSuccessStatusCode();
                string responseBody = response.Content.ReadAsStringAsync().Result;
                var message = JsonConvert.DeserializeObject<Message>(responseBody);
                if (message != null)
                {
                    byte[] content = Convert.FromBase64String(message.content);
                    BigInteger ciphertextInt = new BigInteger(content);
                    BigInteger plaintextInt = DecryptMessage(ciphertextInt, keyValues[0], keyValues[1]);
                    Console.WriteLine(Encoding.UTF8.GetString(plaintextInt.ToByteArray()));   
                }
                else
                {
                    Console.WriteLine("Error: invalid message content");
                    Environment.Exit(1);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: Unable to get message");
                Environment.Exit(1);
                Console.WriteLine(e);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Error: Key does not exist for " + email);
            Console.WriteLine("Download key using: getKey <email>");
            Environment.Exit(1);
            Console.WriteLine(e);
        }
        
    }
    
    /// <summary>
    /// Loads the locally saved private key object
    /// </summary>
    /// <param name="path"> The path of the private key</param>
    /// <returns> The PrivateKey object </returns>
    public static PrivateKey LoadPrivateKey(string path)
    {
        string privateKeystring = File.ReadAllText(path);
        var privKey = JsonConvert.DeserializeObject<PrivateKey>(privateKeystring);
        if (privKey != null)
        {
            return privKey;   
        }
        return new PrivateKey("nullkey");
    }
    
    /// <summary>
    /// Decrypts a ciphertext encrypted with the corresponding public key
    /// </summary>
    /// <param name="ciphertextInt"> the ciphertext </param>
    /// <param name="rsaD"> the D value from the private keyphrase </param>
    /// <param name="rsaN"> the N value from the private keyphrase </param>
    /// <returns> The decrypted message as a BigInteger </returns>
    public static BigInteger DecryptMessage(BigInteger ciphertextInt, BigInteger rsaD, BigInteger rsaN)
    {
        return BigInteger.ModPow(ciphertextInt, rsaD, rsaN);
    }
    
    /// <summary>
    /// Extracts the RSA values from a private key
    /// </summary>
    /// <param name="pk"> the private key </param>
    /// <returns> A lsit of the rsa values </returns>
    public static List<BigInteger> ExtractKeyValues(PrivateKey pk)
    {
        List<BigInteger> list = new List<BigInteger>();
        
        byte[] decodedKey = Convert.FromBase64String(pk.key);
        
        int d = BitConverter.ToInt32(decodedKey.Take(4).Reverse().ToArray());
        BigInteger rsaD = new BigInteger(decodedKey.Skip(4).Take(d).ToArray());
        int n = BitConverter.ToInt32(decodedKey.Skip(4 + d).Take(4).Reverse().ToArray());
        BigInteger rsaN = new BigInteger(decodedKey.Skip(8 + d).Take(n).ToArray());
        
        list.Add(rsaD);
        list.Add(rsaN);
        return list;
    }
}