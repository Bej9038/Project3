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
        switch (option)
        {
            case "keyGen":
                break;
            case "sendKey":
                break;
            case "getKey":
                break;
            case "sendMsg":
                break;
            case "getMsg":
                break;
        }

    }
    

    public static BigInteger ModInverse(BigInteger  a,  BigInteger  n) 
    { 
        BigInteger  i  =  n,  v  =  0,  d  =  1; 
        while  (a>0)  { 
            BigInteger  t  =  i/a,  x  =  a; 
            a  =  i  %  x; 
            i  =  x; 
            x  =  d; 
            d  =  v  - t*x; 
            v  =  x; 
        } 
        v  %=  n; 
        if  (v<0)  v  =  (v+n)%n; 
        return  v; 
    }
}