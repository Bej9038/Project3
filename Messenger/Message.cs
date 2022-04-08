// Author - Benjamin Jordan, bej9038
// File - Message.cs
// Description - Contains the Message class

namespace Messenger;

/// <summary>
/// Represents a Message object
/// </summary>
public class Message
{
    //left the warnings to match the writeup JSON format
    private string _email; 
    private string _content; 

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="email"> the email this message is for </param>
    /// <param name="content"> the message content </param>
    public Message(string email, string content)
    {
        this._email = email;
        this._content = content;
    }

    /// <summary>
    /// Returns or sets the email.
    /// </summary>
    public string email
    {
        get => _email;
        set => _email = value;
    }
    
    /// <summary>
    /// Returns or sets the content
    /// </summary>
    public string content
    {
        get => _content;
        set => _content = value;
    }
}