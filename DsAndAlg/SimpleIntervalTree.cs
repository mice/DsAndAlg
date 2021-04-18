using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class SimpleIntervalTree
{
    public class Line1D
    {
        public float X;
        public float Y;
    }

    public class IntervalTreeNode
    {
        public IntervalTreeNode Left;
        public IntervalTreeNode Right;
        public float Mid;
        public List<Line1D> Hit;
    }

    private IntervalTreeNode root;
    public SimpleIntervalTree(List<Line1D> lines)
    {
        root = this.BuildTree(lines);
    }

    public List<Line1D> IntervalStabbing(float point)
    {
        var result = new List<Line1D>();
        Stabbing(root,point,result);
        return result;
    }


    private void Stabbing(IntervalTreeNode node,float point,List<Line1D> result)
    {
        if (node == null)
            return;

        if (node.Mid < point)
        {
            foreach(var item in node.Hit)
            {
                if (item.Y >= point)
                {
                    result.Add(item);
                }
            }
            Stabbing(node.Right,point,result);
        }
        else if (node.Mid >= point)
        {
            foreach (var item in node.Hit)
            {
                if (item.X < point)
                {
                    result.Add(item);
                }
            }
            Stabbing(node.Left, point, result);
        }
    }

    private IntervalTreeNode BuildTree(List<Line1D> lines)
    {
        if (lines.Count == 0)
        {
            return null;
        }
        var tmpNode = new IntervalTreeNode();
        tmpNode.Mid = Median(lines);
        var rightList = new List<Line1D>();
        var leftList = new List<Line1D>();
        var hitList = new List<Line1D>();
        Split(lines,tmpNode.Mid, hitList, leftList, rightList);
        tmpNode.Hit = hitList;
        tmpNode.Left = BuildTree(leftList);
        tmpNode.Right = BuildTree(rightList);
        return tmpNode;
    }

    private void Split(List<Line1D> lines,float mid,List<Line1D> hit,List<Line1D> left,List<Line1D> right)
    {
        foreach(var item in lines)
        {
            if (item.X > mid)
            {
                right.Add(item);
            }else if (item.Y < mid)
            {
                left.Add(item);
            }
            else
            {
                hit.Add(item);
            }
        }
    }


    private float Median(List<Line1D> lines)
    {
        float min = lines[0].X;float max = lines[0].Y;
        foreach(var item in lines)
        {
            if (item.X < min)
            {
                min = item.X;
            }
            if (item.Y > max)
            {
                max = item.Y;
            }
        }
      
        return (min+max)*0.5f;
    }
}
