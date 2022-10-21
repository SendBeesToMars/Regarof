using System;
using System.Security.Cryptography;
using System.Text;

using UnityEngine;

public class MapGenerator : MonoBehaviour {
    

    public bool debug = false;

    public string seed = "qwer";

    [Range(12, 128)]
    public byte width, height;

    [Range(24,96)]
    public byte points;

    [Range(0.1f,0.4f)]
    public float lerp;

    [Range(1,6)]
    public int iterations;

    private System.Random random;

    private (byte w, byte h) d;
    private byte[,] map;
    private Point[] hull;

    void Start() {
        using (SHA256 sha256 = SHA256.Create()) {
            uint hash = getHash(sha256, seed);
            random = new System.Random((int) (hash - int.MaxValue));
        }

        // GenerateMap();
    }

    // void Update() {
    //     if (Input.GetMouseButtonDown(0)) {
    //         GenerateMap();
    //     }
    // }

    public byte[,] GenerateMap() {
        d = (width, height);
        Point[] pts = randomPoints(random, points);
        return randomMap(pts);
    }

    private byte[,] randomMap(Point[] a) {
        byte[,] m = new byte[d.w, d.h];
        
        Point[] pts = ConvexHull.MonotoneChain(a);
        hull = ChaikinCurve.Cut(pts, lerp, iterations);
        m = FloodFill.Flood(m, hull, ((byte)(width/2), (byte)(height/2)));

        if (debug) {
            for (int i = 0; i < a.Length; i++) {
                m[a[i].x, a[i].y] = 8;
            }

            for (int i = 0; i < pts.Length; i++) {
                m[pts[i].x, pts[i].y] = 9;
            }
        }

        return m;
    }    

    private Point[] randomPoints(System.Random r, int count) {
        Point[] pts = new Point[count];
        for (int i = 0; i < count; i++) {
            pts[i] = getRandomPoint(r, 2);
        }

        return pts;
    }

    private Point getRandomPoint(System.Random r, int inset) {
        return new Point((byte)r.Next(inset,d.w-inset), (byte)r.Next(inset,d.h-inset));
    }

    void OnDrawGizmos() {
        if (map != null) {
            for (int x = 0; x < d.w; x++) {
                for (int y = 0; y < d.h; y++) {
                    Gizmos.color = (map[x,y] == 1) ? Color.black : Color.white;
                    Gizmos.color = (map[x,y] == 8) ? Color.red : Gizmos.color;
                    Gizmos.color = (map[x,y] == 9) ? Color.blue : Gizmos.color;
                    UnityEngine.Vector3 pos = new UnityEngine.Vector3(-d.w/2 + x + 0.5f, -d.h/2 + y + 0.5f, 0);
                    Gizmos.DrawCube(pos, UnityEngine.Vector3.one);
                }
            }
            
            if (debug) {
                Gizmos.color = Color.green;
                for (int i = 0; i < hull.Length; i++) {
                    Vector3 p1 = new Vector3(-d.w/2 + hull[i].x + 0.5f, -d.h/2 + hull[i].y + 0.5f, 0);
                    Vector3 p2;
                    if (i < hull.Length - 1) {
                        p2 = new Vector3(-d.w/2 + hull[i+1].x + 0.5f, -d.h/2 + hull[i+1].y + 0.5f, 0);
                    } else {
                        p2 = new Vector3(-d.w/2 + hull[0].x + 0.5f, -d.h/2 + hull[0].y + 0.5f, 0);
                    }
                    Gizmos.DrawLine(p1, p2);
                }
            }
        }
    }
    
    private static uint getHash(HashAlgorithm hashAlgorithm, string input) {
        byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));
        return BitConverter.ToUInt32(data);
    }

    private static bool VerifyHash(HashAlgorithm hashAlgorithm, string input, uint hash) {
        uint inputHash = getHash(hashAlgorithm, input);
        return hash.Equals(inputHash);
    }

    // void OnValidate() {
    //     using (SHA256 sha256 = SHA256.Create()) {
    //         uint hash = getHash(sha256, seed);
    //         random = new System.Random((int) (hash - int.MaxValue));
    //     }

    //     GenerateMap();
    // }
}
