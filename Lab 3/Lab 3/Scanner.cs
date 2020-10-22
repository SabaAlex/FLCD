using Lab_2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Lab_3
{
    public class Scanner
    {


        /// <summary>
        /// a variant
        /// One sym table for both
        /// </summary>
        public SymbolTable SymbolTable { get; set; }
        public PIF PIF { get; set; }
        public List<string> Tokens { get; set; }

        private string[] ops = new string[] { "+", "-", "*", "/", "%", "is", "!is", "=",
                                "<", ">", "<=", ">=", "OR", "AND"};

        private string[] sep = new string[] { ";", ",", "(", ")", "'", '"'.ToString(), "{", "}" };

        private string[] res = new string[] { "...", "char", "else", "if", "Number"
                                , "INPUT", "var", "for", "OUTPUT", ":", "String ", "Char" };

        public Scanner()
        {
            SymbolTable = new SymbolTable(769);
            PIF = new PIF();
        }

        public bool IsIdentifierOrConstant(string token)
        {
            ///Constants
            ///Digit
            if (Regex.IsMatch(token, "(?<=-)/d+|/d+"))
                return true;
            ///Character
            else if (Regex.IsMatch(token, "(?<=')[a-zA-Z0-9](?=')"))
                return true;
            ///String
            else if (Regex.IsMatch(token, "(?<=\")[a-zA-Z0-9]+(?=\")"))
                return true;

            ///Identifiers (starts with letter and after that any char from alphabet)
            else if (Regex.IsMatch(token, @"^[a-zA-Z][\da-zA-Z]?$"))
                return true;

            return false;
        }

        public void Scanning(string filePath)
        {
            if (!File.Exists(filePath))
                throw new Exception("Invalid File!");

            var operators = ops.ToList();
            var separators = sep.ToList();
            var reservedWord = res.ToList();

            using (StreamReader streamReader = File.OpenText(filePath))
            {
                string lineString;

                while ((lineString = streamReader.ReadLine()) != null)
                {
                    lineString.Split(new char[] { ' ', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                        .ToArray().ToList().ForEach(tokens => 
                        {
                            var patern =
                                "(?<=-)/d+|/d+|" +
                                "(?<=')[a-zA-Z0-9](?=')|" +
                                "(?<=\")[a-zA-Z0-9]+(?=\")|" +
                                "OR|AND|is|!is|[<>]=|" +
                                "[\'(){};,\"<>*/+%=-]" +
                                "[a-zA-Z0-9]+";

                            Regex.Split(tokens, patern)
                                .ToList().ForEach(token => 
                                {
                                    if(reservedWord.Contains(token) || operators.Contains(tokens) || separators.Contains(tokens))
                                        PIF.GeneratePIF(token, new HashPosition() { BucketPosition = -1 });

                                    else if (IsIdentifierOrConstant(token))
                                    {
                                        var index = SymbolTable.Position(token);
                                        PIF.GeneratePIF(token, index);
                                    }
                                    else
                                        throw new Exception("Lexical error");
                                });
                        });
                }
            }
        }
    }
}
