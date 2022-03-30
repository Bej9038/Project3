using System.Numerics;
using System.Text;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Messenger;

public static class Program
{
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
        
        string path = "./jsb@cs.rit.edu.key";
        string keyObj = File.ReadAllText(path);
        var key = JsonConvert.DeserializeObject<Key>(keyObj);
        byte[] decodedKey = Convert.FromBase64String(key.Keyphrase);
        
        var e = BitConverter.ToInt32(decodedKey.Take(4).Reverse().ToArray());
        var E = new BigInteger(decodedKey.Skip(4).Take(e).ToArray());
        var n = BitConverter.ToInt32(decodedKey.Skip(4 + e).Take(4).Reverse().ToArray());
        var N = new BigInteger(decodedKey.Skip(8 + e).Take(n).ToArray());
        Console.WriteLine(e);
        Console.WriteLine(E);
        Console.WriteLine(n);
        Console.WriteLine(N);
    }
}