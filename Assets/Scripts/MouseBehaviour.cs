using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MouseBehaviour : MonoBehaviour {

    public Camera camera;

    public Tile tile;
    public Tilemap tilemap_selection;
    public Tilemap tilemap_fill;

    private Vector3Int previous;

    void LateUpdate() {
        Vector3Int player = tilemap_selection.WorldToCell(transform.position);
        Vector3Int current = tilemap_selection.WorldToCell(camera.ScreenToWorldPoint(Input.mousePosition));

        if(current != previous) {
            if(Vector3.Distance(player, current) < 2.5 && tilemap_fill.GetTile(current) != null) {
                tilemap_selection.SetTile(current, tile);
            }

            tilemap_selection.SetTile(previous, null);
            Debug.Log(current);
            
            previous = current;
        }
    }
}
