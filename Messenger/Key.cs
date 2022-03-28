namespace Messenger;

public class Key
{
    private string? email;
    private string keyphrase;

    public Key(string email, string key)
    {
        this.email = email;
        this.keyphrase = key;
    }
    
    public string Keyphrase => keyphrase;
}