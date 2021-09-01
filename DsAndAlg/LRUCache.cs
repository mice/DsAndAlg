using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DsAndAlg
{
    
    public class LRUCache<TKey, TValue>
    {
        /// <summary>
        /// key->Node
        /// </summary>
        private Dictionary<TKey, LinkedListNode<(TKey, TValue)>> keyDict = new Dictionary<TKey, LinkedListNode<(TKey, TValue)>>();
        private LinkedList<(TKey, TValue)> data = new LinkedList<(TKey, TValue)>();

        public int Capcacity { get; private set; }
        public LRUCache(int cap)
        {
            this.Capcacity = cap;
        }

        public void Put(TKey key, TValue value)
        {
            if (Capcacity == 0)
                return;

            if (keyDict.TryGetValue(key, out var tmpVal))
            {
                tmpVal.Value = (key,value);
                data.Remove(tmpVal);
                data.AddFirst(tmpVal);
                return;
            }

            if (keyDict.Count == Capcacity)
            {
                var tmpValue = data.Last;
                data.RemoveLast();
                keyDict.Remove(tmpValue.Value.Item1);
            }

            var tmpNode = data.AddFirst((key, value));
            keyDict[key] = tmpNode;
        }

        public TValue Get(TKey key)
        {
            if (keyDict.TryGetValue(key, out var tmpVal))
            {
                data.Remove(tmpVal);
                data.AddFirst(tmpVal);
                return tmpVal.Value.Item2;
            }
            return default(TValue);
        }
    }
}
