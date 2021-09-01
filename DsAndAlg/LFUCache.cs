using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DsAndAlg
{
    public class LFUCache<TKey,TValue> 
    {
        public class Node
        {
            public TKey key;
            public TValue value;
            public int Freq;
            public LinkedListNode<TKey> itr;
        }

        private int minFreq = 0;
        /// <summary>
        /// key->Node
        /// </summary>
        private Dictionary<TKey, Node> n_ = new Dictionary<TKey, Node>();

        /// <summary>
        /// freq-List
        /// </summary>
        private Dictionary<int, LinkedList<TKey>> l_ = new Dictionary<int, LinkedList<TKey>>();

        public int Capcacity { get; private set; }
        public LFUCache(int cap)
        {
            this.Capcacity = cap;
        }

        public void Put(TKey key,TValue value)
        {
            if (Capcacity == 0)
                return;

            if (n_.TryGetValue(key, out var tmpVal))
            {
                tmpVal.value = value;
                Touch(tmpVal);
                return;
            }

            if(n_.Count == Capcacity)
            {
                TKey keyToEvict = l_[minFreq].Last();
                l_[minFreq].RemoveLast();

                n_.Remove(keyToEvict);
            }

            int freq = 1;
            minFreq = freq;
            if (!l_.ContainsKey(freq))
            {
                l_[freq] = new LinkedList<TKey>();
            }
            l_[freq].AddFirst(key);
            n_[key] = new Node()
            {
                key = key,
                value = value,
                Freq = freq,
                itr = l_[freq].First
            };
        }

        public TValue Get(TKey key)
        {
            if (n_.TryGetValue(key, out var tmpVal))
            {
                Touch(tmpVal);
                return tmpVal.value;
            }
            return default(TValue);
        }

        private void Touch(Node node)
        {
            int prevFreq = node.Freq;
            int freq = ++node.Freq;
            l_[prevFreq].Remove(node.itr);

            if(l_[prevFreq].Count==0 && prevFreq == minFreq)
            {
                l_.Remove(prevFreq);
                ++minFreq;
            }

            if (!l_.ContainsKey(freq))
            {
                l_[freq] = new LinkedList<TKey>();
            }
            l_[freq].AddFirst(node.key);
            node.itr = l_[freq].First;
        }

    }
}
