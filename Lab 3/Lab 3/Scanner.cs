using Lab_2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
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

        private Dictionary<string, List<string>> ClasifiedTokens;

        private static readonly string ProjectDirectory =
            @"C:\Personal\Limbaje formale si tehnici de compilare\Lab 3\Lab 3";

        string patern =
                    ///matches relational things
                    "OR|AND|is|!is|[<>]=|" +
                    ///separators plus operators and remainint relational operators
                    "[():{}\'\";,<>*/+%=-]|" +
                    "\\s+";


        private static string LastInserted = "";

        private static void SetLastInserted(string value)
            => LastInserted = value;

        private static bool IsLastInsertedGood(string lastInsertedMatch)
        { 
            return lastInsertedMatch == LastInserted;
        }

        public Scanner()
        {
            SymbolTable = new SymbolTable(769);
            PIF = new PIF();
            ///                                                   
            ClasifiedTokens = new Dictionary<string, List<string>> {
                { "Operators", new List<string>() },
                { "Separators", new List<string>() },
                { "ReservedWords", new List<string>() },
            };
        }

        public void LoadTokens(string fileName)
        {
            var _filePath = Path.Combine(ProjectDirectory, fileName);

            string key;

            using (var sr = new StreamReader(_filePath))
            {
                key = sr.ReadLine().Trim();

                while (!sr.EndOfStream)
                {
                    var token = sr.ReadLine();
                    if(string.IsNullOrWhiteSpace(token))
                    {
                        key = sr.ReadLine();
                        continue;
                    }

                    ClasifiedTokens[key].Add(Regex.Replace(token, @"\s+", ""));
                }
            }
        }

        private string AggregateListOfStrings(List<string> strings)
        {
            return strings.Aggregate("", (acc, cur) => acc += cur);
        }

        private bool IsSeparatorOperatorReserved(string StringToAnalize)
        {
            return ClasifiedTokens["ReservedWords"].Contains(StringToAnalize) ||
                    ClasifiedTokens["Operators"].Contains(StringToAnalize) ||
                    ClasifiedTokens["Separators"].Contains(StringToAnalize);
        }

        public bool IsIdentifierOrConstant(string[] tokenList, ref int position)
        {
            var token = tokenList[position];

            ///strings
            if (IsLastInsertedGood("\""))
            {
                var endIndex = tokenList.ToList().FindIndex(position, (stringToFind) => stringToFind == "\"");
                if (endIndex != -1)
                {
                    var positionCopy = position;

                    position = endIndex - 1;
                    var agregatedString = AggregateListOfStrings(tokenList.Skip(positionCopy - 1).Take(endIndex - positionCopy + 2).ToList());
                    if (Regex.IsMatch(agregatedString, "^(?<=\")[a-zA-Z\\d\\s]*(?=\")$"))
                    {
                        AddToken(AggregateListOfStrings(tokenList.Skip(positionCopy).Take(endIndex - positionCopy + 1).ToList()));
                        return true;
                    }

                    return false;
                }
                else
                    return false;
            }
            ///chars
            else if (IsLastInsertedGood("\'"))
            {
                var endIndex = tokenList.ToList().FindIndex(position, (stringToFind) => stringToFind == "\'");
                if (endIndex != -1)
                {
                    var positionCopy = position;

                    position = endIndex - 1;

                    var agregatedString = AggregateListOfStrings(tokenList.Skip(positionCopy - 1).Take(endIndex - positionCopy + 2).ToList());
                    if (Regex.IsMatch(agregatedString, "^(?<=\')[a-zA-Z\\d\\s](?=\')$"))
                    {
                        AddToken(AggregateListOfStrings(tokenList.Skip(positionCopy).Take(endIndex - positionCopy + 1).ToList()));
                        return true;
                    }

                    return false;
                }
                else
                    return false;

            }
            ///digits
            else if (!Regex.IsMatch(tokenList[position], "\\D"))
            {
                if (IsLastInsertedGood("-"))
                {

                }
                
                return true;
            }

            ///identifiers
            return Regex.IsMatch(tokenList[position], @"^[a-zA-Z][\da-zA-Z]*$");
        }

        public override string ToString()
        {
            return SymbolTable.ToString() + '\n' + PIF.ToString();
        }

        private void AddToken(string token)
        {
            var index = SymbolTable.Position(token);
            PIF.GeneratePIF(token, index);
        }

        public void Scanning(string filePath)
        {
            if (!File.Exists(filePath))
                throw new Exception("Invalid File!");

            using (StreamReader streamReader = new StreamReader(filePath))
            {
                string lineString;
                int lineCounter = 0;

                while (!streamReader.EndOfStream)
                {
                    lineString = streamReader.ReadLine();

                    var tokenList = Regex.Split(lineString, $"(?={patern})|(?<={patern})").Where(token => token != " " && token != "").ToArray();

                    for (int i = 0; i < tokenList.Length; i++)
                    {
                        var token = tokenList[i];

                        if (IsSeparatorOperatorReserved(token))
                        {
                            PIF.GeneratePIF(token, new HashPosition() { BucketPosition = -1 });
                            SetLastInserted(token);
                        }
                        else if (IsIdentifierOrConstant(tokenList, ref i))
                        {
                            AddToken(token);
                        }
                        else
                        {
                            if (token == "" || token == " ")
                                continue;
                            Console.WriteLine($"Lexical error at Line: {lineCounter} with Token: {token}");
                        }            
                    }

                    lineCounter++;
                }
            }
        }
    }
}
