// Author - Benjamin Jordan, bej9038
// File - MessageSender.cs
// Description - Home to the MessageSender class

using System.Numerics;
using System.Text;
using Newtonsoft.Json;
namespace Messenger.ProgramOptions;

/// <summary>
/// MessageSender class to instantiate the functionality needed for the sendMsg argument.
/// </summary>
public class MessageSender
{
    private string email;
    private string plaintext;
    private string path;
    
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="email"> the user to send the message to </param>
    /// <param name="plaintext"> the message to send </param>
    public MessageSender(string email, string plaintext)
    {
        this.email = email;
        this.plaintext = plaintext;
        this.path = "./" + email + ".key"; 
    }

    /// <summary>
    /// Sends a message to the server
    /// </summary>
    public void SendMsg()
    {
        byte[] ptBytes = Encoding.UTF8.GetBytes(this.plaintext);
        BigInteger plaintextInt = new BigInteger(ptBytes);
        
        PublicKey pk = LoadPublicKey(path);
        List<BigInteger> keyValues = ExtractKeyValues(pk);
        
        BigInteger ciphertextInt = EncryptMessage(plaintextInt, keyValues[0], keyValues[1]);
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
    
    /// <summary>
    /// Encrypts a plaintext message using RSA encryption
    /// </summary>
    /// <param name="plaintextInt"> the plaintext </param>
    /// <param name="rsaE"> the RSA E value </param>
    /// <param name="rsaN"> the RSA N value</param>
    /// <returns></returns>
    private BigInteger EncryptMessage(BigInteger plaintextInt, BigInteger rsaE, BigInteger rsaN)
    {
        return BigInteger.ModPow(plaintextInt, rsaE, rsaN);
    }
    
    /// <summary>
    /// Loads a public key from a .key file
    /// </summary>
    /// <param name="path"> the key file path </param>
    /// <returns> the PublicKey object</returns>
    public static PublicKey LoadPublicKey(string path)
    {
        string publicKeystring = File.ReadAllText(path);
        var pubKey = JsonConvert.DeserializeObject<PublicKey>(publicKeystring);
        return pubKey;
    }
    
    /// <summary>
    /// Extracts the RSA values from a PublicKey object
    /// </summary>
    /// <param name="pk"> the public key </param>
    /// <returns> a list of the RSA values needed for enncryption </returns>
    private static List<BigInteger> ExtractKeyValues(PublicKey pk)
    {
        List<BigInteger> list = new List<BigInteger>();
        
        byte[] decodedKey = Convert.FromBase64String(pk.key);
        
        int e = BitConverter.ToInt32(decodedKey.Take(4).Reverse().ToArray());
        BigInteger rsaE = new BigInteger(decodedKey.Skip(4).Take(e).ToArray());
        int n = BitConverter.ToInt32(decodedKey.Skip(4 + e).Take(4).Reverse().ToArray());
        BigInteger rsaN = new BigInteger(decodedKey.Skip(8 + e).Take(n).ToArray());
        
        list.Add(rsaE);
        list.Add(rsaN);
        return list;
    }
}