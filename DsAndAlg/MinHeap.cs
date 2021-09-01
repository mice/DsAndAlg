using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class MinHeap
{
    public int[] heapArray {get;private set;}
    public int Capacity{get;private set;}
    public int current_heap_size {get;private set;}

    public MinHeap(int[] data)
    {
        heapArray = new int[data.Length];
        Array.Copy(data,heapArray,data.Length);
        Capacity = current_heap_size = data.Length;
        for (int i = Capacity/2-1; i >= 0 ; i--)
        {
            Heapify(i);
        }
    }

    public MinHeap(int n){
        this.Capacity = n;
        heapArray = new int[n];
        current_heap_size = 0;
    }

    public int GetMin()=>heapArray[0];

    public bool IsEmpty =>current_heap_size==0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Parent(int key)=>(key-1)/2;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Left(int key)=> 2*key+1;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Right(int k) =>2*k+2;

    public bool insertKey(int key){
        if(current_heap_size == Capacity){
            return false;
        }

        int i = current_heap_size;
        heapArray[i] = key;
        current_heap_size++;

        //最小向上移动到比自己小的父亲节点
        while(i!=0&& heapArray[i]<heapArray[Parent(i)])
        {
            Swap(ref heapArray[i], ref heapArray[Parent(i)]);
            i = Parent(i);
        }
        return true;
    }

   // This function deletes key at the  
    // given index. It first reduced value  
    // to minus infinite, then calls extractMin() 
    public void deleteKey(int key) 
    { 
        DecreaseKey(key, int.MinValue); 
        ExtractMin(); 
    }

    // Changes value on a key 
    public void changeValueOnAKey(int key, int new_val) 
    { 
        if (heapArray[key] == new_val) 
        { 
            return; 
        } 
        if (heapArray[key] < new_val) 
        { 
            IncreaseKey(key, new_val); 
        } else
        { 
            DecreaseKey(key, new_val); 
        } 
    } 

    public int ExtractMin()
    {
        if(current_heap_size<=0)
        {
            throw new Exception("Empty");
        }
        if(current_heap_size==1){
            current_heap_size--;
            return heapArray[0];
        }

        int root = heapArray[0];
        heapArray[0] = heapArray[current_heap_size-1];
        current_heap_size --;
        Heapify(0);
        return root;
    }

    // A recursive method to heapify a subtree  
    // with the root at given index  
    // This method assumes that the subtrees 
    // are already heapified 
    public void Heapify(int key)
    {
        int l = Left(key);
        int r = Right(key);

        int smallest = key;

        if(l<current_heap_size && heapArray[l]<heapArray[smallest]){
            smallest = l;
        }

        if(r<current_heap_size && heapArray[r]<heapArray[smallest]){
            smallest = r;
        }

        if(smallest!=key){
            Swap(ref heapArray[key],ref heapArray[smallest]);
            Heapify(smallest);
        }
    }

    // Increases value of given key to new_val. 
    // It is assumed that new_val is greater  
    // than heapArray[key].  
    // Heapify from the given key 
    public void IncreaseKey(int key, int new_val) 
    { 
        heapArray[key] = new_val; 
        Heapify(key); 
    } 

    // Decreases value of given key to new_val.  
    // It is assumed that new_val is smaller  
    // than heapArray[key].  
    public void DecreaseKey(int key, int new_val) 
    { 
        heapArray[key] = new_val; 
    
        while (key != 0 && heapArray[key] <  
                        heapArray[Parent(key)]) 
        { 
            Swap(ref heapArray[key],  
                ref heapArray[Parent(key)]); 
            key = Parent(key); 
        } 
    }

    public void ToArary(List<int> output)
    {
        while(this.current_heap_size>0){
            var top = ExtractMin();
            output.Add(top);
        }
    }

    private void Swap<T>(ref T lhs,ref T rhs){
        T temp = lhs; lhs = rhs; rhs = temp; 
    }
}