using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Lab_2
{
    public class MyHashTable<K, V>
    {
        private readonly int size;
        private readonly LinkedList<KeyValue<K, V>>[] items;
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
            items = new LinkedList<KeyValue<K, V>>[size];
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
            LinkedList<KeyValue<K, V>> linkedList = GetLinkedList(position);

            int listPosition = 0;

            foreach (KeyValue<K, V> item in linkedList)
            {
                if (item.Key.Equals(key))
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
        public void Add(K key, V value)
        {
            int position = GetArrayPosition(key);
            LinkedList<KeyValue<K, V>> linkedList = GetLinkedList(position);
            KeyValue<K, V> item = new KeyValue<K, V>() { Key = key, Value = value };
            linkedList.AddLast(item);
        }

        /// <summary>
        /// Gets the bucket from a certain pozition, if null it creates the bucket
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        private LinkedList<KeyValue<K, V>> GetLinkedList(int position)
        {
            LinkedList<KeyValue<K, V>> linkedList = items[position];
            if (linkedList == null)
            {
                linkedList = new LinkedList<KeyValue<K, V>>();
                items[position] = linkedList;
            }

            return linkedList;
        }
    }
}
