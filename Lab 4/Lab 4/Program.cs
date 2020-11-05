using System;

namespace Lab_4
{
    class Program
    {
        static void Main(string[] args)
        {
            var ui = new UI()
            {
                FA = new FA(@"C:\Personal\Limbaje formale si tehnici de compilare\Lab 4\Lab 4\FA.in"),
            };

            ui.Start();
        }


    }
}
