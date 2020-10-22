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

        public override bool Equals(object obj)
        {
            if ((obj == null) || !GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                var otherHashPosition = (HashPosition)obj;
                return (BucketPosition == otherHashPosition.BucketPosition)
                    && (ListPosition == otherHashPosition.ListPosition);
            }
        }

        public override string ToString()
        {
            return $"Bucket: {BucketPosition}, ListPosition: {ListPosition}";
        }

        public bool Valid()
        {
            return BucketPosition != -1;
        }
    }
}
