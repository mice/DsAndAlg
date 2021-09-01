using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 假设从0开始;
/// </summary>
public static  class HeapUtils
{

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Parent(int key) => (key - 1) / 2;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Left(int key) => 2 * key + 1;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Right(int k) => 2 * k + 2;

    public static T[] BuildHeap<T>(T[] source,IComparer<T> comp)
    {
        int Capacity = source.Length;
        for (int i = Capacity / 2 - 1; i >= 0; i--)
        {
            Heapify<T>(source,i, comp,Capacity);
        }
        return source;
    }


    // A recursive method to heapify a subtree  
    // with the root at given index  
    // This method assumes that the subtrees 
    // are already heapified 
    public static void Heapify<T>(T[] heapArray, int key, IComparer<T> comp,int current_heap_size)
    {
        int l = Left(key);
        int r = Right(key);

        int smallest = key;

        int leftComp = comp.Compare(heapArray[l] ,heapArray[smallest]);
        // if (l < current_heap_size && heapArray[l] < heapArray[smallest])
        if (l < current_heap_size && leftComp < 0 )
        {
            smallest = l;
        }

        int rightComp = comp.Compare(heapArray[r], heapArray[smallest]);
        //if (r < current_heap_size && heapArray[r] < heapArray[smallest])
        if (r < current_heap_size && rightComp < 0 )
        {
            smallest = r;
        }

        if (smallest != key)
        {
            Swap(ref heapArray[key], ref heapArray[smallest]);
            Heapify(heapArray, smallest, comp, current_heap_size);
        }
    }

    public static void Swap<T>(ref T lhs, ref T rhs)
    {
        T temp = lhs; lhs = rhs; rhs = temp;
    }
}