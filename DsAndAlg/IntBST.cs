using System;
using System.Collections.Generic;
using IntBSTNode = BSTNode<int>;

public class BSTNode<T>:IBNode<BSTNode<T>> where T:IComparable<T>
{
    public int key {get; set;}
    public BSTNode<T> left{get; set;}
    public BSTNode<T> right{get; set;}
    public BSTNode()
    {
        this.left = this.right = null;
    }

    public BSTNode(int ele):this(ele,null,null)
    {
        
    }

    public BSTNode(int ele, BSTNode<T> left, BSTNode<T> right){
        this.key = ele;this.left = left;this.right = right;
    }
}


public class IntBST
{
    protected IntBSTNode root;
    public IntBST()
    {
        this.root = null;
    }

    protected void Visit(IntBSTNode p){
        System.Console.WriteLine(p.key);
    }

    public bool Has(int ele)
    {
        if (root == null)
            return false;
        return Search(root, ele) != null;
    }

    private IntBSTNode Search(IntBSTNode p,int ele)
    {
        while(p!=null){
            if(ele == p.key){
                return p;
            }else if(ele<p.key){
                p = p.left;
            }else
                p = p.right;
        }
        return null;
    }

    public int Depth()
    {
        return Depth(root);
    }

    protected int Depth(IntBSTNode root)
    {
        int l, r;
        if (root == null)
            return 0;
        else
        {
            l = Depth(root.left);
            r = Depth(root.right);
            return l > r ? (l + 1) : (r + 1);
        }
    }

    public void breadFirst()
    {
        IntBSTNode p = root;
        Queue<IntBSTNode> queue = new Queue<IntBSTNode>();
        if(p!=null){
            queue.Enqueue(p);
            while(queue.Count>0){
                p = queue.Dequeue();
                Visit(p);
                if(p.left!=null)
                    queue.Enqueue(p.left);
                if(p.right!=null)
                    queue.Enqueue(p.right);
            }
        }
    }

    public void preorder()
    {
        preorder(root);
    }

    public void inorder()
    {
        inorder(root);
    }

    public void postorder(){
        postorder(root);
    }

    protected void postorder(IntBSTNode p){
         if(p!=null){
            postorder(p.left);
            postorder(p.right);
            Visit(p);
        }
    }

    protected void inorder(IntBSTNode p){
         if(p!=null){
            inorder(p.left);
            Visit(p);
            inorder(p.right);
        }
    }

    protected void preorder(IntBSTNode p){
         if(p!=null){
            Visit(p);
            preorder(p.left);
            preorder(p.right);
        }
    }

    public void iterativePreorder()
    {

    }

    public void iterativeInorder()
    {

    }

    public void iterativePostOrder()
    {

    }

    public void MorrisInorder()
    {

    }



    public void Add(int ele)
    {
        IntBSTNode p = root,prev = null;
        while(p!=null){
            prev = p;
            if(p.key<ele)
                p = p.right;
            else
                p = p.left;
        }
        if(root ==null){
            root = new IntBSTNode(ele);
        }else if(prev.key < ele)
            prev.right = new IntBSTNode(ele);
        else
            prev.left = new IntBSTNode(ele);
    }

    /// <summary>
    /// 1. 如果是叶子:直接删除
    /// 2. 如果只有一个节点,可以理解为链表删除
    /// 3. 如果有两个子节点
    /// </summary>
    /// <param name="ele"></param>
    public void deleteByMerging(int ele){
        if (root == null)
            return;
        IntBSTNode p = root, prev = null;
        while(p!=null && p.key != ele)
        {
            prev = p;
            if (p.key < ele)
            {
                p = p.right;
            }
            else
            {
                p = p.left;
            }
        }
        IntBSTNode tmp = null, node = p;
        if(p!=null && p.key == ele)
        {
            if (node.right == null)//重置parent
            {
                node = node.left;
            }else if (node.left == null)//重置parent
            {
                node = node.right;
            }
            else
            {
                tmp = node.left;
                while (tmp.right != null)
                {
                    tmp = tmp.right;
                }
                tmp.right = node.right;

                node = node.left;
            }

            if (p == root)
            {
                root = node;
            }else if(prev.left == p)
            {
                prev.left = node;
            }else
            {
                prev.right = node;
            }
        }else
        {
            Console.WriteLine("Do Not Find");
        }
    }

    public void deleteByCopy(int ele)
    {
        if (root == null)
            return;
        
        IntBSTNode prev = null,node= root, p =  root;
        while (p != null && p.key!=ele)
        {
            prev = p;
            if (p.key > ele)
            {
                p = p.left;
            }
            else
            {
                p = p.right;
            }
        }
        node = p;

        if(p!=null && p.key == ele)
        {
            if (node.right == null)
            {
                node = node.left;
            }else if (node.left == null)
            {
                node = node.right;
            }
            else
            {
                var tmp = node.left;
                var previous = node;
                while (tmp.right != null)
                {
                    previous = tmp;
                    tmp = tmp.right;
                }
                node.key = tmp.key;

                if (previous == node)
                {
                    previous.left = tmp.left;
                }
                else
                    previous.right = tmp.left;
            }
        }
        if (p == root)
        {
            root = node;
        }
        else if (prev.left == p)
        {
            prev.left = node;
        }
        else
            prev.right = node;
    }

    public IntBST(int[] data)
    {
        Balance(data, 0, data.Length);
    }

    public void Balance(int[] data,int first,int last){
        if (first < last)
        {
            var middle = (first + last) / 2;
            Add(data[middle]);
            Balance(data, first, middle - 1);
            Balance(data, middle + 1, last);
        }
    }
}