using System;
using System.Linq;
using System.Text;

namespace Lab_2
{
    class Program
    {
        static void Main(string[] args)
        {
            var symTbl = new SymbolTable(50);

            

            var symResult = symTbl.Position("a");
            Console.WriteLine(symResult.Equals(symTbl.Position("a")));

            Console.WriteLine(symTbl.ToString());
        }
    }
}
