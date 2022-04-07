// Author - Benjamin Jordan, bej9038
// File - Key.cs
// Description - Contains the PublicKey and PrivateKey classes

namespace Messenger;

/// <summary>
/// Represents a PublicKey object
/// </summary>
public class PublicKey
{
    //left the warnings to match the writeup JSON format
    private string email; 
    private string key;
    
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="key"> the keyphrase </param>
    public PublicKey(string key)
    {
        this.key = key;
        this.email = "";
    }

    /// <summary>
    /// Returns or sets the email
    /// </summary>
    public string Email
    {
        get => email;
        set => email = value;
    }

    /// <summary>
    /// Returns or sets the keyphrase
    /// </summary>
    public string Key
    {
        get => key;
        set => key = value;
    }
}

/// <summary>
/// Represents a PrivateKey object
/// </summary>
public class PrivateKey
{
    //left the warnings to match the writeup JSON format
    private List<string> email;
    private string key;
    
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="key"> the keyphrase </param>
    public PrivateKey(string key)
    {
        this.key = key;
        this.email = new List<string>();
    }
    
    /// <summary>
    /// Returns or sets the email
    /// </summary>
    public List<string> Emails
    {
        get => email;
        set => email = value;
    }
    
    /// <summary>
    /// Returns or sets the keyphrase
    /// </summary>
    public string Key
    {
        get => key;
        set => key = value;
    }
}