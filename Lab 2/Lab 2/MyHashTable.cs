using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Lab_2
{
    public class MyHashTable<K>
    {
        private readonly int size;
        private readonly LinkedList<K>[] items;
        public Func<K, int> HashFunction { get; set; }

        /// <summary>
        /// Function that returns a hashable code
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private int ToHashCode(K key)
        {
            return HashFunction(key);
        }

        public MyHashTable(int size)
        {
            this.size = size;
            items = new LinkedList<K>[size];
        }

        /// <summary>
        /// Applies the hash function on the key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private int GetArrayPosition(K key)
        {
            int position = ToHashCode(key) % size;
            return Math.Abs(position);
        }

        /// <summary>
        /// Returns a HashPosition tuple containing the bucket position and the position of the list of a key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public HashPosition Find(K key)
        {
            int position = GetArrayPosition(key);
            LinkedList<K> linkedList = GetLinkedList(position);

            int listPosition = 0;

            foreach (var item in linkedList)
            {
                if (item.Equals(key))
                {
                    return new HashPosition() {
                        BucketPosition = position,
                        ListPosition = listPosition,
                    };
                }
                listPosition += 1;
            }

            return new HashPosition() { BucketPosition = -1 };
        }

        /// <summary>
        /// Function that adds a key value pair to the coresponding bucket
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(K key)
        {
            int position = GetArrayPosition(key);
            LinkedList<K> linkedList = GetLinkedList(position);
            var item = key;
            linkedList.AddLast(item);
        }

        /// <summary>
        /// Gets the bucket from a certain pozition, if null it creates the bucket
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        private LinkedList<K> GetLinkedList(int position)
        {
            LinkedList<K> linkedList = items[position];
            if (linkedList == null)
            {
                linkedList = new LinkedList<K>();
                items[position] = linkedList;
            }

            return linkedList;
        }
    }
}
