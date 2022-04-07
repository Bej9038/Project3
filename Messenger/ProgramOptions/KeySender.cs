// Author - Benjamin Jordan, bej9038
// File - KeySender.cs
// Description - Home to the KeySender class

using System.Text;
using Newtonsoft.Json;
namespace Messenger.ProgramOptions;

/// <summary>
/// KeySender class to instantiate the functionality needed for the sendKey argument.
/// </summary>
public class KeySender
{
    private string email;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="email"> the email of the user the key belongs to </param>
    public KeySender(string email)
    {
        this.email = email;
    }

    /// <summary>
    /// Sends a public key to the server and adds the email to the private key
    /// </summary>
    public void SendKey()
    {
        HttpClient client = new HttpClient();
        try
        {
            //load private key and edit
            string privateKeystring = File.ReadAllText(Program.PrivateKeyPath);
            var privKey = JsonConvert.DeserializeObject<PrivateKey>(privateKeystring);
            if (privKey != null)
            {
                privKey.email.Add(email);
                Program.SavePrivateKey(privKey);
            }
            else
            {
                Console.WriteLine("Error: corresponding private key does not exist");
                Environment.Exit(1);
            }

            //load public key, edit, and send
            string publicKeystring = File.ReadAllText(Program.PublicKeyPath);
            var pubKey = JsonConvert.DeserializeObject<PublicKey>(publicKeystring);
            if (pubKey != null)
            {
                pubKey.email = email;
                string keyWithEmail = JsonConvert.SerializeObject(pubKey);
                HttpResponseMessage response = client.PutAsync("http://kayrun.cs.rit.edu:5000/Key/" + email,
                    new StringContent(keyWithEmail, Encoding.UTF8, "application/json")).Result;
                response.EnsureSuccessStatusCode();
                Console.WriteLine("Key saved");
            }
            else
            {
                Console.WriteLine("Error: no such key");
                Environment.Exit(1);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Error: unable to send key to server");
            Environment.Exit(1);
            Console.WriteLine(e);
        }
    }
}