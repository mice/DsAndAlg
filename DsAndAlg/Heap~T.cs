using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static HeapUtils;

namespace DsAndAlg
{
    /**MinHeap, revers Comp will be Max*/
    public class Heap<T> where T:IComparable<T>
    {
        public T[] heapArray { get; private set; }
        public int Capacity { get; private set; }
        public int current_heap_size { get; private set; }

        private IComparer<T> comp;

        public Heap(T[] data,IComparer<T> comp)
        {
            heapArray = new T[data.Length];
            this.comp = comp;
            Array.Copy(data, heapArray, data.Length);
            Capacity = current_heap_size = data.Length;
            BuildHeap<T>(heapArray,comp);
        }

        public Heap(int n)
        {
            this.Capacity = n;
            heapArray = new T[n];
            current_heap_size = 0;
        }

        public T Top() => heapArray[0];

        public bool IsEmpty => current_heap_size == 0;

        public bool Add(T key)
        {
            if (current_heap_size == Capacity)
            {
                return false;
            }

            int i = current_heap_size;
            heapArray[i] = key;
            current_heap_size++;

            //最小向上移动到比自己小的父亲节点
            //while (i != 0 && heapArray[i] < heapArray[Parent(i)])
            while (i != 0 && comp.Compare( heapArray[i] , heapArray[Parent(i)])<0)
            {
                Swap(ref heapArray[i], ref heapArray[Parent(i)]);
                i = Parent(i);
            }
            return true;
        }

        // This function deletes key at the   given index. 
        // 1. It first reduced value   to minus infinite, then calls extractMin() 
        // 2. swap with last element and movedown
        public void Remove(int key)
        {
            if(key == current_heap_size - 1)
            {
                heapArray[current_heap_size - 1] = default(T);
                current_heap_size--;
                return;
            }
            
            changeValueOnAKey(key, heapArray[current_heap_size - 1]);
            heapArray[current_heap_size - 1] = default(T);
            current_heap_size--;
        }

        public T ExtractTop()
        {
            if (current_heap_size <= 0)
            {
                throw new Exception("Empty");
            }
            if (current_heap_size == 1)
            {
                current_heap_size--;
                return heapArray[0];
            }

            T root = heapArray[0];
            heapArray[0] = heapArray[current_heap_size - 1];
            current_heap_size--;
            Heapify(heapArray,0,comp,current_heap_size);
            return root;
        }

        // Changes value on a key 
        public void changeValueOnAKey(int key, T new_val)
        {
            int compValue = comp.Compare(heapArray[key], new_val);
            //if (heapArray[key] == new_val)
            if (compValue == 0)
            {
                return;
            }
            if (compValue < 0)
            {
                IncreaseKey(key, new_val);
            }
            else
            {
                DecreaseKey(key, new_val);
            }
        }

        // Increases value of given key to new_val. 
        // It is assumed that new_val is greater  
        // than heapArray[key].  
        // Heapify from the given key 
        public void IncreaseKey(int key, T new_val)
        {
            heapArray[key] = new_val;
            Heapify(heapArray, key, comp, current_heap_size);
        }

        // Decreases value of given key to new_val.  
        // It is assumed that new_val is smaller  
        // than heapArray[key].  
        public void DecreaseKey(int key, T new_val)
        {
            heapArray[key] = new_val;

            //while (key != 0 && heapArray[key] < heapArray[Parent(key)])
            while (key != 0 && comp.Compare( heapArray[key], heapArray[Parent(key)])<0)
            {
                Swap(ref heapArray[key],
                    ref heapArray[Parent(key)]);
                key = Parent(key);
            }
        }

        public void ToSortedArary(List<T> output)
        {
            while (this.current_heap_size > 0)
            {
                var top = ExtractTop();
                output.Add(top);
            }
        }
    }
}
