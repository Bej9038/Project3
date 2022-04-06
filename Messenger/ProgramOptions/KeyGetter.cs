using System.Text;

namespace Messenger;

public class KeyGetter
{
    private string email;
    private string path;
    
    public KeyGetter(string email)
    {
        this.email = email;
        this.path = "./" + email + ".key"; 
    }

    public void GetKey()
    {
        HttpClient client = new HttpClient();
        try
        {
            HttpResponseMessage response = client.GetAsync("http://kayrun.cs.rit.edu:5000/Key/" + email).Result;
            response.EnsureSuccessStatusCode();
            string responseBody = response.Content.ReadAsStringAsync().Result;
            File.WriteAllText(path, responseBody);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}