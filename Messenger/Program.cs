using System.Numerics;
namespace Messenger;

public static class Program
{
    public static void main(String[] args)
    {
        if (args.Length < 2 || args.Length > 3)
        {
            Console.WriteLine("Usage: <option> <option arguments>");
            Environment.Exit(1);
        }

        String option = args[0];
        String arg1 = args[1];
        switch (option)
        {
            case "keyGen":
                KeyGen(arg1);
                break;
            case "sendKey":
                SendKey(arg1);
                break;
            case "getKey":
                GetKey(arg1);
                break;
            case "sendMsg":
                String arg2 = args[2];
                SendMsg(arg1, arg2);
                break;
            case "getMsg":
                GetMsg(arg1);
                break;
        }

    }

    private static void KeyGen(String keysize)
    {
        
    }

    private static void SendKey(String email)
    {
        
    }

    private static void GetKey(String email)
    {
        
    }

    private static void SendMsg(String email, String plaintext)
    {
        
    }

    private static void GetMsg(String email)
    {
        
    }
}