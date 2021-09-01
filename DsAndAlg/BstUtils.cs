using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntBSTNode = BSTNode<int>;

public interface IFormat<T>
{
    void Format(T t);
}

public interface IBNode<T>
{
    T left { get; set; }
    T right { get; set; }
}

namespace DsAndAlg
{

    public static class BstUtils
    {
        public static int BinarySearch<T>(List<T> sources,T target) where T:IComparable<T>
        {
            return __BinarySearch(sources, Comparer<T>.Default, target, 0, sources.Count-1);
        }

        public static int BinarySearch<T>(List<T> sources,System.Func<T,int> compare)
        {
            return __BinarySearch(sources, compare, 0, sources.Count - 1);
        }

        private static int __BinarySearch<T>(List<T> sources, System.Func<T, int> compare, int start, int end)
        {
            while (start <= end)
            {
                var medIndex = start + (end - start) / 2;
                var med = sources[medIndex];
                var compValue = compare.Invoke(med);
                if (compValue < 0)
                {
                    end = medIndex - 1;
                }
                else if (compValue > 0)
                {
                    start = medIndex + 1;
                }
                else
                    return medIndex;
            }
            return start;
        }

        private static int __BinarySearch<T>(List<T> sources, IComparer<T> compare, T target, int start, int end)
        {
            while (start <= end)
            {
                var medIndex = start + (end - start) / 2;
                var med = sources[medIndex];
                var compValue = compare.Compare(target, med);
                if (compValue < 0)
                {
                    end = medIndex - 1;
                }
                else if (compValue > 0)
                {
                    start = medIndex + 1;
                }
                else
                    return medIndex;
            }
            return start;
        }

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


        public static RBTree<T> SortedArray<T>(T[] array) where T : IComparable<T>
        {
            var root = ToRBTree(array, 0, array.Length - 1);
            return new RBTree<T>(root);
        }

        private static RBTree<T>.RBTreeNode<T> ToRBTree<T>(T[] array, int left, int right) where T : IComparable<T>
        {
            if (right < left)
            {
                return null;
            }

            //求中点不要用 int mid = (l + r) / 2，有溢出风险，稳妥的方法是 int mid = l + (r - l) / 2。
            var mid = left + (right - left) / 2;
            var tmpNode = new RBTree<T>.RBTreeNode<T>();
            tmpNode.value = array[mid];
            tmpNode.left = ToRBTree(array, left, mid - 1);
            tmpNode.right = ToRBTree(array, mid + 1, right);
            tmpNode.color = (tmpNode.left == null && tmpNode.right ==null) ? NodeColor.Red : NodeColor.Black;
            if (tmpNode.left!=null)
                tmpNode.left.parent = tmpNode;
            if (tmpNode.right != null)
                tmpNode.right.parent = tmpNode;

            return tmpNode;
        }


        public static bool VolateRRRule<T>(RBTree<T> tree) where T:IComparable<T>
        {
            var p = tree.root;
            bool hasRR = false;
            StringBuilder sb = new StringBuilder();
            Iterate<RBTree<T>.RBTreeNode<T>>(p, (node) =>
            {
                sb.Append(node.ToString());
                sb.Append(",");
                if (node!=null && node.parent != null)
                {
                    if (node.color== NodeColor.Red && node.parent.color == NodeColor.Red)
                    {
                        hasRR = true;
                    }
                }
            });
            Console.WriteLine(sb.ToString());
            return hasRR;
        }

        public static bool VolateBlackCountRule<T>(RBTree<T> tree) where T : IComparable<T>
        {
            var tmpArray = new List<RBTree<T>.RBTreeNode<T>>(100);
            bool sameCount = true;
            List<int> tmpList = new List<int>();
            var sb = new StringBuilder();
            sb.Append("Start::");
            LeafToRoot(tree.root, tmpArray, 0, (tmpArra) =>
            {
                var tmpBCount = 0;
                sb.Append("Begin:");
               
                for (int i = 0; i < tmpArra.Count; i++)
                {
                    sb.Append(tmpArra[i]+",");
                    if (tmpArra[i].color == NodeColor.Black)
                    {
                        tmpBCount = tmpBCount + 1;
                    }
                }
                sb.Append("END:\n");
                if (tmpList.Count>0 && tmpList[tmpList.Count - 1] != tmpBCount)
                {
                    sameCount = false;
                }
                tmpList.Add(tmpBCount);
            });
            sb.Append("Content:\n");
            sb.AppendLine(string.Join(",", tmpList.ToArray()));
            sb.AppendLine("END::");
            Console.WriteLine(sb.ToString());
            return !sameCount;
        }
    

        public static T Min<T>(T node) where T : IBNode<T>
        {
            while (node.left != null)
                node = node.left;
            return node;
        }

        public static T Max<T>(T node) where T : IBNode<T>
        {
            while (node.right != null)
                node = node.right;
            return node;
        }

        private static void Iterate<T>(T p,System.Action<T> func) where T:IBNode<T>
        {
            Queue<T> queue = new Queue<T>();
            if (p != null)
            {
                queue.Enqueue(p);
                while (queue.Count > 0)
                {
                    p = queue.Dequeue();
                    func(p);
                    if (p.left != null)
                        queue.Enqueue(p.left);
                    if (p.right != null)
                        queue.Enqueue(p.right);
                }
            }
        }

        private static void IteratePreOrder<T>(T p, System.Action<T> func) where T : IBNode<T>
        {
            Stack<T> queue = new Stack<T>();
            if (p != null)
            {
                queue.Push(p);
            }
        }

        private static void LeafToRoot<T>(T p, List<T> path,int len,System.Action<List<T>> endCall) where T : IBNode<T>
        {
            if (p!=null)
            {
                path.Add(p);
                if (p.left == null && p.right == null)//当T为叶子结点时，逆序输出 
                {
                    endCall(path);
                }
                else//当不为终端结点时，该节点对应的值进入数组 
                {
                   
                    LeafToRoot(p.left, path, len, endCall);
                    LeafToRoot(p.right, path, len, endCall);
                }
                path.RemoveAt(path.Count - 1);
            } 
        }
    }
}
