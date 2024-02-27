using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using System;

public class IslandGenerator : MonoBehaviour {

    private struct point {
        public int x;
        public int y;

        public point(int x, int y) {
            this.x = x;
            this.y = y;
        }
    };

    private int i0;
    private point p0;

    public const int ISLAND_SIZE = 64;

    [Range(0, int.MaxValue)]
    public int seed = 0;

    public IslandGenerator() {

    }

    public byte[,] GenerateIsland() {
        byte[,] island = new byte[ISLAND_SIZE,ISLAND_SIZE];

        point[] points = generatePoints(10, 10);
        Stack<point> stack = new();

        foreach (point p in points) {
            island[p.x, p.y] = 1;
        }

        p0 = findFirstPoint(points);
        Array.Sort(points, comparePoints);

        island[points[0].x, points[0].y] = 4;



        foreach (point p in points) {
            while (stack.Count > 1 && ccw(nextToTop(stack), top(stack), p) <= 0) {
                stack.Pop();
            }

            stack.Push(p);
        }

        point p3 = top(stack);
        while(stack.Count > 1) {
            point p2 = nextToTop(stack);
            point p1 = stack.Pop();
            Debug.Log($"{p1.x}, {p1.y}");
            Debug.Log($"{p2.x}, {p2.y}");
            line(p1, p2, island);

            if(stack.Count == 1) line(p2, p3, island);
        }

        // for (int i = 1; i < points.Length; i++) {
        //     line(points[i-1], points[i], island);
        // }

        return island;
    }

    private point top(Stack<point> s) {
        point top = s.Pop();
        s.Push(top);

        return top;
    }

    private point nextToTop(Stack<point> s) {
        point top = s.Pop();
        point nextToTop = s.Pop();
        s.Push(nextToTop);
        s.Push(top);

        return nextToTop;
    }

    private point[] generatePoints(int numPoints, int offset) {
        int s = seed;

        point[] points = new point[numPoints];
        for (int i= 0; i < numPoints; i++) {
            UnityEngine.Random.InitState(s);
            points[i] = new point(UnityEngine.Random.Range(offset, ISLAND_SIZE-offset), UnityEngine.Random.Range(offset, ISLAND_SIZE-offset));
            s++;
        }
        return points;
    }

    private point findFirstPoint(point[] points) {
        point p0 = new point(ISLAND_SIZE, ISLAND_SIZE);

        foreach (point p in points) {
            if (p.y < p0.y)
                p0 = p;
            else if (p.y == p0.y && p.x < p0.x)
                p0 = p;
        }

        return p0;
    }

    // private int findFirstPointIndex(point[] points) {
    //     int index = 0;

    //     for (int i = 0; i < points.Length; i++) {
    //         if (points[i].y < points[index].y)
    //             index = i;
    //         else if (points[i].y == points[index].y && points[i].x < points[index].x)
    //             index = i;
    //     }

    //     return index;
    // }


    static double ccw(point p1, point p2, point p3) {
        double c1 = (p2.x - p1.x) * (p3.y - p1.y);
        double c2 = (p2.y - p1.y) * (p3.x - p1.x);
        if (c1 == c2)
            return 0;
        return c1 - c2;
    }

    private void swap(ref point p1, ref point p2) { 
        point temp = p1; 
        p1 = p2; 
        p2 = temp; 
    } 

    private int comparePoints(point p1, point p2) {
        double a1 = Math.Atan2(p1.y - p0.y, p1.x - p0.x) * 180 / Math.PI;
        double a2 = Math.Atan2(p2.y - p0.y, p2.x - p0.x) * 180 / Math.PI;
        if (a1 < a2)
            return -1;
        return a1 > a2 ? 1 : 0;
    }

    private void line(point p1, point p2, byte[,] island) {
        int dx = Math.Abs(p2.x - p1.x);
        int sx = p1.x < p2.x ? 1 : -1;
        int dy = -Math.Abs(p2.y - p1.y);
        int sy = p1.y < p2.y ? 1 : -1;
        int error = dx + dy;

        while(true) {
            if(island[p1.x, p1.y] < 1) island[p1.x, p1.y] = 5;

            if(p1.x == p2.x && p1.y == p2.y) break;
            int e2 = 2 * error;

            if(e2 >= dy) {
                if(p1.x == p2.x) break;
                error += dy;
                p1.x += sx;
            }

            if(e2 <= dx) {
                if(p1.y == p2.y) break;
                error += dx;
                p1.y += sy;
            }
        }
    }
}

