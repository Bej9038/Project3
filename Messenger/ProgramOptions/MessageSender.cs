using System.Collections;
using System.Numerics;
using System.Text;
using Newtonsoft.Json;

namespace Messenger;

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
        
        PublicKey pk = Program.LoadPublicKey(path);
        List<BigInteger> keyValues = Program.ExtractKeyValues(pk);
        
        BigInteger ciphertextInt = EncryptMessage(plaintextInt, keyValues[1], keyValues[3]);
        byte[] ctBytes = ciphertextInt.ToByteArray();

        Message m = new Message(email, Convert.ToBase64String(ctBytes));
        string message = JsonConvert.SerializeObject(m);

        HttpClient client = new HttpClient();
        try
        {
            HttpResponseMessage response = client.PutAsync("http://kayrun.cs.rit.edu:5000/Message/" + Program.ServerEmail,
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
}