using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class World : MonoBehaviour {

    public float seed;

    public Grid world_grid;
    public Tilemap world_tilemap;

    public Tile[] ground_tiles;
    // public Tile tile;
    // public Dictionary<vector, Chunk> chunks;
    public List<Chunk> chunks;
    
    public float scale = 0.15f;
    public float magnitude = 0.75f;
    public float border_weight = 0.1f;
    public float cut_off = 0.3f;
    
    void Start() {

        // new Chunk(-1,-1).PrintIsland(ref world_tilemap);

        // chunks = new Dictionary<vector, Chunk>();
        chunks = new List<Chunk>();
        for (int i = -3; i < 3; i++) {
            for (int j = -3; j < 3; j++) {
                // chunks.Add(v(i,j), new Chunk(i,j));
                // chunks.Add(v(0,0), new Chunk(0,0));
                chunks.Add(new Chunk(i, j, ground_tiles));
            }
        }
        Seed();
    }

    void FixedUpdate() {
        foreach(Chunk chunk in chunks) {
            chunk.GenerateIsland(scale, magnitude, border_weight, cut_off);
            for(int x = 0; x < Chunk.CHUNK_DIMENSION; x++) {
                for(int y = 0; y < Chunk.CHUNK_DIMENSION; y++) {
                    if (chunk.tile_map[x,y] >= 0)
                        world_tilemap.SetTile(new Vector3Int((chunk.coord.x * Chunk.CHUNK_DIMENSION) + x, (chunk.coord.y * Chunk.CHUNK_DIMENSION) + y, 0), ground_tiles[chunk.tile_map[x,y]]);
                    else
                        world_tilemap.SetTile(new Vector3Int((chunk.coord.x * Chunk.CHUNK_DIMENSION) + x, (chunk.coord.y * Chunk.CHUNK_DIMENSION) + y, 0), null);
                }
            }
            // chunk.PrintIsland(ref world_tilemap);
        }
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

    public static vector v(int x, int y) {
        return new vector(x, y);
    }

    [ContextMenu("Seed")]
    public void Seed() {
        seed = Random.Range(-2147483.0f, 2147483.0f);
        foreach(Chunk chunk in chunks)
            chunk.SeedChunk();
    }
}