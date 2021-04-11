using System;
using System.Collections.Generic;

public class IntBSTNode
{
    public int key {get; set;}
    public IntBSTNode left{get; set;}
    public IntBSTNode right{get; set;}
    public IntBSTNode()
    {
        this.left = this.right = null;
    }

    public IntBSTNode(int ele):this(ele,null,null)
    {
        
    }

    public IntBSTNode(int ele,IntBSTNode left,IntBSTNode right){
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

    public IntBSTNode Search(IntBSTNode p,int ele)
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
    /// 1. �����Ҷ��:ֱ��ɾ��
    /// 2. ���ֻ��һ���ڵ�,�������Ϊ����ɾ��
    /// 3. ����������ӽڵ�
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
            if (node.right == null)//����parent
            {
                node = node.left;
            }else if (node.left == null)//����parent
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