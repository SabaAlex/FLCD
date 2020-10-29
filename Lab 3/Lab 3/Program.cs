using System;

namespace Lab_3
{

    public class Program
    {
        static void Main(string[] args)
        {  
            var scanner = new Scanner();

            scanner.LoadTokens("tokens.in");

            scanner.Scanning(@"C:\Personal\Limbaje formale si tehnici de compilare\Lab 1\p1err.txt");
        }
    }
}
