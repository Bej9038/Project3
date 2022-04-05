using System.Collections;

namespace Messenger;

public class PublicKey
{
    private string? email;
    private string keyphrase;

    public PublicKey(string email, string key)
    {
        this.email = email;
        this.keyphrase = key;
    }
    public PublicKey(string key)
    {
        this.email = email;
        this.keyphrase = key;
    }
    public string Keyphrase => keyphrase;
}

public class PrivateKey
{
    private ArrayList emails;
    private string keyphrase;
    
    public PrivateKey(ArrayList emails, string key)
    {
        this.emails = emails;
        this.keyphrase = key;
    }
    
    public string Keyphrase => keyphrase;
}