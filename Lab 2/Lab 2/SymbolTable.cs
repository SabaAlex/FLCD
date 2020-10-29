using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lab_2
{
    public class SymbolTable
    {
        private MyHashTable<string> hashTable;

        public SymbolTable(int size)
        {
            hashTable = new MyHashTable<string>(size) {
                HashFunction = hashString =>
                {
                    var byteArray = Encoding.ASCII.GetBytes(hashString);
                    return (byteArray.ToList().Select(charByte => Convert.ToInt32(charByte)).Aggregate(0, (acc, x) => acc + x) % size);
                }
            };
        }
        /// <summary>
        /// Returns the HashPosition of an element in the hash table if it already exists,
        /// Otherwise it adds it and returns the HashPosition
        /// </summary>
        /// <param name="token">String thet represents the identifier</param>
        /// <returns></returns>
        public HashPosition Position(string token)
        {
            var valueInHash = hashTable.Find(token);

            if (valueInHash.Valid())
                return valueInHash;

            hashTable.Add(token);

            return hashTable.Find(token);
        }

        public override string ToString()
        {
            return hashTable.ToString();
        }
    }
}
