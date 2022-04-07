// Author - Benjamin Jordan, bej9038
// File - KeyGetter.cs
// Description - Home to the KeyGetter class

namespace Messenger.ProgramOptions;

/// <summary>
/// KeyGetter class to instantiate the functionality needed for the getKey argument.
/// </summary>
public class KeyGetter
{
    private string email;
    private string path;
    
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="email"> the email to get the key of </param>
    public KeyGetter(string email)
    {
        this.email = email;
        this.path = "./" + email + ".key"; 
    }

    /// <summary>
    /// Gets the key from the user specified by the email
    /// </summary>
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