using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

public enum NodeColor : int
{
    Red, Black
}

public class RBTree<TKey> where TKey : IComparable<TKey>
{
    [DebuggerDisplay("{value}:{color}")]
    public class RBTreeNode<T>:IBNode<RBTreeNode<T>>
    {
        public T value;
        public NodeColor color;
        public RBTreeNode<T> left { get; set; }
        public RBTreeNode<T> right { get; set; }
        public RBTreeNode<T> parent;

        public override string ToString()
        {
            return $"{value}_{color}";
        }
    }

    protected RBTreeNode<TKey> root_;

    public RBTreeNode<TKey> root =>root_;
    public RBTree()
    {
        this.root_ = null;
    }

    public RBTree(RBTreeNode<TKey> root)
    {
        this.root_ = root;
    }

    public void Insert(TKey value)
    {
        if(root_ == null)
        {
            //case 1
            root_ = new RBTreeNode<TKey>() { value = value,color = NodeColor.Black };
        }
        else
        {
            Insert(root_, value);
        }
    }

    public bool Exist(TKey key) => Find(key) != null;

    private RBTreeNode<TKey> Find(TKey key)
        =>root_ != null? _Find(root_, key) : null;

    private RBTreeNode<TKey> _Find(RBTreeNode<TKey> root,TKey key)
    {
        RBTreeNode<TKey> p = root;
        while (p != null)
        {
            var cmp = p.value.CompareTo(key);
            if (cmp == 0)
                return p;
            else if (cmp > 0)
                p = p.left;
            else
                p = p.right;
        }
        return p;
    }

    #region Insert
    /// <summary>
    /// 假设不重复值
    /// </summary>
    /// <param name="node"></param>
    /// <param name="value"></param>
    private void Insert(RBTreeNode<TKey> node, TKey data)
    {
        var p = node;
        if (p.value.CompareTo(data) >= 0)
        {
            if (p.left != null)
                Insert(p.left, data);
            else
            {
                var tmp = new RBTreeNode<TKey>();
                tmp.value = data;
                tmp.left = tmp.right= null;
                tmp.parent = p;
                p.left = tmp;
                InsertCase1_2(tmp);
            }
        }
        else
        {
            if (p.right != null)
                Insert(p.right, data);
            else
            {
                var tmp = new RBTreeNode<TKey>();
                tmp.value = data;
                tmp.left = tmp.right = null;
                tmp.parent = p;
                p.right= tmp;
                InsertCase1_2(tmp);
            }
        }
    }

    private void InsertCase1_2(RBTreeNode<TKey> node)
    {
        var parent = node.parent;
        if(parent==null)
        {
            node.color = NodeColor.Black;
            return;
        }else if (parent.color == NodeColor.Black) //case 2 parent is black;
        {
            return;
        }
        else
        {
            InsertCase3(node);
        }
    }

    private void InsertCase3(RBTreeNode<TKey> n)
    {
        var uncl = Uncle(n);
        if (uncl != null && uncl.color == NodeColor.Red)
        {
            //flip color;
            n.parent.color = NodeColor.Black;
            uncl.color = NodeColor.Black;
            var grandParent = Grandparent(n);
            grandParent.color = NodeColor.Red;
            //需要递归recolor;
            InsertCase1_2(grandParent);
        }
        else
        {
            var parent = n.parent;
            var grandParent = Grandparent(n);
            if (parent.right == n && grandParent.left == parent)
            {
                rotate_left(n);
                n.color = NodeColor.Black;
                grandParent.color = NodeColor.Red;
                rotate_right(n);
            }
            else if (parent.left == n && grandParent.right == parent)
            {
                rotate_right(n);
                n.color = NodeColor.Black;
                grandParent.color = NodeColor.Red;
                rotate_left(n);
            }
            else if (parent.left == n && grandParent.left==parent)
            {
                parent.color = NodeColor.Black;
                grandParent.color = NodeColor.Red;
                rotate_right(parent);
            }
            else if (parent.right == n && grandParent.right == parent)
            {
                parent.color = NodeColor.Black;
                grandParent.color = NodeColor.Red;
                rotate_left(parent);
            }
        }
    }
    #endregion

    #region Delete
    public bool Delete(TKey value)
    {
        if (root_ == null)
            return false;
        var node = _Find(root_, value);
        var hasValue = node != null;
        if (hasValue)
        {
            DeleteAndReblance(node);
        }
        return hasValue;
    }

    private RBTreeNode<TKey> ReplaceNode(RBTreeNode<TKey> x)
    {
        // when node have 2 children
        if (x.left != null && x.right != null)
            return Successor(x.right);

        // when leaf
        if (x.left == null && x.right == null)
            return null;

        // when single child
        if (x.left != null)
            return x.left;
        else
            return x.right;
    }

    private void DeleteAndReblance(RBTreeNode<TKey> p)
    {
        var u = ReplaceNode(p);
        // True when u and v are both black
        bool uvBlack = ((u == null || u.color == NodeColor.Black) && (p.color == NodeColor.Black));
        var parent = p.parent;

        if (u == null)
        {
            if (p == root_)
            {
                root_ = null;
            }
            else
            {
                if (uvBlack)
                {
                    // u and v both black
                    // v is leaf, fix double black at v
                    fixDoubleBlack(p);
                }
                else
                {
                    if (Sibling(p) != null)
                    {
                        Sibling(p).color = NodeColor.Red;
                    }

                    // delete v from the tree
                    if(p.parent.left == p)
                    {
                        parent.left = null;
                    }
                    else
                    {
                        parent.right = null;
                    }
                }
            }
            return;
        }
        
        if(p.left ==null || p.right == null)
        {
            if (p == root_)
            {
                p.value = u.value;
                p.left = p.right = null;
            }
            else
            {
                if(p.parent.left == p)
                {
                    parent.left = u;
                }
                else
                {
                    p.right = u;
                }
                u.parent = parent;
                if (uvBlack)
                {
                    fixDoubleBlack(u);
                }
                else
                {
                    u.color = NodeColor.Black;
                }
            }
            return;
        }

        // v has 2 children, swap values with successor and recurse
        var tmpValue = u.value;
        u.value = p.value;
        p.value = tmpValue;
    }

    private void fixDoubleBlack(RBTreeNode<TKey> x)
    {
        if (x == root)
            // Reached root
            return;

        RBTreeNode<TKey> sibling = Sibling(x), parent = x.parent;
        if (sibling == null)
        {
            // No sibiling, double black pushed up
            fixDoubleBlack(parent);
        }
        else
        {
            if (sibling.color == NodeColor.Red)
            {
                // Sibling red
                parent.color = NodeColor.Red;
                sibling.color = NodeColor.Black;
                if (sibling.parent.left==sibling)
                {
                    // left case
                    rotate_right(parent);
                }
                else
                {
                    // right case
                    rotate_left(parent);
                }
                fixDoubleBlack(x);
            }
            else
            {
                // Sibling black
                if (HasRedChild(sibling))
                {
                    // at least 1 red children
                    if (sibling.left != null && sibling.left.color == NodeColor.Red) {
                        if (IsOnLeft(sibling))
                        {
                            // left left
                            sibling.left.color = sibling.color;
                            sibling.color = parent.color;
                            rotate_right(parent);
                        }
                        else
                        {
                            // right left
                            sibling.left.color = parent.color;
                            rotate_right(sibling);
                            rotate_left(parent);
                        }
                    } else
                    {
                        if (IsOnLeft(sibling))
                        {
                            // left right
                            sibling.right.color = parent.color;
                            rotate_left(sibling);
                            rotate_right(parent);
                        }
                        else
                        {
                            // right right
                            sibling.right.color = sibling.color;
                            sibling.color = parent.color;
                            rotate_left(parent);
                        }
                    }
                    parent.color = NodeColor.Black;
                }
                else
                {
                    // 2 black children
                    sibling.color = NodeColor.Red;
                    if (parent.color == NodeColor.Black)
                        fixDoubleBlack(parent);
                    else
                        parent.color = NodeColor.Black;
                }
            }
        }
    }
    #endregion

    void rotate_right(RBTreeNode<TKey> p)
    {
        var gp = Grandparent(p);
        var fa = p.parent;
        var y = p.right;

        fa.left = y;

        if (y != null)
            y.parent = fa;
        p.right= fa;
        fa.parent = p;

        if (root_ == fa)
            root_ = p;
        p.parent = gp;

        if (gp != null)
        {
            if (gp.left == fa)
                gp.left = p;
            else
                gp.right = p;
        }

    }

    void rotate_left(RBTreeNode<TKey> p)
    {
        if (p.parent == null)
        {
            root_ = p;
            return;
        }
        var gp = Grandparent(p);
        var fa = p.parent;
        var y = p.left;

        fa.right = y;

        if (y != null)
            y.parent = fa;
        p.left = fa;
        fa.parent = p;

        if (root_ == fa)
            root_ = p;
        p.parent = gp;

        if (gp != null)
        {
            if (gp.left == fa)
                gp.left = p;
            else
                gp.right = p;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    RBTreeNode<TKey> Grandparent(RBTreeNode<TKey> n) => n.parent.parent;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    RBTreeNode<TKey> Uncle(RBTreeNode<TKey> n)
    {
        var grandParent = Grandparent(n);
        if (n.parent == grandParent.left)
            return grandParent.right;
        else
            return grandParent.left;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    RBTreeNode<TKey> Sibling(RBTreeNode<TKey>  n)
    {
        if(n == n.parent.left)
            return n.parent.right;
        else
            return n.parent.left;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    bool HasRedChild(RBTreeNode<TKey> n)
    {
        return (n.left != null && n.left.color == NodeColor.Red) ||
               (n.right != null && n.right.color == NodeColor.Red);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsOnLeft(RBTreeNode<TKey> n) => n.parent.left == n;

    /* 
     * 找结点(x)的后继结点。即，查找"红黑树中数据值大于该结点"的"最小结点"。
     */
    public RBTreeNode<TKey> Successor(RBTreeNode<TKey> x)
    {
        // 如果x存在右孩子，则"x的后继结点"为 "以其右孩子为根的子树的最小结点"。
        if (x.right != null)
            return DsAndAlg.BstUtils.Min(x.right);

        // 如果x没有右孩子。则x有以下两种可能：
        // (01) x是"一个左孩子"，则"x的后继结点"为 "它的父结点"。
        // (02) x是"一个右孩子"，则查找"x的最低的父结点，并且该父结点要具有左孩子"，找到的这个"最低的父结点"就是"x的后继结点"。
        var y = x.parent;
        while ((y != null) && (x == y.right))
        {
            x = y;
            y = y.parent;
        }

        return y;
    }

    /* 
     * 找结点(x)的前驱结点。即，查找"红黑树中数据值小于该结点"的"最大结点"。
     */
    public RBTreeNode<TKey> Predecessor(RBTreeNode<TKey> x)
    {
        // 如果x存在左孩子，则"x的前驱结点"为 "以其左孩子为根的子树的最大结点"。
        if (x.left != null)
            return DsAndAlg.BstUtils.Max(x.left);

        // 如果x没有左孩子。则x有以下两种可能：
        // (01) x是"一个右孩子"，则"x的前驱结点"为 "它的父结点"。
        // (01) x是"一个左孩子"，则查找"x的最低的父结点，并且该父结点要具有右孩子"，找到的这个"最低的父结点"就是"x的前驱结点"。
        var y = x.parent;
        while ((y != null) && (x == y.left))
        {
            x = y;
            y = y.parent;
        }

        return y;
    }
}
