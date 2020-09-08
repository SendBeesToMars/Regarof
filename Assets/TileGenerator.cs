using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileGenerator : MonoBehaviour {

    public Tilemap tilemap_generated;
    public Tile fill_tile;

    [Range(0.0f, 1.0f)]
    public float magnitude = 0.75f;

    [Range(0.0f, 1.0f)]
    public float cut_off = 0.4f;

    [Range(0.0f, 1.0f)]
    public float scale = 0.005f;
    
    [Range(-1.0f, 0.0f)]
    public float border_fade_offset = -0.02f;

    private float[,] height_map;
    float[,] border_fade;

    private int island_width = 16;
    private int island_height = 16;

    private float seed;
    
    void Start() {
        height_map = new float[island_width, island_height];
        border_fade = GenerateBorderFade(island_width, island_height);

        Seed();  
    }

    void FixedUpdate() {
        for(int x = 0; x < island_width; x++) {
            for(int y = 0; y < island_height; y++) {
                float noise = (Mathf.PerlinNoise(seed + (x * scale), seed + (y * scale)) * magnitude) + border_fade[x,y];

                if(noise < cut_off) {
                    tilemap_generated.SetTile(new Vector3Int(x, y, 0), fill_tile);
                } else {
                    tilemap_generated.SetTile(new Vector3Int(x, y, 0), null);
                }
            }
        }
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

    [ContextMenu("Seed")]
    public void Seed() {
        seed = Random.Range(-2147483.0f, 2147483.0f);
        Debug.Log(seed);
    }
}
