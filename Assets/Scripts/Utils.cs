using UnityEngine;

public class Utils : MonoBehaviour {
}


public struct Vector {

    public int x { get; set; }
    public int y { get; set; }

    public Vector(int x, int y) {
        this.x = x;
        this.y = y;
    }

    public static Vector operator /(Vector a, int b) {
        return new Vector((a.x < 0 ? a.x + 1 : a.x) / b, (a.y < 0 ? a.y + 1 : a.y) / b);
    } 

    public override string ToString() => $"({x},{y})";
    public override int GetHashCode() => $"({x},{y})".GetHashCode();
    public override bool Equals(object v) => (v is Vector) && ((Vector)v).x == this.x && ((Vector)v).y == this.y;
    public bool Equals(Vector v) => v.x == this.x && v.y == this.y;
}