using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour {

    public Tile[] grass_tiles;
    public Tile[] water_tiles;
    public Tile[] debug_tiles;
    public Tile boundary_tile;


    private static byte[] bitmask_in { get; }
    private static byte[] bitmask_out;

    private float seed;
    
    void Start() {
        InitializeBitmasks();
    }

    private Tile GetBorderTile(int x, int y) {
        // byte byte_value = 0;
        // int iteration_count = 0;

        // for(int i = x-1; i <= x+1; i++) {
        //     for(int j = y-1; j <= y+1; j++) {
        //         if ((i <= 0 || i >= island_width || j <= 0 || j >= island_height) || (i == x && j == y)) {
        //             iteration_count++;
        //             continue;
        //         }

        //         if (ground_map[i,j] == 1) {
        //             byte_value += GetByteValue(iteration_count);
        //         }

        //         iteration_count++;
        //     }
        // }
 
        // if(byte_value > 0)
        //     tilemap_generated_boundary.SetTile(GetGridCoordinates(x,y), boundary_tile);
        // else
        //     tilemap_generated_boundary.SetTile(GetGridCoordinates(x,y), null);

        // for(int index = 0; index < bitmask_in.Length; index++) {
        //     if(index == 25)
        //         continue;

        //     if((byte_value & bitmask_in[index]) == bitmask_out[index])
        //         return ground_tiles[index];
        // }

        return null;
    }

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

    private void InitializeBitmasks() {

//        4 A 1
//        D   B
//        3 C 2
//
//        0b_DCBA_4321

        // bitmask_in = new byte[ground_tiles.Length];
        // bitmask_out = new byte[ground_tiles.Length];

        bitmask_in[0] = 0b_1111_0010;
        bitmask_out[0] = 0b_1001_0010;
        bitmask_in[1] = 0b_1111_0100;
        bitmask_out[1] = 0b_0011_0100;
        bitmask_in[2] = 0b_1111_0000;
        bitmask_out[2] = 0b_0101_0000;
        bitmask_in[3] = 0b_1111_0110;
        bitmask_out[3] = 0b_0001_0110;
        bitmask_in[4] = 0b_1111_1001;
        bitmask_out[4] = 0b_0100_1001;
        bitmask_in[5] = 0b_1111_0000;
        bitmask_out[5] = 0b_0111_0000;
        bitmask_in[6] = 0b_1111_0000;
        bitmask_out[6] = 0b_1110_0000;
        bitmask_in[7] = 0b_1111_0010;
        bitmask_out[7] = 0b_1001_0000;
        bitmask_in[8] = 0b_1111_0100;
        bitmask_out[8] = 0b_0011_0000;

        bitmask_in[9] = 0b_1111_0001;
        bitmask_out[9] = 0b_1100_0001;
        bitmask_in[10] = 0b_1111_1000;
        bitmask_out[10] = 0b_0110_1000;
        bitmask_in[11] = 0b_1111_0000;
        bitmask_out[11] = 0b_1010_0000;
        bitmask_in[12] = 0b_1111_0011;
        bitmask_out[12] = 0b_1000_0011;
        bitmask_in[13] = 0b_1111_1100;
        bitmask_out[13] = 0b_0010_1100;
        bitmask_in[14] = 0b_1111_0000;
        bitmask_out[14] = 0b_1011_0000;
        bitmask_in[15] = 0b_1111_0000;
        bitmask_out[15] = 0b_1101_0000;
        bitmask_in[16] = 0b_1111_0001;
        bitmask_out[16] = 0b_1100_0000;
        bitmask_in[17] = 0b_1111_1000;
        bitmask_out[17] = 0b_0110_0000;

        bitmask_in[18] = 0b_1111_1111;
        bitmask_out[18] = 0b_0000_0010;
        bitmask_in[19] = 0b_1111_1111;
        bitmask_out[19] = 0b_0000_0100;
        bitmask_in[20] = 0b_1111_1111;
        bitmask_out[20] = 0b_0000_0110;
        bitmask_in[21] = 0b_1111_1111;
        bitmask_out[21] = 0b_0000_1001;
        bitmask_in[22] = 0b_1111_0011;
        bitmask_out[22] = 0b_1000_0000;
        bitmask_in[23] = 0b_1111_1100;
        bitmask_out[23] = 0b_0010_0000;
        bitmask_in[24] = 0b_1111_1111;
        bitmask_out[24] = 0b_0000_1111;
        
        bitmask_in[26] = 0b_1111_1111;
        bitmask_out[26] = 0b_0000_0001;
        bitmask_in[27] = 0b_1111_1111;
        bitmask_out[27] = 0b_0000_1000;
        bitmask_in[28] = 0b_1111_1111;
        bitmask_out[28] = 0b_0000_0011;
        bitmask_in[29] = 0b_1111_1111;
        bitmask_out[29] = 0b_0000_1100;
        bitmask_in[30] = 0b_1111_0110;
        bitmask_out[30] = 0b_0001_0000;
        bitmask_in[31] = 0b_1111_1001;
        bitmask_out[31] = 0b_0100_0000;
        bitmask_in[32] = 0b_1111_0000;
        bitmask_out[32] = 0b_1111_0000;

        bitmask_in[33] = 0b_1111_1111;
        bitmask_out[33] = 0b_0000_1101;
        bitmask_in[34] = 0b_1111_1111;
        bitmask_out[34] = 0b_0000_1011;
        bitmask_in[35] = 0b_1111_0011;
        bitmask_out[35] = 0b_1000_0001;
        bitmask_in[36] = 0b_1111_1100;
        bitmask_out[36] = 0b_0010_1000;
        bitmask_in[37] = 0b_1111_0110;
        bitmask_out[37] = 0b_0001_0100;
        bitmask_in[38] = 0b_1111_0110;
        bitmask_out[38] = 0b_0001_0010;
        bitmask_in[39] = 0b_1111_1111;
        bitmask_out[39] = 0b_0000_0101;

        bitmask_in[40] = 0b_1111_1111;
        bitmask_out[40] = 0b_0000_1110;
        bitmask_in[41] = 0b_1111_1111;
        bitmask_out[41] = 0b_0000_0111;
        bitmask_in[42] = 0b_1111_0011;
        bitmask_out[42] = 0b_1000_0010;
        bitmask_in[43] = 0b_1111_1100;
        bitmask_out[43] = 0b_0010_0100;
        bitmask_in[44] = 0b_1111_1001;
        bitmask_out[44] = 0b_0100_1000;
        bitmask_in[45] = 0b_1111_1001;
        bitmask_out[45] = 0b_0100_0001;
        bitmask_in[46] = 0b_1111_1111;
        bitmask_out[46] = 0b_0000_1010;
    }
}
