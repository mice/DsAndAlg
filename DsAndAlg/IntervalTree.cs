using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class IntervalTree<TKey> where TKey : IComparable<TKey>
{
    public class IntervalBSTNode<T> :IBNode<IntervalBSTNode<T>>
    {
        public T key { get; set; }
        public T end { get;private set ; }
        public T max { get; set; }

        public IntervalBSTNode<T> left { get; set; }
        public IntervalBSTNode<T> right { get; set; }

        public IntervalBSTNode(T ele, T end) : this(ele,end, null, null)
        {

        }

        public IntervalBSTNode(T ele, T end,IntervalBSTNode<T> left, IntervalBSTNode<T> right)
        {
            this.key = ele; this.end = end; this.left = left; this.right = right;
        }

    }

    protected IntervalBSTNode<TKey> root_;
    public IntervalTree()
    {
        this.root_ = null;
    }

    public void Remove(TKey value)
    {

    }


    public void Insert(TKey value,TKey end)
    {
        if (root_ == null)
        {
            root_ = new IntervalBSTNode<TKey>(value, end);
            root_.max = end;
        }else
        {
            Insert(root_, value, end);
        }
    }

    private IntervalBSTNode<TKey> Insert(IntervalBSTNode<TKey> node, TKey data,TKey end)
    {
        if (node ==null)
        {
            var tmp = new IntervalBSTNode<TKey>(data, end);
            tmp.left = tmp.right = null;
            tmp.max = end;
            return tmp;
        }

        var cmp = node.key.CompareTo(data);
        if (cmp >= 0)
        {
            node.left = Insert(node.left, data,end);
        }
        else
        {
            node.right = Insert(node.right, data, end);
        }
        if (node.max.CompareTo(end)<0)
        {
            node.max = end;
        }
        return node;
    }

    public void RangeTest(TKey start,TKey end ,List<IntervalBSTNode<TKey>> results)
    {
        if (root_ == null)
            return;

        DoRangeTest(root_, start, end, results);
    }

    public void StableTest(TKey val,List<IntervalBSTNode<TKey>> results)
    {
        if (root_ == null)
            return;

        DoStableTest(root_, val, results);
    }


    private void DoStableTest(IntervalBSTNode<TKey> node,TKey val, List<IntervalBSTNode<TKey>> results)
    {
        if (node == null)
            return;

        var cmp = node.key.CompareTo(val);
        var cmpEnd = node.end.CompareTo(val);
        var cmpMax = node.max.CompareTo(val);
        Console.WriteLine($"TestNode::{node.key}::{node.end}::max:{node.max}");
        if (cmp <= 0 && cmpEnd >0)
        {
            results.Add(node);
        }
        if(cmpMax<0)
        {
            return;
        }

        if(cmp>=0 || cmpMax > 0)
        {
            DoStableTest(node.left, val, results);
        }

        if (cmp < 0) 
        {
            DoStableTest(node.right, val, results);
        }
    }

    private void DoRangeTest(IntervalBSTNode<TKey> node, TKey start,TKey end, List<IntervalBSTNode<TKey>> results)
    {
        if (node == null)
            return;

        var cmp = node.key.CompareTo(start);
        var cmpEnd = node.end.CompareTo(start);
        var cmpMax = node.max.CompareTo(start);

        Console.WriteLine($"TestNode::{node.key}::{node.end}::max:{node.max}");
        if (!(end.CompareTo(node.key) <0 || node.end.CompareTo(start)<0))
        {
            results.Add(node);
        }

        // If left child of root is present and max of left child is
        // greater than or equal to given interval, then i may
        // overlap with an interval is left subtree
        if (node.left != null && node.left.max.CompareTo(start)>=0)
            DoRangeTest(node.left, start, end, results);

        if (cmp < 0 || end.CompareTo(node.key)>0)
        {
            DoRangeTest(node.right, start, end, results);
        }
    }
}
