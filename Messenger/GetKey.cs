namespace Messenger;

public class GetKey
{
    private string email;

    public GetKey(string email)
    {
        this.email = email;
    }

    public async Task<HttpResponseMessage> getKey()
    {
        HttpClient client = new HttpClient();
        try
        {
            HttpResponseMessage response = await client.GetAsync("http://kayrun.cs.rit.edu:5000/Key/" + email);
            response.EnsureSuccessStatusCode();
            return response;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return null;
    }
}