using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DsAndAlg
{
    public class BTreeNode
    {
        public const int m = 4;
        public bool leaf => references.Length==0 || references[0] == null;
        public int size = 1;
        public int[] keys = new int[m - 1];
        public BTreeNode[] references = new BTreeNode[m];

        public BTreeNode(int key)
        {
            keys[0] = key;
            this.size = 1;
            for (int i = 0; i < m; i++)
            {
                references[i] = null;
            }
        }

        public BTreeNode(int[] keys_,BTreeNode[] references_, int start,int end)
        {
            this.references = new BTreeNode[m];
            this.keys = new int[m - 1];
            this.size = 0;
            for (int i = start; i < end; i++)
            {
                
                this.references[size] = references_[i];
                this.keys[size] = keys_[i];
                this.size++;
            }
            this.references[size] = references_[end];
        }

        public BTreeNode(int key, BTreeNode left, BTreeNode right) : this(key)
        {
            this.size = 1;
            this.keys[0] = key;
            this.references[0] = left;
            this.references[1] = right;
        }

        public int SearchInNode(int target)
        {
            int l = 0, r = size - 1;
            while (l <= r)
            {
                int m = (l + r) / 2;
                
                if (keys[m] == target)
                    return m;
                else if (target < keys[m])
                    r = m - 1;
                else
                    l = m + 1;
            }
            return l;
        }

        public (int[], BTreeNode[]) InsertInNode(int elem, BTreeNode left,BTreeNode right,int ins)
        {
            if(this.size == references.Length - 1)
            {
                var tmpKeys = new int[m];
                var tmpRef = new BTreeNode[m + 1];
                Array.Copy(keys, tmpKeys, keys.Length);
                Array.Copy(references, tmpRef, references.Length);
                NoFullInsert(elem, tmpKeys, tmpRef, left, right, ins);
                return (tmpKeys, tmpRef);
            }
            else
            {
                NoFullInsert(elem, keys, references, left, right, ins);
                return (keys, references);
            }
        }

        private void NoFullInsert(int elem,int[] keys, BTreeNode[] references, BTreeNode left, BTreeNode right, int ins)
        {
            for (int i = size; i > ins; i--)
            {
                keys[i] = keys[i - 1];
                references[i + 1] = references[i];
            }

            size++;
            keys[ins] = elem;
            references[ins] = left;
            references[ins + 1] = right;
        }
    }

    public class BTree
    {
        private BTreeNode root_;
        private int NodeCount = 4;


        public void Insert(int key)
        {
            //找点;
            if (root_ == null)
            {
                root_ = new BTreeNode(key);
            }
            else
            {
                _Insert(root_, key);
            }
        }

        private void _Insert(BTreeNode node ,int key)
        {
            BTreeNode curr = node;
            Stack<(int,BTreeNode)> an = new Stack<(int, BTreeNode)>();
            while (true)
            {
                int currPos = curr.SearchInNode(key);
                if (currPos < curr.keys.Length && key.Equals(curr.keys[currPos]))
                    return;
                else if (curr.leaf)
                {
                    (var tmpKeys,var tmpRefs)=curr.InsertInNode(key, null, null, currPos);
                    if (curr.size == NodeCount)
                    {
                        SplitNode(curr, tmpKeys, tmpRefs, an);
                    }
                    return;
                }
                else
                {
                    an.Push((currPos, curr));
                    curr = curr.references[currPos];
                }
            }
        }

        private void SplitNode(BTreeNode node, int[] keys,BTreeNode[] references ,Stack<(int, BTreeNode)> an)
        {
            int medPos = (node.size) / 2;
            var med = node.keys[medPos];

            var left = new BTreeNode(keys, references, 0, medPos);
            var right = new BTreeNode(keys, references, medPos + 1, node.size);
           
            if (node == root_)
            {
                root_ = new BTreeNode(med,left,right);
            }
            else
            {
                (var parentIns, var parent) = an.Pop();
                (var tmpKeys, var tmpRefs) = parent.InsertInNode(med, left, right, parentIns);
                if (parent.size == NodeCount)
                {
                    SplitNode(parent, tmpKeys, tmpRefs, an);
                }
            }
        }

        public BTreeNode Find(int key)
        {
            if (root_ == null)
                return null;
            return Search(root_, key);
        }

        private BTreeNode Search(BTreeNode node,int key)
        {
            var curr = node;
            while (true)
            {
                int currPos = curr.SearchInNode(key);
                if (key.Equals(curr.keys[currPos]))
                    return curr;
                else if (curr.leaf)
                {
                    return null;
                }
                else
                {
                    curr = curr.references[currPos];
                }
            }
        }

        private BTreeNode _Search(BTreeNode node,int key)
        {
            // Find the first key greater than or equal to k
            int i = 0;
            var n = node.size;
            var keys = node.keys;
            while (i < n && key > keys[i])
                i++;

            // If the found key is equal to k, return this node
            if (i< keys.Length && keys[i] == key)
                return node;

            // If the key is not found here and this is a leaf node
            if (node.leaf == true)
                return null;

            // Go to the appropriate child
            return Search(node.references[i], key);
        }
    
    
    }
}
