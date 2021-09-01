using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DsAndAlg
{

    public class BPTreeNode
    {
        public const int m = 4;
        public bool leaf => references.Length == 0 || references[0] == null;
        public int size = 1;
        public int[] keys = new int[m - 1];
        public BPTreeNode[] references = new BPTreeNode[m];
        public BPTreeNode parent;
        public BPTreeNode leftSible;
        public BPTreeNode rightSible;

        public BPTreeNode(int key)
        {
            keys[0] = key;
            this.size = 1;
            for (int i = 0; i < m; i++)
            {
                references[i] = null;
            }
            leftSible = null;
            rightSible = null;
        }

        public BPTreeNode(int[] keys_, BPTreeNode[] references_, int start, int end)
        {
            this.references = new BPTreeNode[m];
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

        public BPTreeNode(int key, BPTreeNode left, BPTreeNode right) : this(key)
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

        public (int[], BPTreeNode[]) InsertInNode(int elem, BPTreeNode left, BPTreeNode right, int ins)
        {
            if (this.size == references.Length - 1)
            {
                var tmpKeys = new int[m];
                var tmpRef = new BPTreeNode[m + 1];
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

        private void NoFullInsert(int elem, int[] keys, BPTreeNode[] references, BPTreeNode left, BPTreeNode right, int ins)
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
    public class BPTree
    {
        private BPTreeNode root_;
        private int NodeCount = 4;


        public void Insert(int key)
        {
            //找点;
            if (root_ == null)
            {
                root_ = new BPTreeNode(key);
            }
            else
            {
                _Insert(root_, key);
            }
        }

        /**插入算法:
        1. 叶节点未满,直接插入
        2. 叶节点已满,左邻居如果有空,给左边邻居一个,如果右邻居未满,给右邻居,注意更新父节点的值.
        3. 如果左右邻居都满.分裂给父节点一个元素,如果父节点满,父节点分裂.
         */
        private void _Insert(BPTreeNode node, int key)
        {

        }
    }
}
