// Author - Benjamin Jordan, bej9038
// File - Key.cs
// Description - Contains the PublicKey and PrivateKey classes

using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Messenger;

/// <summary>
/// Represents a PublicKey object
/// </summary>
public class PublicKey
{
    //left the warnings to match the writeup JSON format
    private string _email; 
    private string _key;
    
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="key"> the keyphrase </param>
    public PublicKey(string key)
    {
        this._key = key;
        this._email = "";
    }

    /// <summary>
    /// Returns or sets the email
    /// </summary>
    public string email
    {
        get => _email;
        set => _email = value;
    }

    /// <summary>
    /// Returns or sets the keyphrase
    /// </summary>
    public string key
    {
        get => _key;
        set => _key = value;
    }
}

/// <summary>
/// Represents a PrivateKey object
/// </summary>
public class PrivateKey
{
    //left the warnings to match the writeup JSON format
    private List<string> _email;
    private string _key;
    
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="key"> the keyphrase </param>
    public PrivateKey(string key)
    {
        this._key = key;
        this._email = new List<string>();
    }
    
    /// <summary>
    /// Returns or sets the email
    /// </summary>
    public List<string> email
    {
        get => _email;
        set => _email = value;
    }
    
    /// <summary>
    /// Returns or sets the keyphrase
    /// </summary>
    public string key
    {
        get => _key;
        set => _key = value;
    }
}