using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DsAndAlg
{
    public class B23TreeNode
    {
        public bool leaf => references.Length == 0 || references[0] == null;
        public int size = 1;
        public int[] keys = new int[2];
        public B23TreeNode[] references = new B23TreeNode[3];
        public int leftMax => keys[0];
        public int midMax => keys[1];
        public B23TreeNode parent;
        public B23TreeNode leftSible;
        public B23TreeNode rightSible;

        public B23TreeNode(int key)
        {
            keys[0] = key;
            this.size = 1;
            for (int i = 0; i < 3; i++)
            {
                references[i] = null;
            }
            leftSible = null;
            rightSible = null;
        }

        public B23TreeNode(int key, B23TreeNode left, B23TreeNode right) : this(key)
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

        public void NoFullInsert(int elem)
        {
            var tmpLeftMax = leftMax;
            if (elem <= tmpLeftMax)
            {
                keys[0] = elem;
                keys[1] = tmpLeftMax;
            }
            else
            {
                keys[1] = elem;
            }
            size++;
        }

        public B23TreeNode GetNotFullSible()
        {
            if(leftSible!=null && leftSible.size < 2)
            {
                return leftSible;
            }
            if (rightSible != null && rightSible.size < 2)
                return rightSible;
            return null;
        }
    }

    public class B23Tree
    {
        private B23TreeNode root_;

        public void Insert(int key)
        {
            //找点;
            if (root_ == null)
            {
                root_ = new B23TreeNode(key);
            }
            else
            {
                _Insert(root_, key);
            }
        }

        private void _Insert(B23TreeNode node, int key)
        {
            B23TreeNode curr = node;
            while (true)
            {
                int currPos = curr.SearchInNode(key);
                if (currPos < curr.keys.Length && key.Equals(curr.keys[currPos]))
                    return;
                else if (curr.leaf)
                {
                    if (curr.size <2)
                    {
                        //SplitNode(curr, tmpKeys, tmpRefs, an);
                        FullAdd(curr, key);
                    }
                    else
                    {
                        curr.NoFullInsert(key);
                    }
                    return;
                }
                else { 
                    curr = curr.references[currPos];
                }
            }
        }

        private void FullAdd(B23TreeNode node, int key)
        {
            var notFullSible = node.GetNotFullSible();
            if (notFullSible!=null)
            {
                //notFullSible.Add
            }
            else
            {
                //添加到父亲节点   
            }
        }
    }
}
