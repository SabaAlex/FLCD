using Lab_2;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lab_3
{
    public class PIF
    {
        public List<Tuple<string, HashPosition>> Tuples { get; set; }

        public PIF()
        {
            Tuples = new List<Tuple<string, HashPosition>>();
        }

        public void GeneratePIF(string token, HashPosition hashPosition) =>
            Tuples.Add(new Tuple<string, HashPosition>(token, hashPosition));

    }
}
