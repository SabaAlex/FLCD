using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Lab_4
{
    public class FA
    {
        private List<string> States { get; set; }
        private List<string> Alphabet { get; set; }
        private Dictionary<Tuple<string, string>, string> Transitions { get; set; }
        private string InitialState { get; set; }
        private List<string> FinalStates { get; set; }

        private string FileName;

        public FA(string fileName)
        {
            States = new List<string>();
            Alphabet = new List<string>();
            InitialState = "";
            FinalStates = new List<string>();

            Transitions = new Dictionary<Tuple<string, string>, string>();

            FileName = fileName;
        }

        public void ReadFA()
        {
            using (var streamReader = new StreamReader(FileName))
            {
                ///All possible states
                States = streamReader.ReadLine().Trim().Split(" ").ToList();

                ///Alphabet
                Alphabet = streamReader.ReadLine().Trim().Split(" ").ToList();

                ///Transitions
                ///1. Transitions are separated by |
                ///2. Transition elements are separated by ,
                var transitionsToCompute = streamReader.ReadLine().Split("|").Select(trasition => trasition.Trim()).ToList();

                transitionsToCompute.ForEach(transition => {
                    var elementsOfTransition = transition.Split(",").ToList();

                    Transitions.Add(new Tuple<string, string>(elementsOfTransition[0], elementsOfTransition[1]), elementsOfTransition[2]);
                });

                InitialState = streamReader.ReadLine().Trim();

                FinalStates = streamReader.ReadLine().Trim().Split(" ").ToList();
            }
        }

        public string SequenceCheckResult(string sequence)
        {
            return IsSequenceAccepted(sequence) ? $"Sequence {sequence} is valid" : $"Sequence {sequence} is invalid";
        }

        private bool IsSequenceAccepted(string sequence) {
            var currentState = InitialState;

            while (sequence != "")
            {
                var transitionKey = new Tuple<string, string>(currentState, sequence[0].ToString());

                if (Transitions.ContainsKey(transitionKey))
                {
                    currentState = Transitions[transitionKey];
                    sequence = sequence.Substring(1);
                }
                else return false;
            }

            if (!FinalStates.Contains(currentState))
                return false;

            return true;
        }

        public string StatesToString()
                => $"Set of States:\n\n{{{string.Join(", ", States)}}}\n";

        public string AlphabetToString()
                => $"Set for Alphabet:\n\n{{{string.Join(", ", Alphabet)}}}\n";

        public string InitialStatesToString()
                => $"Set of InitialStates:\n\n{InitialState}\n";

        public string FinalStateToString()
                => $"Set of FinalStates:\n\n{{{string.Join(", ", FinalStates)}}}\n";

        public string TransitionsToString()
                => $"Set of Transitions:\n\n{Transitions.Aggregate("", (accumulator, current) => accumulator += $"{current.Key.Item1} -{current.Key.Item2}> {current.Value}\n")}";

    }
}
