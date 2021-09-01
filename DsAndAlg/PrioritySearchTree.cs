using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DsAndAlg
{
    public class PrioritySearchTree
    {
        public class PointerPSTNode:IBNode<PointerPSTNode>
        {
            private Vector2 p;
            public float median { get; set; }
            public PointerPSTNode(float x, float y)
            {
                p = new Vector2(x,y);
            }
            public float getX() => p.x;
            public float getY() => p.y;
            public Vector2 getPoint() { return p; }

            public PointerPSTNode left { get; set; }
            public PointerPSTNode right { get; set; }
        }

        private PointerPSTNode _root;
        public PointerPSTNode root => _root;

        public PrioritySearchTree(List<Vector2> points)
        {
            if (points == null) return;
            points.Sort(_sortYandX); // Sort by y-coordinate in decreasing order
            _root = buildTree(points);
        }

        private int _sortYandX(Vector2 p1, Vector2 p2)
        {
            if (p1.y != p2.y)
                return (int)Mathf.Sign(p2.y - p1.y);
            if (p1.x != p2.x)
                return (int)Mathf.Sign(p1.x- p2.x );
            return 0;
        }

        private float GetMedian(List<Vector2> points,int start,int end)
        {
            float sum = 0.0f;
            for (int i = start; i < end; i++)
            {
                sum += points[i].x;
            }
            return sum / (end - start);
        }

        // Assumes all points are valid (e.g. not null)
        private PointerPSTNode buildTree(List<Vector2> points)
        {
            if (points == null || points.Count < 1) return null;
            int start = 0;
            // Find point with highest Y value
            Vector2 rootPoint = points[start];
            PointerPSTNode root;
            root = new PrioritySearchTree.PointerPSTNode(rootPoint.x, rootPoint.y);
            if (points.Count == 1)
            {
                root.median = rootPoint.x;
                return root;
            }

            // Find median X value
            var medianX = GetMedian(points, 1, points.Count);// 
            root.median = medianX;
            var left = new List<Vector2>();
            var right = new List<Vector2>();
            SplitList(points, 1, medianX, left, right);
            // Make upper and lower point array
            root.left = buildTree(left);
            root.right = buildTree(right);
            return root;
        }

        private void  SplitList(List<Vector2> points,int start ,float medianX, List<Vector2> left, List<Vector2> right)
        {
            for (int i = start; i < points.Count; i++)
            {
                if (points[i].x <= medianX)
                {
                    left.Add(points[i]);
                }
                else
                {
                    right.Add(points[i]);
                }
            }
        }

        public void Insert(Vector2 p)
        {

        }

        public void Remove(Vector2 p)
        {

        }
    }
}
