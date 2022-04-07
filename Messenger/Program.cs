// Author - Benjamin Jordan, bej9038
// File - Program.cs
// Description - Implements a secure messaging system using RSA public key encryption.

using System.Text;
using Messenger.ProgramOptions;
using Newtonsoft.Json;
namespace Messenger;

/// <summary>
/// Starting point of secure messaging program
/// </summary>
public static class Program
{
    public const string ServerEmail = "jsb@cs.rit.edu";
    public const string MyEmail = "bej9038@rit.edu";
    public const string PublicKeyPath = "./public.key";
    public const string PrivateKeyPath = "./private.key";

    /// <summary>
    /// Main. Handles program arguments and initializes the required program option
    /// </summary>
    /// <param name="args"> program arguments </param>
    public static void Main(string[] args)
    {
        int numArgs = args.Length;
        if (args.Length == 2 || args.Length == 3)
        {
                String option = args[0];
                String arg1 = args[1];
                switch (option)
                 {
                     case "keyGen":
                         if (numArgs != 2)
                         {
                             Console.WriteLine("Usage: keyGen <keysize>");
                             Environment.Exit(1);
                         }
                         KeyGenerator kgen = new KeyGenerator(arg1);
                         kgen.KeyGen();
                         break;
                     case "sendKey":
                         if (numArgs != 2)
                         {
                             Console.WriteLine("Usage: sendKey <email>");
                             Environment.Exit(1);
                         }
                         KeySender ksend = new KeySender(arg1);
                         ksend.SendKey();
                         break;
                     case "getKey":
                         if (numArgs != 2)
                         {
                             Console.WriteLine("Usage: getKey <email>");
                             Environment.Exit(1);
                         }
                         KeyGetter kgtr = new KeyGetter(arg1);
                         kgtr.GetKey();
                         break;
                     case "sendMsg":
                         if (numArgs != 3)
                         {
                             Console.WriteLine("Usage: sendMsg <email> <plaintext>");
                             Environment.Exit(1);
                         }
                         String arg2 = args[2];
                         MessageSender msend = new MessageSender(arg1, arg2);
                         msend.SendMsg();
                         break;
                     case "getMsg":
                         if (numArgs != 2)
                         {
                             Console.WriteLine("Usage: getMsg <email>");
                             Environment.Exit(1);
                         }
                         MessageGetter mget = new MessageGetter(arg1);
                         mget.GetMsg();
                         break;
                     default:
                         Console.WriteLine("Usage: <option> <option arguments>");
                         Environment.Exit(1);
                         break;
                 }
        }
        else
        {
            Console.WriteLine("Usage: <option> <option arguments>");
            Environment.Exit(1);
        }
    }
    
    /// <summary>
    /// Saves a public key to the current directory as a .key file
    /// </summary>
    /// <param name="publicKey"> The key to save</param>
    public static void SavePublicKey(PublicKey publicKey)
    {
        FileStream fspub = File.Create(PublicKeyPath);
        string pubJson = JsonConvert.SerializeObject(publicKey);
        fspub.Write(new UTF8Encoding(true).GetBytes(pubJson));
        fspub.Close();
    }
    
    /// <summary>
    /// Saves a private key to the current directory as a .key file
    /// </summary>
    /// <param name="privateKey"> The key to save</param>
    public static void SavePrivateKey(PrivateKey privateKey)
    {
        FileStream fspriv = File.Create(PrivateKeyPath);
        string privJson = JsonConvert.SerializeObject(privateKey);
        fspriv.Write(new UTF8Encoding(true).GetBytes(privJson));
        fspriv.Close();
    }
}