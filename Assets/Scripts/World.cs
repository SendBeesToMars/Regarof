using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class World : MonoBehaviour {

    private const short TEST_SIZE = 1;

    public float global_seed;

    public Grid grid;
    public Tilemap tilemap;
    public Tile[] grass_tiles;

    public Dictionary<Coordinate, Chunk> chunks { get; set; }
    
    //TODO Remove once island generation is final
    public float scale = 0.1f;
    public float magnitude = 0.75f;
    public float border_weight = 0.2f;
    public float cut_off = 0.3f;
    
    void Start() {
        Seed();

        TileDataHandler.world = this;

        //TODO Replace with final world loading from save file (all chunks and their tiles to be saved, loading only visible chunks into memory)
        chunks = new Dictionary<Coordinate, Chunk>();
        for (int x = -TEST_SIZE; x <= TEST_SIZE; x++) {
            for (int y = -TEST_SIZE; y <= TEST_SIZE; y++) {
                CreateChunk(new Coordinate(x,y));
            }
        }

        //TODO Run once on world creation, and then again for every new chunk created off screen (3x3 around chunk created or relative to existing chunks).
        //     Will not be run on existing chunks being loaded in
        // foreach(Chunk chunk in chunks.Values) {
        //     InitializeChunk(chunk);
        // }
    }

    void FixedUpdate() {
        // //TODO Replace with final chunk loading logic and only redraw tiles modified (3x3 around updated tile) 
        // foreach(Chunk chunk in chunks.Values) {
            // chunk.GenerateIsland(scale, magnitude, border_weight, cut_off);
            // RenderChunk(chunk);
        // }
    }

    public Chunk CreateChunk(Coordinate coord) {
        Chunk chunk = new Chunk(coord, this);
        chunks.Add(coord, chunk);
        InitializeChunk(chunks[coord]);

        return chunk;
    }

    public void InitializeChunk(Chunk chunk) {
        for(int x = 0; x < Chunk.CHUNK_DIMENSION; x++) {
            for(int y = 0; y < Chunk.CHUNK_DIMENSION; y++) {
                TileDataHandler.UpdateNeighbours(chunk.tiles[x,y]);
                TileDataHandler.UpdateTileIndex(chunk.tiles[x,y]);
                // tilemap.SetTile(new Vector3Int((chunk.coord.x * Chunk.CHUNK_DIMENSION) + x, (chunk.coord.y * Chunk.CHUNK_DIMENSION) + y, 0), grass_tiles[chunk[x,y].tile_index]);
            }
        }
    }

    public void RenderChunk(Chunk chunk) {
        for(int x = 0; x < Chunk.CHUNK_DIMENSION; x++) {
            for(int y = 0; y < Chunk.CHUNK_DIMENSION; y++) {
                tilemap.SetTile(new Vector3Int((chunk.coord.x * Chunk.CHUNK_DIMENSION) + x, (chunk.coord.y * Chunk.CHUNK_DIMENSION) + y, 0), grass_tiles[chunk[x,y].tile_index]);
            }
        }
    }

    // Use only for debugging purposes
    // void LateUpdate() {
    //     // Vector3Int player = grid.WorldToCell(transform.position);
    //     // Vector3Int current = grid.WorldToCell(camera.ScreenToWorldPoint(Input.mousePosition));

    //     // if (Input.GetMouseButtonDown(0)) {
    //     //     // Debug.Log(GetTileData(v(current.x, current.y)).land);
    //     // }
    // }

    public TileDataModel GetTileData(Coordinate global_tile_coord) {
        Coordinate chunk_coord = GetChunkCoords(global_tile_coord);
        Coordinate relative_tile_coord = GetTileCoordsRelativeToChunk(global_tile_coord);

        // TODO Replace with chunk loading logic
        if(!chunks.ContainsKey(chunk_coord))
            return new TileDataModel(relative_tile_coord);

        return chunks[chunk_coord][relative_tile_coord.x, relative_tile_coord.y];
    }

    public Coordinate GetChunkCoords(Coordinate v) {
        return new Coordinate(v.x < 0 ? v.x - Chunk.CHUNK_DIMENSION : v.x, v.y < 0 ? v.y - Chunk.CHUNK_DIMENSION : v.y) / Chunk.CHUNK_DIMENSION;
    }

    private Coordinate GetTileCoordsRelativeToChunk(Coordinate v) {
        v.x %= Chunk.CHUNK_DIMENSION;
        v.y %= Chunk.CHUNK_DIMENSION;
        return new Coordinate(v.x < 0 ? Chunk.CHUNK_DIMENSION + v.x : v.x, v.y < 0 ? Chunk.CHUNK_DIMENSION + v.y : v.y);
    }
    
    //TODO Remove once island generation is final
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

    [ContextMenu("Seed")]
    public void Seed() {
        global_seed = Random.Range(-2147483.0f, 2147483.0f);
    }
}