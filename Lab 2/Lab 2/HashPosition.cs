using System;
using System.Collections.Generic;
using System.Text;

namespace Lab_2
{
    /// <summary>
    /// Class that represents the hash position of a key (the bucket it is in and the position in the list)
    /// </summary>
    public class HashPosition
    {
        public int BucketPosition { get; set; }
        public int ListPosition { get; set; }

        public bool Valid()
        {
            return BucketPosition != -1;
        }
    }
}
