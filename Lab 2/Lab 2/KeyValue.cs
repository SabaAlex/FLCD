using System;
using System.Collections.Generic;
using System.Text;

namespace Lab_2
{
    /// <summary>
    /// Generic class that holds a key and a value
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="V"></typeparam>
    public class KeyValue<K, V>
    {
        public K Key { get; set; }
        public V Value { get; set; }
    }
}
