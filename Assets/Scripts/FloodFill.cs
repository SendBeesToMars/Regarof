
using System;
using System.Collections;
using System.Data.Common;
using UnityEditor;
using UnityEngine;

public class FloodFill {


    public static byte[,] Flood(byte[,] map, Point[] hull, (byte x, byte y) origin) {
        outline(map, hull);

        if (isInside(origin, hull)) {
            floodFill(map, origin);
        }

        return map;
    }

    private static void floodFill(byte[,] map, (byte x, byte y) origin) {
        byte w = (byte) map.GetLength(0);
        byte h = (byte) map.GetLength(1);

        Queue q = new Queue();
        q.Enqueue(origin);

        while (q.Count != 0) {
            (byte x, byte y) p = ((byte x, byte y)) q.Dequeue();

            if (p.x < 0 || p.x >= w || p.y < 0 || p.y >= h || map[p.x,p.y] == 1)
                continue;

            map[p.x,p.y] = 1;

            q.Enqueue(((byte)(p.x+1), (byte)(p.y)));
            q.Enqueue(((byte)(p.x-1), (byte)(p.y)));
            q.Enqueue(((byte)(p.x), (byte)(p.y+1)));
            q.Enqueue(((byte)(p.x), (byte)(p.y-1)));
        }
    }

    private static byte[,] outline(byte[,] map, Point[] hull) {

        for (int i = 0; i < hull.Length; i++) {
            if (i == hull.Length - 1) {
                line(map, hull[i], hull[0]);
            } else {
                line(map, hull[i], hull[i+1]);
            }
        }

        return map;
    }

    private static void line(byte[,] map, Point p1, Point p2) {
        if (Mathf.Abs(p2.y - p1.y) < Mathf.Abs(p2.x - p1.x)) {
            if (p1.x > p2.x)
                lineLow(map, p2, p1);
            else
                lineLow(map, p1, p2);
        } else {
            if (p1.y > p2.y)
                lineHigh(map, p2, p1);
            else
                lineHigh(map, p1, p2);
        }
    }

    private static void lineLow(byte[,] map, Point p1, Point p2) {
        short dx = (short)(p2.x - p1.x);
        short dy = (short)(p2.y - p1.y);
        short yi = 1;

        if (dy < 0) {
            yi = -1;
            dy = (short) -dy;
        }

        short D = (short) (2 * dy - dx);
        short y = p1.y;

        for (short x = p1.x; x <= p2.x; x++) {
            map[x,y] = 1;
            
            if (D > 0) {
                y = (short) (y + yi);
                D = (short) (D + 2 * (dy - dx));
            } else {
                D = (short) (D + 2 * dy);
            }
        }
    }

    private static void lineHigh(byte[,] map, Point p1, Point p2) {
        short dx = (short)(p2.x - p1.x);
        short dy = (short)(p2.y - p1.y);
        short xi = 1;

        if (dx < 0) {
            xi = -1;
            dx = (short) -dx;
        }

        short D = (short) (2 * dx - dy);
        short x = p1.x;

        for (short y = p1.y; y <= p2.y; y++) {
            map[x,y] = 1;
            
            if (D > 0) {
                x = (short) (x + xi);
                D = (short) (D + 2 * (dx - dy));
            } else {
                D = (short) (D + 2 * dx);
            }
        }
    }

    private static bool isInside((byte x, byte y) p, Point[] pts) {
        int num = pts.Length;
        int j = num - 1;
        bool c = false;

        for (int i = 0; i < num; i++) {
            if (p.x == pts[i].x && p.y == pts[i].y) {
                // # point is a corner
                return false;
            }

            if ((pts[i].y > p.y) != (pts[j].y > p.y)) {
                float slope = (p.x-pts[i].x)*(pts[j].y-pts[i].y)-(pts[j].x-pts[i].x)*(p.y-pts[i].y);
                if (slope == 0) {
                    // # point is on boundary
                    return false;
                }
                if ((slope < 0) != (pts[j].y < pts[i].y)) {
                    c = !c;
                }
            }

            j = i;
        }
        return c;
    }
}