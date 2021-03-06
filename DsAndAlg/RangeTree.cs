
/******************************************************************************
 *  Compilation:  javac RangeTree.java
 *  Execution:    java RangeTree
 *  
 *  Range tree implemented using binary search trees.
 *
 *  Assumes no two points have the same x or y coordinate!!!!
 *
 *  Could be made more efficient by assuming points are given
 *  all at once. Then could sort by x and by y to ensure tree
 *  are perfectly balanced.
 * 
 ******************************************************************************/
using System;
using System.Collections.Generic;
using DsAndAlg;
using boolean = System.Boolean;

public class RangeTree<Key> 
    where Key: struct, System.IComparable<Key>, System.IEquatable<Key>
{

    private Node root;   // root of the primary BST

    // BST helper node data type
    class Node {
        public Key x, y;                   // x- and y- coordinates
        public Node left, right;           // left and right subtrees
        public RangeSearch<Key, Key> bst;  // secondary BST

        public Node(Key x, Key y) {
            this.x   = x;
            this.y   = y;
            this.bst = new RangeSearch<Key, Key>();
            this.bst.put(y, x);
        }
    }


  /***********************************************************************
    *  Insert (x, y) into range tree.
    *  Sort by x in primary BST; sort by y in secondary BST.
    ***************************************************************************/
    public void insert(Key x, Key y) {
        root = insert(root, x, y);
    }

    private Node insert(Node h, Key x, Key y) {
        if (h == null) return new Node(x, y);
        h.bst.put(y, x);
        if (less(x, h.x)) h.left  = insert(h.left,  x, y);
        else              h.right = insert(h.right, x, y);
        return h;
    }


   /***************************************************************************
    *  Range searching.
    ***************************************************************************/


    // print all points in range
    public void query2D(Interval2D<Key> rect) {
        Interval<Key> intervalX = rect.intervalX;
        // find splitting node h where h.x is in the x-interval
        Node h = root;
        while (h != null && !intervalX.contains(h.x)) {
            if      (less(intervalX.max, h.x)) h = h.left;
            else if (less(h.x, intervalX.min)) h = h.right;
        }
        if (h == null) return;

        if (rect.contains(h.x, h.y)) Console.WriteLine("A: " + h.x + ", " + h.y);

        queryL(h.left,  rect);
        queryR(h.right, rect);
    }

    // find all keys >= xmin in subtree rooted at h
    private void queryL(Node h, Interval2D<Key> rect) {
        if (h == null) return;
        if (rect.contains(h.x, h.y)) Console.WriteLine("B: " + h.x + ", " + h.y);
        if (!less(h.x, rect.intervalX.min)) {
            enumerate(h.right, rect);
            queryL(h.left, rect);
        }
        else {
            queryL(h.right, rect);
        }
    }

    // find all keys <= xmax in subtree rooted at h
    private void queryR(Node h, Interval2D<Key> rect) {
        if (h == null) return;
        if (rect.contains(h.x, h.y)) Console.WriteLine("C: " + h.x + ", " + h.y);
        if (!less(rect.intervalX.max, h.x)) {
            enumerate(h.left, rect);
            queryR(h.right, rect);
        }
        else {
            queryR(h.left, rect);
        }
    }

    // precondition: subtree rooted at h has keys between xmin and xmax
    private void enumerate(Node h, Interval2D<Key> rect) {
        if (h == null) return;
        Console.WriteLine("integrity: " + h.bst.check());
        IEnumerable<Key> list = h.bst.range(rect.intervalY);
        foreach(Key y in list) {
            Key? x = h.bst.get(y);
            Console.WriteLine("D: " + x + ", " + y);
        }
        Console.WriteLine("-");
    }


   /***************************************************************************
    *  Helper comparison functions.
    ***************************************************************************/

    private boolean less(Key k1, Key k2) {
        return k1.CompareTo(k2) < 0;
    }


   /***************************************************************************
    *  Test client.
    ***************************************************************************/
    //public static void main(String[] args) {
    //    int M = Integer.parseInt(args[0]);   // queries
    //    int N = Integer.parseInt(args[1]);   // points

    //    RangeTree<Integer> st = new RangeTree<Integer>();

    //    // insert N random points in the unit square
    //    for (int i = 0; i < N; i++) {
    //        int x = StdRandom.uniform(100);
    //        int y = StdRandom.uniform(100);
    //        StdOut.println("(" + x + ", " + y + ")");
    //        st.insert(x, y);
    //    }
    //    StdOut.println("Done preprocessing " + N + " points");

    //    // do some range searches
    //    for (int i = 0; i < M; i++) {
    //        int xmin = StdRandom.uniform(100);
    //        int ymin = StdRandom.uniform(100);;
    //        int xmax = xmin + StdRandom.uniform(10);
    //        int ymax = ymin + StdRandom.uniform(20);
    //        Interval<Integer> intX = new Interval<Integer>(xmin, xmax);
    //        Interval<Integer> intY = new Interval<Integer>(ymin, ymax);
    //        Interval2D<Integer> rect = new Interval2D<Integer>(intX, intY);
    //        StdOut.println(rect);
    //        st.query2D(rect);
    //    }
    //}

}
