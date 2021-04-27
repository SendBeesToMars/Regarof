using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class World : MonoBehaviour {
    public Camera camera;

    public float seed;

    public Grid grid;
    public Tilemap tilemap;

    public Tile[] ground_tiles;
    // public Tile tile;
    // public Dictionary<vector, Chunk> chunks;
    public Dictionary<Vector, Chunk> chunks { get; set; }
    
    public float scale = 0.1f;
    public float magnitude = 0.75f;
    public float border_weight = 0.2f;
    public float cut_off = 0.3f;
    
    void Start() {
        Seed();

        chunks = new Dictionary<Vector, Chunk>();
        for (int i = -3; i < 3; i++) {
            for (int j = -3; j < 3; j++) {
                chunks.Add(v(i,j), new Chunk(v(i,j), this));
            }
        }

        TileDataHandler.world = this;

        foreach(Chunk chunk in chunks.Values) {
            for(int x = 0; x < Chunk.CHUNK_DIMENSION; x++) {
                for(int y = 0; y < Chunk.CHUNK_DIMENSION; y++) {
                    TileDataHandler.UpdateNeighbours(chunk.tiles[x,y]);
                    TileDataHandler.UpdateTileIndex(chunk.tiles[x,y]);
                    tilemap.SetTile(new Vector3Int((chunk.coord.x * Chunk.CHUNK_DIMENSION) + x, (chunk.coord.y * Chunk.CHUNK_DIMENSION) + y, 0), ground_tiles[chunk.tiles[x,y].tile_index]);
                }
            }
        }
    }

    void FixedUpdate() {
        
        foreach(Chunk chunk in chunks.Values) {
            // chunk.GenerateIsland(scale, magnitude, border_weight, cut_off);
            for(int x = 0; x < Chunk.CHUNK_DIMENSION; x++) {
                for(int y = 0; y < Chunk.CHUNK_DIMENSION; y++) {
                    // if (chunk.tiles[x,y].tile_index != -1) {
                        tilemap.SetTile(new Vector3Int((chunk.coord.x * Chunk.CHUNK_DIMENSION) + x, (chunk.coord.y * Chunk.CHUNK_DIMENSION) + y, 0), ground_tiles[chunk.tiles[x,y].tile_index]); 
                    // } else
                    //     tilemap.SetTile(new Vector3Int((chunk.coord.x * Chunk.CHUNK_DIMENSION) + x, (chunk.coord.y * Chunk.CHUNK_DIMENSION) + y, 0), null);
                }
            }
        }

        // TileDataHandler.Toggle();
    }

    void LateUpdate() {
        Vector3Int player = grid.WorldToCell(transform.position);
        Vector3Int current = grid.WorldToCell(camera.ScreenToWorldPoint(Input.mousePosition));

        // Vector3Int grid_position = world_grid.WorldToCell(current);

        // if (Input.GetMouseButtonDown(0)) {
        //     // Debug.Log(GetTileData(v(current.x, current.y)).land);
        // }
    }

    public TileDataModel GetTileData(Vector v) {
        Vector c = GetChunkCoords(v);
        Vector v1 = GetTileCoordsRelativeToChunk(v);

        if(!chunks.ContainsKey(c))
            return new TileDataModel(v1);

        // Debug.Log(c + " " + v + " " + v1 + " " + chunks[c].tiles[v1.x, v1.y]);
        // Debug.Log(c + " " + v1);
        return chunks[c].tiles[v1.x, v1.y];
    }

    public Vector GetChunkCoords(Vector v) {
        return new Vector(v.x < 0 ? v.x - Chunk.CHUNK_DIMENSION : v.x, v.y < 0 ? v.y - Chunk.CHUNK_DIMENSION : v.y) / Chunk.CHUNK_DIMENSION;
    }

    private Vector GetTileCoordsRelativeToChunk(Vector v) {
        v.x %= Chunk.CHUNK_DIMENSION;
        v.y %= Chunk.CHUNK_DIMENSION;
        return new Vector(v.x < 0 ? Chunk.CHUNK_DIMENSION + v.x : v.x, v.y < 0 ? Chunk.CHUNK_DIMENSION + v.y : v.y);
    }
    
    public void updateChunkIslandScale(float scale) {
        this.scale = scale;
    }

    public void updateChunkIslandMagnitude(float magnitude) {
        this.magnitude = magnitude;
    }

    public void updateChunkIslandBorder(float border_weight) {
        this.border_weight = border_weight;
    }

    public void updateChunkIslandCutOff(float cut_off) {
        this.cut_off = cut_off;
    }

    public static Vector v(int x, int y) {
        return new Vector(x, y);
    }

    [ContextMenu("Seed")]
    public void Seed() {
        seed = Random.Range(-2147483.0f, 2147483.0f);
        // foreach(Chunk chunk in chunks)
        //     chunk.SeedChunk();
    }
}