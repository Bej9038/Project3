using System.Collections;

namespace Messenger;

public class PublicKey
{
    private string? email;
    private string key;
    public PublicKey(string key)
    {
        this.key = key;
    }

    public string Email
    {
        get => email;
        set => email = value;
    }
    public string Key => key;
}

public class PrivateKey
{
    private ArrayList emails;
    private string key;
    
    public PrivateKey(ArrayList emails, string key)
    {
        this.emails = emails;
        this.key = key;
    }
    
    public string Key => key;
}