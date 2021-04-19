using UnityEngine;

public class Utils : MonoBehaviour {
}


public struct vector {

    public int x { get; }
    public int y { get; }

    public vector(int x, int y) {
        this.x = x;
        this.y = y;
    }

    public override string ToString() => $"({x},{y})";
    public override int GetHashCode() => $"({x},{y})".GetHashCode();
}