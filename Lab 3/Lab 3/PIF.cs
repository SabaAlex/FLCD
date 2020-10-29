﻿using Lab_2;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public override string ToString()
        {
            return Tuples.Aggregate("", (acc, cur) =>
                acc += $"{cur.Item1} at {cur.Item2.BucketPosition.ToString() + " " + cur.Item2.ListPosition.ToString()}\n"
            );
        }
    }
}
