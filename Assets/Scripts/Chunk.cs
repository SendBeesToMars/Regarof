using UnityEngine;
using UnityEngine.Tilemaps;

public class Chunk {

    public const int CHUNK_DIMENSION = 16;

    private World world;

    private byte[] bitmask_in;
    private byte[] bitmask_out;

    public Vector coord { get; }
    public TileDataModel[,] tiles;

    // public int[,] island_map;
    // public byte[,] tile_map;
    float[,] border_map;

    public Chunk(Vector coord, World world) {
        this.coord = coord;
        this.world = world;

        tiles = new TileDataModel[CHUNK_DIMENSION, CHUNK_DIMENSION];

        for(int x = 0; x < CHUNK_DIMENSION; x++) {
            for(int y = 0; y < CHUNK_DIMENSION; y++) {
                tiles[x,y] = new TileDataModel(new Vector((CHUNK_DIMENSION * coord.x) + x, (CHUNK_DIMENSION * coord.y) + y));
            }
        }


        // island_map = new int[CHUNK_DIMENSION, CHUNK_DIMENSION];
        // tile_map = new byte[CHUNK_DIMENSION, CHUNK_DIMENSION];

        // InitializeBitmasks();
        
        border_map = GenerateBorderMap(0.4f);
        GenerateIsland(world.scale, world.magnitude, world.border_weight, world.cut_off);
    }

    // public void PrintIsland(ref Tilemap tilemap) {
    //     for(int x = 0; x < CHUNK_DIMENSION; x++) {
    //         for(int y = 0; y < CHUNK_DIMENSION; y++) {
    //             if(island_map[x,y] == 1)
    //                 tilemap.SetTile(new Vector3Int((coord.x * CHUNK_DIMENSION) + x, (coord.y * CHUNK_DIMENSION) + y, 0), tile);
    //             else
    //                 tilemap.SetTile(new Vector3Int((coord.x * CHUNK_DIMENSION) + x, (coord.y * CHUNK_DIMENSION) + y, 0), tile2);
    //         }
    //     }
    // }

    public void GenerateIsland(float scale, float magnitude, float border_weight, float cut_off) {
        border_map = GenerateBorderMap(border_weight);

        for(int x = 0; x < CHUNK_DIMENSION; x++) {
            for(int y = 0; y < CHUNK_DIMENSION; y++) {
                // float noise = (Mathf.PerlinNoise(chunk_seed + (coord.x * CHUNK_DIMENSION + x) * scale, chunk_seed + (coord.y * CHUNK_DIMENSION + y) * scale) * magnitude) * GetBorderFade(x, y, border_weight);
                float noise = (Mathf.PerlinNoise(world.seed + (coord.x * CHUNK_DIMENSION + x) * scale, world.seed + (coord.y * CHUNK_DIMENSION + y) * scale) * magnitude) / border_map[x,y];
                if (noise > cut_off) {
                    tiles[x,y].land = true;
                    // tiles[x,y].tile_index = 25;
                } else {
                    tiles[x,y].land = false;
                    // tiles[x,y].tile_index = 47;
                }
            }
        }

        PopulateTileMap();
    }

    // public void SetTileIndex(int x, int y) {
    //     if (tiles[x,y].land)
    //         tiles[x,y].tile_index = 25;

    //     for(short index = 0; index < bitmask_in.Length; index++) {
    //         if(index == 25)
    //             continue;

    //         if((tiles[x,y].neighbours & bitmask_in[index]) == bitmask_out[index])
    //             tiles[x,y].tile_index = index;
    //     }
    // }

    private void PopulateTileMap() {
        for(int x = 0; x < CHUNK_DIMENSION; x++) {
            for(int y = 0; y < CHUNK_DIMENSION; y++) {
                tiles[x,y].neighbours = GetBorderTile(x,y);
                // SetTileIndex(x,y);
            }
        }
    }

    private byte GetBorderTile(int x, int y) {
        byte byte_value = 0;
        int iteration_count = 0;

        for(int i = x-1; i <= x+1; i++) {
            for(int j = y-1; j <= y+1; j++) {
                if ((i <= 0 || i >= CHUNK_DIMENSION || j <= 0 || j >= CHUNK_DIMENSION) || (i == x && j == y)) {
                    iteration_count++;
                    continue;
                }

                if (tiles[i,j].land == true) {
                    byte_value += GetByteValue(iteration_count);
                }

                iteration_count++;
            }
        }

        return byte_value;
    }

    private float[,] GenerateBorderMap(float border_weight) {

        float[,] border_fade = new float[CHUNK_DIMENSION, CHUNK_DIMENSION];

        int range_x = CHUNK_DIMENSION/2;
        int range_y = CHUNK_DIMENSION/2;

        for(int x = 0; x < CHUNK_DIMENSION; x++) {
            for(int y = 0; y < CHUNK_DIMENSION; y++) {
                if(x < range_x) {
                    border_fade[x,y] = 1.0f / (x + 1) + border_weight;
                } else {
                    border_fade[x,y] = 1.0f / (CHUNK_DIMENSION - x) + border_weight;
                }

                if(y < range_y) {
                    border_fade[x,y] += 1.0f / (y + 1) + border_weight;
                } else {
                    border_fade[x,y] += 1.0f / (CHUNK_DIMENSION - y) + border_weight;
                }
            }
        }

        return border_fade;
    }
    
    // private float GetBorderFade(int x, int y, int weight = 25) {
    //     int dist = Mathf.FloorToInt(Mathf.Pow(x - (CHUNK_DIMENSION / 2 - 0.5f), 2) + Mathf.Pow(y - (CHUNK_DIMENSION / 2 - 0.5f), 2));
    //     return dist < weight ? 1 : 1 / (dist / weight);
    // }

    private byte GetByteValue(int iteration_count) {
        switch (iteration_count) {
            case 0:
                return 0b_0000_0100;
            case 1:
                return 0b_1000_0000;
            case 2:
                return 0b_0000_1000;
            case 3:
                return 0b_0100_0000;
            case 5:
                return 0b_0001_0000;
            case 6:
                return 0b_0000_0010;
            case 7:
                return 0b_0010_0000;
            case 8:
                return 0b_0000_0001;
            default:
                return 0b_0000_0000;
        }
    }

//     private void InitializeBitmasks() {

// //        4 A 1
// //        D   B
// //        3 C 2
// //
// //        0b_DCBA_4321

//         bitmask_in = new byte[47];
//         bitmask_out = new byte[47];

//         bitmask_in[0] = 0b_1111_0010;
//         bitmask_out[0] = 0b_1001_0010;
//         bitmask_in[1] = 0b_1111_0100;
//         bitmask_out[1] = 0b_0011_0100;
//         bitmask_in[2] = 0b_1111_0000;
//         bitmask_out[2] = 0b_0101_0000;
//         bitmask_in[3] = 0b_1111_0110;
//         bitmask_out[3] = 0b_0001_0110;
//         bitmask_in[4] = 0b_1111_1001;
//         bitmask_out[4] = 0b_0100_1001;
//         bitmask_in[5] = 0b_1111_0000;
//         bitmask_out[5] = 0b_0111_0000;
//         bitmask_in[6] = 0b_1111_0000;
//         bitmask_out[6] = 0b_1110_0000;
//         bitmask_in[7] = 0b_1111_0010;
//         bitmask_out[7] = 0b_1001_0000;
//         bitmask_in[8] = 0b_1111_0100;
//         bitmask_out[8] = 0b_0011_0000;

//         bitmask_in[9] = 0b_1111_0001;
//         bitmask_out[9] = 0b_1100_0001;
//         bitmask_in[10] = 0b_1111_1000;
//         bitmask_out[10] = 0b_0110_1000;
//         bitmask_in[11] = 0b_1111_0000;
//         bitmask_out[11] = 0b_1010_0000;
//         bitmask_in[12] = 0b_1111_0011;
//         bitmask_out[12] = 0b_1000_0011;
//         bitmask_in[13] = 0b_1111_1100;
//         bitmask_out[13] = 0b_0010_1100;
//         bitmask_in[14] = 0b_1111_0000;
//         bitmask_out[14] = 0b_1011_0000;
//         bitmask_in[15] = 0b_1111_0000;
//         bitmask_out[15] = 0b_1101_0000;
//         bitmask_in[16] = 0b_1111_0001;
//         bitmask_out[16] = 0b_1100_0000;
//         bitmask_in[17] = 0b_1111_1000;
//         bitmask_out[17] = 0b_0110_0000;

//         bitmask_in[18] = 0b_1111_1111;
//         bitmask_out[18] = 0b_0000_0010;
//         bitmask_in[19] = 0b_1111_1111;
//         bitmask_out[19] = 0b_0000_0100;
//         bitmask_in[20] = 0b_1111_1111;
//         bitmask_out[20] = 0b_0000_0110;
//         bitmask_in[21] = 0b_1111_1111;
//         bitmask_out[21] = 0b_0000_1001;
//         bitmask_in[22] = 0b_1111_0011;
//         bitmask_out[22] = 0b_1000_0000;
//         bitmask_in[23] = 0b_1111_1100;
//         bitmask_out[23] = 0b_0010_0000;
//         bitmask_in[24] = 0b_1111_1111;
//         bitmask_out[24] = 0b_0000_1111;
        
//         bitmask_in[26] = 0b_1111_1111;
//         bitmask_out[26] = 0b_0000_0001;
//         bitmask_in[27] = 0b_1111_1111;
//         bitmask_out[27] = 0b_0000_1000;
//         bitmask_in[28] = 0b_1111_1111;
//         bitmask_out[28] = 0b_0000_0011;
//         bitmask_in[29] = 0b_1111_1111;
//         bitmask_out[29] = 0b_0000_1100;
//         bitmask_in[30] = 0b_1111_0110;
//         bitmask_out[30] = 0b_0001_0000;
//         bitmask_in[31] = 0b_1111_1001;
//         bitmask_out[31] = 0b_0100_0000;
//         bitmask_in[32] = 0b_1111_0000;
//         bitmask_out[32] = 0b_1111_0000;

//         bitmask_in[33] = 0b_1111_1111;
//         bitmask_out[33] = 0b_0000_1101;
//         bitmask_in[34] = 0b_1111_1111;
//         bitmask_out[34] = 0b_0000_1011;
//         bitmask_in[35] = 0b_1111_0011;
//         bitmask_out[35] = 0b_1000_0001;
//         bitmask_in[36] = 0b_1111_1100;
//         bitmask_out[36] = 0b_0010_1000;
//         bitmask_in[37] = 0b_1111_0110;
//         bitmask_out[37] = 0b_0001_0100;
//         bitmask_in[38] = 0b_1111_0110;
//         bitmask_out[38] = 0b_0001_0010;
//         bitmask_in[39] = 0b_1111_1111;
//         bitmask_out[39] = 0b_0000_0101;

//         bitmask_in[40] = 0b_1111_1111;
//         bitmask_out[40] = 0b_0000_1110;
//         bitmask_in[41] = 0b_1111_1111;
//         bitmask_out[41] = 0b_0000_0111;
//         bitmask_in[42] = 0b_1111_0011;
//         bitmask_out[42] = 0b_1000_0010;
//         bitmask_in[43] = 0b_1111_1100;
//         bitmask_out[43] = 0b_0010_0100;
//         bitmask_in[44] = 0b_1111_1001;
//         bitmask_out[44] = 0b_0100_1000;
//         bitmask_in[45] = 0b_1111_1001;
//         bitmask_out[45] = 0b_0100_0001;
//         bitmask_in[46] = 0b_1111_1111;
//         bitmask_out[46] = 0b_0000_1010;
//     }
}