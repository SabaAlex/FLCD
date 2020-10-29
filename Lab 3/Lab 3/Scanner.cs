using Lab_2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Lab_3
{
    public class Scanner
    {
        /// <summary>
        /// a) variant
        /// One sym table for both
        /// </summary>
        private SymbolTable SymbolTable { get; set; }
        private PIF PIF { get; set; }

        private static readonly string ExceptionFile = $"Errors.txt";

        private static readonly string SymbotTableFile = $"SymbolTable.txt";

        private static readonly string PIFFile = $"PIF.txt";

        private Dictionary<string, List<string>> ClasifiedTokens;

        private static readonly string ProjectDirectory =
            @"C:\Personal\Limbaje formale si tehnici de compilare\Lab 3\Lab 3";

        private static readonly string OutputDirectory =
            Directory.CreateDirectory(Path.Combine(ProjectDirectory, "Logs", $"Run-{DateTime.Now.ToFileTimeUtc()}")).FullName;

        string patern =
                    ///matches relational things
                    "OR|AND|is|!is|[<>]=|" +
                    ///separators plus operators and remainint relational operators
                    "[():{}\'\";,<>*/+%=-]|\\.\\.\\.|" +
                    "\\s+";

        private string ExceptionsResults = "";

        #region Scanner Pre-Algorithm
        public Scanner()
        {
            SymbolTable = new SymbolTable(769);
            PIF = new PIF();                                          
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
                    if (string.IsNullOrWhiteSpace(token))
                    {
                        key = sr.ReadLine();
                        continue;
                    }

                    ClasifiedTokens[key].Add(Regex.Replace(token, @"\s+", ""));
                }
            }
        }

        #endregion

        #region Scanner Output

        private void OutputPIF()
        { 
            using (FileStream fs = File.Create(Path.Combine(OutputDirectory, PIFFile)))
            {
                Console.WriteLine(ExceptionsResults);
                byte[] info = new UTF8Encoding(true).GetBytes(PIF.ToString());
                fs.Write(info, 0, info.Length);
            }
        }

        private void OutputSymbolTable()
        {
            using (FileStream fs = File.Create(Path.Combine(OutputDirectory, SymbotTableFile)))
            {
                Console.WriteLine(ExceptionsResults);
                byte[] info = new UTF8Encoding(true).GetBytes(SymbolTable.ToString());
                fs.Write(info, 0, info.Length);
            }
        }

        private void OutputExceptions()
        {
            using (FileStream fs = File.Create(Path.Combine(OutputDirectory, ExceptionFile)))
            {
                Console.WriteLine(ExceptionsResults);
                byte[] info = new UTF8Encoding(true).GetBytes(ExceptionsResults);
                fs.Write(info, 0, info.Length);
            }
        }

        #endregion

        #region Scanner Utils

        private static string LastInserted = "";

        private static void SetLastInserted(string value)
            => LastInserted = value;

        private static bool IsLastInsertedGood(string lastInsertedMatch)
        { 
            return lastInsertedMatch == LastInserted;
        }

        #endregion

        #region Scanner String Manipulation

        private string AggregateListOfStrings(List<string> strings)
        {
            return strings.Aggregate("", (acc, cur) => acc += cur);
        }

        private void LogError(int lineCounter, string agregatedString)
        {
            ExceptionsResults += $"Lexical error at Line: {lineCounter} with Token: {agregatedString}\n";
        }
        public override string ToString()
        {
            return SymbolTable.ToString() + '\n' + PIF.ToString();
        }

        #endregion

        #region Scanner Matchers
        private bool IsSeparatorOperatorReserved(string StringToAnalize)
        {
            return ClasifiedTokens["ReservedWords"].Contains(StringToAnalize) ||
                    ClasifiedTokens["Operators"].Contains(StringToAnalize) ||
                    ClasifiedTokens["Separators"].Contains(StringToAnalize);
        }

        private static bool IsIdentifier(string[] tokenList, int position)
        {
            return Regex.IsMatch(tokenList[position], @"^[a-zA-Z][\da-zA-Z]*$");
        }

        private static bool IsNumberConstant(string[] tokenList, int position)
        {
            return !Regex.IsMatch(tokenList[position], @"\D");
        }

        private static bool IsNegativeNumberConstant(string agregatedString)
        {
            return Regex.IsMatch(agregatedString, "((?<!(\\d|[a-zA-Z]|\\)|'|\"))-\\d+)$");
        }

        private static bool IsChar(string agregatedString)
        {
            return Regex.IsMatch(agregatedString, "^(?<=\')[a-zA-Z\\d\\s](?=\')$");
        }

        private static bool IsString(string agregatedString)
        {
            return Regex.IsMatch(agregatedString, "^(?<=\")[a-zA-Z\\d\\s]*(?=\")$");
        }

        #endregion

        #region Scanner Adding Logic

        private void AddToken(string token)
        {
            var index = SymbolTable.Position(token);
            PIF.GeneratePIF(token, index);
        }

        private void ChangeLast(string token)
        {
            var index = SymbolTable.Position(token);
            PIF.ChangeLast(token, index);
        }

        #endregion

        #region Scanner Algorithm

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

                    var tokenList = Regex.Split(lineString, $"(?={patern})|(?<={patern})").Where(token => !(string.IsNullOrWhiteSpace(token) || string.IsNullOrEmpty(token))).ToArray();

                    ComputeLine(lineCounter, tokenList);

                    lineCounter++;
                }
            }

            Parallel.Invoke(() => OutputExceptions(), () => OutputPIF(), () => OutputSymbolTable());
        }

        private void ComputeLine(int lineCounter, string[] tokenList)
        {
            for (int i = 0; i < tokenList.Length; i++)
            {
                var token = tokenList[i];

                if (IsSeparatorOperatorReserved(token))
                {
                    PIF.GeneratePIF(token, new HashPosition() { BucketPosition = -1 });
                    SetLastInserted(token);
                    continue;
                }
                
                TreatIdentifierOrConstant(tokenList, ref i, lineCounter);
            }
        }

        private void TreatIdentifierOrConstant(string[] tokenList, ref int position, int lineCounter)
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
                    if (IsString(agregatedString))
                    {
                        AddToken(AggregateListOfStrings(tokenList.Skip(positionCopy).Take(endIndex - positionCopy + 1).ToList()));
                        return;
                    }

                    LogError(lineCounter, agregatedString);
                    return;
                }
                else
                {
                    LogError(lineCounter, AggregateListOfStrings(tokenList.Skip(position).ToList()));
                    position = tokenList.Length;
                    return;
                }

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
                    if (IsChar(agregatedString))
                    {
                        AddToken(AggregateListOfStrings(tokenList.Skip(positionCopy).Take(endIndex - positionCopy + 1).ToList()));
                        return;
                    }

                    LogError(lineCounter, agregatedString);
                    return;
                }
                else
                {
                    LogError(lineCounter, AggregateListOfStrings(tokenList.Skip(position).ToList()));
                    position = tokenList.Length;
                    return;
                }

            }
            ///digits
            else if (IsNumberConstant(tokenList, position))
            {
                if (IsLastInsertedGood("-"))
                {
                    var positionCopy = position;

                    var agregatedString = AggregateListOfStrings(tokenList.Skip(positionCopy - 2).Take(3).ToList());

                    if (IsNegativeNumberConstant(agregatedString))
                    {
                        ChangeLast(AggregateListOfStrings(tokenList.Skip(positionCopy - 1).Take(2).ToList()));
                        return;
                    }
                }

                AddToken(token);
                return;
            }


            ///identifiers
            if (IsIdentifier(tokenList, position))
            {
                AddToken(token);
                return;
            }

            LogError(lineCounter, token);
        }

        #endregion
    }
}
