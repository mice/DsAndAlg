using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DsAndAlg
{
    public static class BstUtils
    {
        public static IntBSTNode SortedArray(int[] array)
        {
            var root = ToTree(array,0,array.Length -1);
            return root;
        }

        private static IntBSTNode ToTree(int[] array, int left, int right)
        {
            if (right<left)
            {
                return null;
            }

            //求中点不要用 int mid = (l + r) / 2，有溢出风险，稳妥的方法是 int mid = l + (r - l) / 2。
            var mid = left + (right - left) / 2;
            var tmpNode = new IntBSTNode(array[mid]);
            tmpNode.left = ToTree(array, left, mid - 1);
            tmpNode.right = ToTree(array, mid + 1, right);
            return tmpNode;
        }
    
        
        public static IntBSTNode Min(IntBSTNode node)
        {
            while (node.left != null)
                node = node.left;
            return node;
        }

        public static IntBSTNode Max(IntBSTNode node)
        {
            while (node.right != null)
                node = node.right;
            return node;
        }
    
    }
}
