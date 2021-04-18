using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class IntBBST
{
    protected IntBBSTNode root_;

    public IntBBST()
    {
        this.root_ = null;
    }
    
    public int Depth()
    {
        return Depth(root_);
    }

    protected int Depth(IntBBSTNode node)
    {
        if (node == null)
            return 0;
        return Math.Max(Depth(node.left),Depth(node.right))+1;
    }

    protected int BalanceFactor(IntBBSTNode node)
    {
        if (node == null)
            return 0;
        return  Depth(node.left)- Depth(node.right);
    }

    public void Add(int ele)
    {
        if (root_ == null)
        {
            root_ = new IntBBSTNode(ele);
            return;
        }
        root_ = Insert(root_, ele);
    }

    private IntBBSTNode Insert(IntBBSTNode root, int value)
    {
        if(root == null)
        {
            root = new IntBBSTNode(value);
            return root;
        }else if (value < root.key)
        {
            root.left = Insert(root.left, value);
            root = ReBalance(root);
        }else if (value>=root.key)
        {
            root.right = Insert(root.right, value);
            root = ReBalance(root);
        }
        return root;
    }

    private IntBBSTNode ReBalance(IntBBSTNode node)
    {
        var balanceFactor  = BalanceFactor(node);
        if (balanceFactor >1)
        {
            if (BalanceFactor(node.left)>0)
                node = LeftLeft_Rotation(node);
            else
                node = LeftRight_Rotation(node);
        }
        else if (balanceFactor <-1)
        {
            if (BalanceFactor(node.right) > 0)
            {
                node = RightLeft_Rotation(node);
            }
            else
            {
                node = RightRight_Rotation(node);
            }
        }
        return node;
    }



    /// <summary>
    ///      A                 B 
    ///     / \              /   \
    ///    B   X --->       C    A
    ///   / \              /     /\
    ///  C   Y            N    Y  X
    /// N
    /// <param name="parent">A</param>
    /// <returns></returns>
    private IntBBSTNode LeftLeft_Rotation(IntBBSTNode parent)
    {
        IntBBSTNode temp;
        temp = parent.left;
        parent.left = temp.right;
        temp.right = parent;
        return temp;
    }

    private IntBBSTNode LeftRight_Rotation(IntBBSTNode parent)
    {
        IntBBSTNode temp = parent.left;
        parent.left = RightRight_Rotation(temp);
        return LeftLeft_Rotation(parent);
    }


    private IntBBSTNode RightLeft_Rotation(IntBBSTNode parent)
    {
        IntBBSTNode temp = parent.right;
        parent.right = LeftLeft_Rotation(temp);
        return RightRight_Rotation(parent);
    }

    /// <summary>
    ///      A                 B 
    ///     / \              /   \
    ///    X   B --->       A     C
    ///       /  \         / \     \
    ///      Y    C       X   Y     N
    ///             N
    /// </summary>
    /// <param name="parent">A</param>
    /// <returns></returns>
    private IntBBSTNode RightRight_Rotation(IntBBSTNode parent)
    {
        IntBBSTNode temp;
        temp = parent.right;
        parent.right = temp.left;
        temp.left = parent;
        return temp;
    }

    private void Inorder(IntBBSTNode root,System.Action<IntBBSTNode> func)
    {
        if (root==null) return;

        Inorder(root.left, func);
        func.Invoke(root);
        Inorder(root.right, func);
    }

    public override string ToString()
    {
        var s = new StringBuilder();
        Inorder(root_,(v)=> {
            var nLen = Depth(v);
            for (int i = 0; i < nLen; i++)
            {
                s.Append("*");
            }
            s.Append(v.key);
            s.Append(",");
            s.AppendLine("");
        });
        return s.ToString();
    }


    public class IntBBSTNode
    {
        public int key { get; set; }
        public int factor { get; set; }
        public IntBBSTNode left { get; set; }
        public IntBBSTNode right { get; set; }
        public IntBBSTNode()
        {
            this.left = this.right = null;
        }

        public IntBBSTNode(int ele) : this(ele, null, null)
        {

        }

        public IntBBSTNode(int ele, IntBBSTNode left, IntBBSTNode right)
        {
            this.key = ele; this.left = left; this.right = right;
        }

        public bool IsLeaf => this.left != null && this.right != null;
    }
}
