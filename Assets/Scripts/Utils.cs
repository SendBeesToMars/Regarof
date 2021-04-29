using UnityEngine;

public class Utils : MonoBehaviour {
}


public struct Coordinate {

    public int x { get; set; }
    public int y { get; set; }

    public Coordinate(int x, int y) {
        this.x = x;
        this.y = y;
    }

    public static Coordinate operator +(Coordinate a, Coordinate b) => new Coordinate(a.x + b.x, a.y + b.y);
    public static Coordinate operator -(Coordinate a, Coordinate b) => new Coordinate(a.x - b.x, a.y - b.y);
    public static Coordinate operator /(Coordinate a, int b) {
        return new Coordinate((a.x < 0 ? a.x + 1 : a.x) / b, (a.y < 0 ? a.y + 1 : a.y) / b);
    } 

    public override string ToString() => $"({x},{y})";
    public override int GetHashCode() => $"({x},{y})".GetHashCode();
    public override bool Equals(object v) => (v is Coordinate) && ((Coordinate)v).x == this.x && ((Coordinate)v).y == this.y;
    public bool Equals(Coordinate v) => v.x == this.x && v.y == this.y;
}