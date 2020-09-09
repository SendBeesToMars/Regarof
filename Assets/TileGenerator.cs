using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileGenerator : MonoBehaviour {

    public Tilemap tilemap_generated_fill;
    public Tilemap tilemap_generated_boundary;
    public Tile fill_tile;
    public Tile boundary_tile;

    [Range(0.0f, 1.0f)]
    public float magnitude = 0.75f;

    [Range(0.0f, 1.0f)]
    public float cut_off = 0.4f;

    [Range(0.0f, 1.0f)]
    public float scale = 0.005f;
    
    [Range(-1.0f, 0.0f)]
    public float border_fade_offset = -0.02f;

    private int[,] ground_map;
    float[,] border_fade;

    private int island_width = 24;
    private int island_height = 24;
    private int island_offset;

    private float seed;
    
    void Start() {
        Seed();  

        island_offset = (island_width == island_height) ? island_width / 2 : 0;
    }

    void FixedUpdate() {
        border_fade = GenerateBorderFade(island_width, island_height);
        ground_map = GenerateGroundMap(island_width, island_height);
        PopulateTileMap();
    }

    private float[,] GenerateBorderFade(int width, int height) {
        if(width%2 != 0 || height%2 != 0) {
            Debug.LogError("GenerateBorderFade(width, height) requires even values for both width and height.");
            return new float[width, height];
        }

        float[,] border_fade = new float[width, height];

        int range_x = width/2;
        int range_y = width/2;

        for(int x = 0; x < width; x++) {
            for(int y = 0; y < height; y++) {
                if(x < range_x) {
                    border_fade[x,y] = 1.0f / (x + 1) - border_fade_offset;
                } else {
                    border_fade[x,y] = 1.0f / (width - x) - border_fade_offset;
                }

                if(y < range_y) {
                    border_fade[x,y] += 1.0f / (y + 1) - border_fade_offset;
                } else {
                    border_fade[x,y] += 1.0f / (height - y) - border_fade_offset;
                }
            }
        }

        return border_fade;
    }

    private int[,] GenerateGroundMap(int width, int height) {
        int[,] map = new int[width,height];

        for(int x = 0; x < island_width; x++) {
            for(int y = 0; y < island_height; y++) {
                float noise = (Mathf.PerlinNoise(seed + (x * scale), seed + (y * scale)) * magnitude) + border_fade[x,y];
                if(noise < cut_off) {
                    map[x,y] = 1;
                } else {
                    map[x,y] = 0;
                }
            }
        }
        return map;
    }

    private void PopulateTileMap() {
        for(int x = 0; x < island_width; x++) {
            for(int y = 0; y < island_height; y++) {
                tilemap_generated_fill.SetTile(GetGridCoordinates(x,y), (ground_map[x,y] == 1) ? fill_tile : GetBorderTile(x,y));
            }
        }
    }

    private Tile GetBorderTile(int x, int y) {
        int neighbor_count = 0;
        for(int i = x-1; i <= x+1; i++) {
            for(int j = y-1; j <= y+1; j++) {
                if((i <= 0 || i >= island_width || j <= 0 || j >= island_height) || (i == x && j == y)) 
                    continue;
                neighbor_count += ground_map[i,j];
            }
        }

        if(neighbor_count != 0)
            tilemap_generated_boundary.SetTile(GetGridCoordinates(x,y), boundary_tile);

        return null;
    }

    private Vector3Int GetGridCoordinates(int x, int y) {
        return new Vector3Int(x - island_offset, y - island_offset, 0);
    }

    [ContextMenu("Seed")]
    public void Seed() {
        seed = Random.Range(-2147483.0f, 2147483.0f);
        Debug.Log(seed);
    }
}
