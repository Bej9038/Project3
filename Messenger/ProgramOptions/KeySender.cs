using System.Text;
using Newtonsoft.Json;

namespace Messenger.ProgramOptions;

public class KeySender
{
    private string email;

    public KeySender(string email)
    {
        this.email = email;
    }

    public void SendKey()
    {
        HttpClient client = new HttpClient();
        try
        {
            string privateKeystring = File.ReadAllText(Program.PrivateKeyPath);
            var privKey = JsonConvert.DeserializeObject<PrivateKey>(privateKeystring);

            if (privKey != null)
            {
                privKey.Emails.Add(email);
                Program.SavePrivateKey(privKey);
            }
            
            string publicKeystring = File.ReadAllText(Program.PublicKeyPath);
            var pubKey = JsonConvert.DeserializeObject<PublicKey>(publicKeystring);
            
            if (pubKey != null)
            {
                pubKey.Email = email;

                string keyWithEmail = JsonConvert.SerializeObject(pubKey);

                HttpResponseMessage response = client.PutAsync("http://kayrun.cs.rit.edu:5000/Key/" + Program.MyEmail,
                    new StringContent(keyWithEmail, Encoding.UTF8, "application/json")).Result;
                response.EnsureSuccessStatusCode();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}