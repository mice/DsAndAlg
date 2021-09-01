﻿
/******************************************************************************
*  Compilation:  javac Interval.java
*  Execution:    java Interval
*
*  Implementation of an interval.
*
******************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RuntimeException = System.Exception;
using boolean = System.Boolean;

namespace DsAndAlg
{
    public class Interval<Key> where Key:IComparable<Key>,IEquatable<Key>
    { 
        public Key min { get; private set; }  // min endpoint
        public Key max { get; private set; } // max endpoint

        public Interval(Key min, Key max)
        {
            if (less(max, min)) throw new RuntimeException("Illegal argument");
            this.min = min;
            this.max = max;
        }

        // is x between min and max
        public boolean contains(Key x)
        {
            return !less(x, min) && !less(max, x);
        }

        // does this interval a intersect interval b?
        public boolean intersects(Interval<Key> b)
        {
            Interval<Key> a = this;
            if (less(a.max, b.min)) return false;
            if (less(b.max, a.min)) return false;
            return true;
        }

        // does this interval a equal interval b?
        public boolean equals(Interval<Key> b)
        {
            Interval<Key> a = this;
            return a.min.Equals(b.min) && a.max.Equals(b.max);
        }


        // comparison helper functions
        private boolean less(Key x, Key y)
        {
            return x.CompareTo(y) < 0;
        }

        // return string representation
        public override String ToString()
        {
            return "[" + min + ", " + max + "]";
        }


        // test client
        //public static void main(String[] args)
        //{
        //    int n = Integer.parseInt(args[0]);

        //    Interval<Integer> a = new Interval<Integer>(5, 17);
        //    Interval<Integer> b = new Interval<Integer>(5, 17);
        //    Interval<Integer> c = new Interval<Integer>(5, 18);
        //    System.out.println(a.equals(b));
        //    System.out.println(!a.equals(c));
        //    System.out.println(!b.equals(c));


        //    // generate n random points in [-1, 2] and compute
        //    // fraction that lies in [0, 1]
        //    Interval<Double> interval = new Interval<Double>(0.0, 1.0);
        //    int count = 0;
        //    for (int i = 0; i < n; i++)
        //    {
        //        double x = 3 * Math.random() - 1.0;
        //        if (interval.contains(x)) count++;
        //    }
        //    System.out.println("fraction = " + (1.0 * count / n));
        //}
    }
}
