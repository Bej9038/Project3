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
    private string email; 
    private string content; 

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="email"> the email this message is for </param>
    /// <param name="content"> the message content </param>
    public Message(string email, string content)
    {
        this.email = email;
        this.content = content;
    }

    /// <summary>
    /// Returns or sets the email.
    /// </summary>
    public string Email
    {
        get => email;
        set => email = value;
    }
    
    /// <summary>
    /// Returns or sets the content
    /// </summary>
    public string Content
    {
        get => content;
        set => content = value;
    }
}