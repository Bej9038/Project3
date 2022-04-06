using System.Text;
using Messenger.ProgramOptions;
using Newtonsoft.Json;

namespace Messenger;

public static class Program
{
    public const string ServerEmail = "jsb@cs.rit.edu";
    public const string MyEmail = "bej9038@rit.edu";
    public const string PublicKeyPath = "./public.key";
    public const string PrivateKeyPath = "./private.key";

    public static void Main(string[] args)
    {
        int len = args.Length;
        if (len < 2 || len > 3)
        {
            Console.WriteLine("Usage: <option> <option arguments>");
            Environment.Exit(1);
        }

        String option = args[0];
        String arg1 = args[1];
        switch (option)
        {
            case "keyGen":
                KeyGenerator kgen = new KeyGenerator(arg1);
                kgen.KeyGen();
                break;
            case "sendKey":
                KeySender ksend = new KeySender(arg1);
                ksend.SendKey();
                break;
            case "getKey":
                KeyGetter kgtr = new KeyGetter(arg1);
                kgtr.GetKey();
                break;
            case "sendMsg":
                if (len != 3)
                {
                    Console.WriteLine("Usage: sendMsg <email> <plaintext>");
                    Environment.Exit(1);
                }

                String arg2 = args[2];
                MessageSender msend = new MessageSender(arg1, arg2);
                msend.SendMsg();
                break;
            case "getMsg":
                MessageGetter mget = new MessageGetter(arg1);
                mget.GetMsg();
                break;
        }
    }
    
    public static void SavePublicKey(PublicKey publicKey)
    {
        FileStream fspub = File.Create(PublicKeyPath);
        string pubJson = JsonConvert.SerializeObject(publicKey);
        fspub.Write(new UTF8Encoding(true).GetBytes(pubJson));
        fspub.Close();
    }
    
    public static void SavePrivateKey(PrivateKey privateKey)
    {
        FileStream fspriv = File.Create(PrivateKeyPath);
        string privJson = JsonConvert.SerializeObject(privateKey);
        fspriv.Write(new UTF8Encoding(true).GetBytes(privJson));
        fspriv.Close();
    }
}