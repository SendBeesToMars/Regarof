using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MouseBehaviour : MonoBehaviour {

    public new Camera camera;

    public World world;

    private Vector3Int previous;

    public AudioSource audio_source;

    void Start() {
        audio_source = GetComponent<AudioSource>();
    }

    void LateUpdate() {
        Vector3Int player_coord = world.grid.WorldToCell(transform.position);
        Vector3Int mouse_coord = world.grid.WorldToCell(camera.ScreenToWorldPoint(Input.mousePosition));

        if (Input.GetMouseButtonDown(0)) {
            Coordinate tile_coord = new Coordinate(mouse_coord.x, mouse_coord.y);
            TileDataHandler.Toggle(world.GetTileData(tile_coord));
            audio_source.Play();
        }

        Coordinate player = new Coordinate(player_coord.x, player_coord.y);
        TileDataModel model = world.GetTileData(player);
        if(!model.land) {
            TileDataHandler.Toggle(model);
            audio_source.Play();
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
