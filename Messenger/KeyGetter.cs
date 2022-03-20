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
            string responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseBody);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}