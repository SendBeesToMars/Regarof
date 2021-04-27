using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MouseBehaviour : MonoBehaviour {

    public Camera camera;

    public World world;

    private Vector3Int previous;

    void Start() {

    }

    void LateUpdate() {
        Vector3Int player_coord = world.grid.WorldToCell(transform.position);
        Vector3Int mouse_coord = world.grid.WorldToCell(camera.ScreenToWorldPoint(Input.mousePosition));

        if (Input.GetMouseButtonDown(0)) {
            Vector tile_coord = new Vector(mouse_coord.x, mouse_coord.y);
            Debug.Log(tile_coord);
            Debug.Log(world.GetTileData(tile_coord).land);
            TileDataHandler.Toggle(world.GetTileData(tile_coord));
        }

        // if (mouse_coord != previous) {
        //     if(Vector3.Distance(player_coord, mouse_coord) < 2.5 && tilemap_fill.GetTile(mouse_coord) != null) {
        //         tilemap_selection.SetTile(mouse_coord, tile);
        //     }

        //     tilemap_selection.SetTile(previous, null);
        //     // Debug.Log(current);
            
        //     previous = mouse_coord;
        // }
    }
}
