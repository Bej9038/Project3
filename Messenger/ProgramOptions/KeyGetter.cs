using System.Text;

namespace Messenger;

public class KeyGetter
{
    private string email;
    
    public KeyGetter(string email)
    {
        this.email = email;
    }

    public async void GetKey()
    {
        HttpClient client = new HttpClient();
        try
        {
            HttpResponseMessage response = client.GetAsync("http://kayrun.cs.rit.edu:5000/Key/" + email).Result;
            response.EnsureSuccessStatusCode();
            string responseBody = response.Content.ReadAsStringAsync().Result;
            string path = "./" + email + ".key"; 
            FileStream fs = File.Create(path);
            fs.Write(new UTF8Encoding(true).GetBytes(responseBody));
            fs.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}