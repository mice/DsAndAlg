using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class RBTree
{
    public enum NodeColor:int 
    {
        Red,Black
    }

    public class RBTreeNode
    {
        public int value;
        public NodeColor color;
        public RBTreeNode left;
        public RBTreeNode right;
        public RBTreeNode parent;
    }

    protected RBTreeNode root_;

    public RBTree()
    {
        this.root_ = null;
    }

    public void Insert(int value)
    {
        if(root_ == null)
        {
            root_ = new RBTreeNode() { value = value,color = NodeColor.Black };
        }
        else
        {
            Insert(root_, value);
        }
    }

    /// <summary>
    /// 假设不重复值
    /// </summary>
    /// <param name="node"></param>
    /// <param name="value"></param>
    private void Insert(RBTreeNode node,int value)
    {
        RBTreeNode p = node, prev = null;
        while (p != null)
        {
            prev = p;
            if (p.value < value)
                p = p.right;
            else
                p = p.left;
        }

        //case 1
        if(prev.color == NodeColor.Black)
        {
            if (prev.value < value)
                prev.right = new RBTreeNode() {
                    value = value,
                    color = NodeColor.Black
                };
            else
                prev.left = new RBTreeNode() {
                    value = value,
                    color = NodeColor.Black
                };
        }
        else
        {
            //var node = Other();
        }
    }

    
    public static NodeColor GetNodeColor(RBTreeNode node)
        => node == null ? NodeColor.Black : node.color;

}
