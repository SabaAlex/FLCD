using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading;

namespace Lab_4
{
    public class UI
    {
        public FA FA { get; set; }

        public void PrintMenu()
        {
            Console.WriteLine("0. Exit");
            Console.WriteLine("1. Print set of states");
            Console.WriteLine("2. Print alphabet");
            Console.WriteLine("3. Print transitions");
            Console.WriteLine("4. Print initial state");
            Console.WriteLine("5. Print final states");
            Console.WriteLine("6. Check if a sequence is accepted by the FA");
        }

        public void PrintString(string toPrint)
        {
            Console.WriteLine(toPrint);
        }



        public void Start()
        {
            FA.ReadFA();

            PrintMenu();

            var keepRunning = true;

            while (keepRunning)
            {
                Console.Write("Enter command: ");
                var command = Console.ReadLine();

                command = command.Trim();

                if (command == "1")
                    PrintString(FA.StatesToString());
                else if (command == "2")
                    PrintString(FA.AlphabetToString());
                else if (command == "3")
                    PrintString(FA.TransitionsToString());
                else if (command == "4")
                    PrintString(FA.InitialStatesToString());
                else if (command == "5")
                    PrintString(FA.FinalStateToString());
                else if (command == "6")
                {
                    Console.Write("Enter sequence -> ");
                    var sequence = Console.ReadLine();
                    PrintString(FA.SequenceCheckResult(sequence));
                }
                else if (command == "0")
                    keepRunning = false;
                else
                    DisplayInvalidCommand();
            }
        }

        private void DisplayInvalidCommand()
        {
            PrintString("Not yet implemented!");
        }
    }
}
