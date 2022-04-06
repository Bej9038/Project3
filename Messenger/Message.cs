namespace Messenger;

public class Message
{
    //left the warnings to match the writeup JSON format
    private string email; 
    private string content; 

    public Message(string email, string content)
    {
        this.email = email;
        this.content = content;
    }

    public string Email
    {
        get => email;
        set => email = value ?? throw new ArgumentNullException(nameof(value));
    }
    
    public string Content
    {
        get => content;
        set => content = value ?? throw new ArgumentNullException(nameof(value));
    }
}