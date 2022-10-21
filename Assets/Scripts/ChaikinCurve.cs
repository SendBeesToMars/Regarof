
using UnityEngine;

public class ChaikinCurve {

    // public static Point[] Iterate(Point[] pts, float ratio, int iterations) {

    // }

    public static Point[] Cut(Point[] pts, float ratio, int iterations) {
        if (iterations == 1) {
            return pts;
        }

        float r = Mathf.Clamp(ratio, 0.1f, 0.4f);

        Point[] curve = new Point[pts.Length * 2];

        Vector2 p1;
        Vector2 p2;

        for (int i = 0; i < pts.Length; i++) {
            if (i == pts.Length - 1) {
                p1 = Vector2.Lerp(pts[i].v, pts[0].v, r);
                p2 = Vector2.Lerp(pts[i].v, pts[0].v, 1f - r);
            } else {
                p1 = Vector2.Lerp(pts[i].v, pts[i+1].v, r);
                p2 = Vector2.Lerp(pts[i].v, pts[i+1].v, 1f - r);
            }

            curve[i*2] = new Point((byte)p1.x, (byte)p1.y);
            curve[i*2+1] = new Point((byte)p2.x, (byte)p2.y);
        }

        return Cut(curve, r, iterations - 1);
    }
}