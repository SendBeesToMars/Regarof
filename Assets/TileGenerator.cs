using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileGenerator : MonoBehaviour {

    public Tilemap tilemap_generated;
    public Tilemap tilemap_generated_boundary;

    public Tile[] ground_tiles;
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

        map[6,6] = 0;
        return map;
    }

    private void PopulateTileMap() {
        for(int x = 0; x < island_width; x++) {
            for(int y = 0; y < island_height; y++) {
                tilemap_generated.SetTile(GetGridCoordinates(x,y), (ground_map[x,y] == 1) ? ground_tiles[25] : GetBorderTile(x,y));
            }
        }
    }

    private Tile GetBorderTile(int x, int y) {
        byte byte_integer_value = 0;
        int iteration_count = 0;

        for(int i = x-1; i <= x+1; i++) {
            for(int j = y-1; j <= y+1; j++) {
                if((i <= 0 || i >= island_width || j <= 0 || j >= island_height) || (i == x && j == y)) {
                    iteration_count++;
                    continue;
                }

                if(ground_map[i,j] == 1) {
                    byte_integer_value += GetIntegerValue(iteration_count);
                }

                iteration_count++;
            }
        }


        //EDGES
        if((byte_integer_value & 0b_1111_0000) == 0b_1111_0000)
            return ground_tiles[32];

        if((byte_integer_value & 0b_1111_0100) == 0b_0011_0000)
            return ground_tiles[8];

        if((byte_integer_value & 0b_1111_1000) == 0b_0110_0000)
            return ground_tiles[17];

        if((byte_integer_value & 0b_1111_0001) == 0b_1100_0000)
            return ground_tiles[16];

        if((byte_integer_value & 0b_1111_0010) == 0b_1001_0000)
            return ground_tiles[7];

        if((byte_integer_value & 0b_1111_0000) == 0b_0111_0000)
            return ground_tiles[5];

        if((byte_integer_value & 0b_1111_0000) == 0b_1110_0000)
            return ground_tiles[6];

        if((byte_integer_value & 0b_1111_0000) == 0b_1011_0000)
            return ground_tiles[14];

        if((byte_integer_value & 0b_1111_0000) == 0b_1101_0000)
            return ground_tiles[15];

        if((byte_integer_value & 0b_1111_0110) == 0b_0001_0000)
            return ground_tiles[30];

        if((byte_integer_value & 0b_1111_1001) == 0b_0100_0000)
            return ground_tiles[31];

        if((byte_integer_value & 0b_1111_0011) == 0b_1000_0000)
            return ground_tiles[22];

        if((byte_integer_value & 0b_1111_1100) == 0b_0010_0000)
            return ground_tiles[23];



        //CORNERS
        if((byte_integer_value & 0b_1111_0011) == 0b_0000_0001)
            return ground_tiles[26];

        if((byte_integer_value & 0b_1111_0011) == 0b_0000_0010)
            return ground_tiles[18];

        if((byte_integer_value & 0b_1111_1100) == 0b_0000_0100)
            return ground_tiles[19];

        if((byte_integer_value & 0b_1111_1100) == 0b_0000_1000)
            return ground_tiles[27];

        if((byte_integer_value & 0b_1111_0011) == 0b_0000_0011)
            return ground_tiles[28];

        if((byte_integer_value & 0b_1111_1100) == 0b_0000_1100)
            return ground_tiles[29];

        if((byte_integer_value & 0b_0000_1001) == 0b_0000_1001)
            return ground_tiles[47];

        if((byte_integer_value & 0b_0000_0011) == 0b_0000_0011)
            return ground_tiles[47];

        if((byte_integer_value & 0b_0000_0110) == 0b_0000_0110)
            return ground_tiles[47];

        if((byte_integer_value & 0b_0000_1100) == 0b_0000_1100)
            return ground_tiles[47];

        if((byte_integer_value & 0b_0000_0111) == 0b_0000_0111)
            return ground_tiles[47];

        if((byte_integer_value & 0b_0000_1110) == 0b_0000_1110)
            return ground_tiles[47];

        if((byte_integer_value & 0b_0000_1101) == 0b_0000_1101)
            return ground_tiles[47];

        if((byte_integer_value & 0b_0000_1011) == 0b_0000_1011)
            return ground_tiles[47];

        if((byte_integer_value & 0b_0000_0000) == 0b_0000_0000)
            return null;


        if((int)byte_integer_value > 0)
            tilemap_generated_boundary.SetTile(GetGridCoordinates(x,y), boundary_tile);


        return ground_tiles[47];
    }

    private Vector3Int GetGridCoordinates(int x, int y) {
        return new Vector3Int(x - island_offset, y - island_offset, 0);
    }

    private byte GetIntegerValue(int iteration_count) {
        byte value = 0;

        switch (iteration_count) {
            case 0:
                value = 0b_0000_0100;
                break;
            case 1:
                value = 0b_1000_0000;
                break;
            case 2:
                value = 0b_0000_1000;
                break;
            case 3:
                value = 0b_0100_0000;
                break;
            case 5:
                value = 0b_0001_0000;
                break;
            case 6:
                value = 0b_0000_0010;
                break;
            case 7:
                value = 0b_0010_0000;
                break;
            case 8:
                value = 0b_0000_0001;
                break;
            default:
                break;
        }

        return value;
    }

    [ContextMenu("Seed")]
    public void Seed() {
        seed = Random.Range(-2147483.0f, 2147483.0f);
        Debug.Log(seed);
    }
}
