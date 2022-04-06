

namespace Messenger;

public class PublicKey
{
    //left the warnings to match the writeup JSON format
    private string email; 
    private string key;
    public PublicKey(string key)
    {
        this.key = key;
        this.email = "";
    }

    public string Email
    {
        get => email;
        set => email = value ?? throw new ArgumentNullException(nameof(value));
    }

    public string Key
    {
        get => key;
        set => key = value ?? throw new ArgumentNullException(nameof(value));
    }
}

public class PrivateKey
{
    //left the warnings to match the writeup JSON format
    private List<string> emails;
    private string key;
    
    public PrivateKey(List<string> emails, string key)
    {
        this.emails = emails;
        this.key = key;
    }
    
    public List<string> Emails
    {
        get => emails;
        set => emails = value ?? throw new ArgumentNullException(nameof(value));
    }
    
    public string Key
    {
        get => key;
        set => key = value ?? throw new ArgumentNullException(nameof(value));
    }
}