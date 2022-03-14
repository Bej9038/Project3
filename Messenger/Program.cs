using System.Numerics;
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
                KeyGen(arg1);
                break;
            case "sendKey":
                SendKey(arg1);
                break;
            case "getKey":
                GetKey(arg1);
                break;
            case "sendMsg":
                if (len != 3)
                {
                    Console.WriteLine("Usage: sendMsg <email> <plaintext>");
                    Environment.Exit(1);
                }
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

    private static async void GetKey(String email)
    {
        HttpClient client = new HttpClient();
        try
        {
            HttpResponseMessage response = await client.GetAsync("http://kayrun.cs.rit.edu:5000/Key/" + email);
            Console.WriteLine(response.GetType());
            string responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseBody);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
    }

    private static void SendMsg(String email, String plaintext)
    {
        
    }

    private static void GetMsg(String email)
    {
        
    }
    
    public static  BigInteger ModInverse(BigInteger  a,  BigInteger  n) 
    { 
        BigInteger  i  =  n ,  v  =  0,  d  =  1; 
        while  (a > 0)  { 
            BigInteger  t  =  i / a,  x  =  a; 
            a  =  i  %  x; 
            i  =  x; 
            x  =  d; 
            d  =  v  - t * x; 
            v  =  x; 
        } 
        v  %=  n; 
        if  (v < 0)  v  =  ( v + n ) % n; 
        return  v; 
    }
}