using System;
using UnityEngine;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography.X509Certificates;
// using System.Collections.Generic;
// using System.Linq;

public struct Point {

    public Point(byte _x, byte _y) {
        x = _x;
        y = _y;
    }

    public Vector2 v => new Vector2(x, y);

    public readonly byte x, y;
    public readonly int CompareTo(Point p) => (this.x == p.x) ? this.y - p.y : this.x - p.x;
    public readonly override string ToString() => $"({x}, {y})";
}

public class ConvexHull {

    public static long Cross(Point o, Point a, Point b) {
        return (a.x - o.x) * (long) (b.y - o.y) - (a.y - o.y) * (long) (b.x - o.x);
    }

    public static Point[] MonotoneChain(Point[] pts) {

        if (pts.Length > 1) {
            int n = pts.Length, k = 0;
            Point[] hull = new Point[2 * n];

            Array.Sort(pts, (Point a, Point b) => a.CompareTo(b));
            // Arrays.sort(pts);

            // Build lower hull
            for (int i = 0; i < n; ++i) {
                while (k >= 2 && Cross(hull[k - 2], hull[k - 1], pts[i]) <= 0)
                    k--;
                hull[k++] = pts[i];
            }

            // Build upper hull
            for (int i = n - 2, t = k + 1; i >= 0; i--) {
                while (k >= t && Cross(hull[k - 2], hull[k - 1], pts[i]) <= 0)
                    k--;
                hull[k++] = pts[i];
            }
            if (k > 1) {
                hull = copyOfRange(hull, 0, k - 1); // remove non-hull vertices after k; remove k - 1 which is a duplicate
            }
            return hull;
        } else if (pts.Length <= 1) {
            return pts;
        } else {
            return null;
        }
    }

    // public static Point[] GrahamScan(Point[] pts) {
    //     List<Point> points = pts.ToList<Point>();
    //     Stack<Point> stack = new Stack<Point>();

    //     points.ForEach (point => {
    //         while (stack.Count > 1 && turn(stack.Peek(), point, peek2(stack)) <= 0) {
    //             stack.Pop();
    //         }
    //         stack.Push(point);
    //     });

    //     Point[] hull = new Point[stack.Count];
    //     stack.CopyTo(hull, 0);

    //     return hull;
    // }

    // private static int turn(Point p1, Point p2, Point p3) {
    //     return (p2.x - p1.x) * (p3.y - p1.y) - (p2.y - p1.y) * (p3.x - p1.x);
    // }

    // private static Point peek2(Stack<Point> stack) {
    //     Point top = stack.Pop();
    //     Point peak = stack.Peek();
    //     stack.Push(top);
    //     return peak;
    // }

    private static Point[] copyOfRange(Point[] src, int start, int end) {
        int length = end - start;
        Point[] dest = new Point[length];
        Array.Copy(src, start, dest, 0, length);
        return dest;
    }
}
