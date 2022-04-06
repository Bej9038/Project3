using System.Text;
using Newtonsoft.Json;

namespace Messenger;

public class KeySender
{
    private string email;
    private string path;

    public KeySender(string email)
    {
        this.email = email;
        this.path = "./public.key";
    }

    public void SendKey()
    {
        HttpClient client = new HttpClient();
        try
        {
            string publicKeystring = File.ReadAllText(path);
            PublicKey? pubKey = JsonConvert.DeserializeObject<PublicKey>(publicKeystring);
            if (pubKey != null)
            {
                pubKey.Email = Program.MyEmail;
                string keyWithEmail = JsonConvert.SerializeObject(pubKey);

                HttpResponseMessage response = client.PutAsync("http://kayrun.cs.rit.edu:5000/Key/" + Program.ServerEmail,
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