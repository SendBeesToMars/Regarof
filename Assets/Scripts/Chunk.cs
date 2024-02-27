using UnityEngine;
using UnityEngine.Tilemaps;

public class Chunk {

    public const int CHUNK_DIMENSION = 32;

    private World world;

    public Coordinate coord { get; }
    public TileDataModel[,] tiles;

    // Remove or make const when island generation is final
    float[,] border_map;

    public Chunk(Coordinate coord, World world) {
        this.coord = coord;
        this.world = world;

        tiles = new TileDataModel[CHUNK_DIMENSION, CHUNK_DIMENSION];

        for(int x = 0; x < CHUNK_DIMENSION; x++) {
            for(int y = 0; y < CHUNK_DIMENSION; y++) {
                tiles[x,y] = new TileDataModel(new Coordinate((CHUNK_DIMENSION * coord.x) + x, (CHUNK_DIMENSION * coord.y) + y));
            }
        }
        
        // border_map = GenerateBorderMap(0.4f);
        GenerateIsland(world.scale, world.magnitude, world.border_weight, world.cut_off);
    }

    // To be run once during chunk creation (made private once generation is final)
    public void GenerateIsland(float scale, float magnitude, float border_weight, float cut_off) {
        //TODO Remove once island generation is final
        border_map = GenerateBorderMap(border_weight);

        for(int x = 0; x < CHUNK_DIMENSION; x++) {
            for(int y = 0; y < CHUNK_DIMENSION; y++) {
                // float noise = (Mathf.PerlinNoise(chunk_seed + (coord.x * CHUNK_DIMENSION + x) * scale, chunk_seed + (coord.y * CHUNK_DIMENSION + y) * scale) * magnitude) * GetBorderFade(x, y, border_weight);
                float noise = (Mathf.PerlinNoise(world.global_seed + (coord.x * CHUNK_DIMENSION + x) * scale, world.global_seed + (coord.y * CHUNK_DIMENSION + y) * scale) * magnitude) / border_map[x,y];
                tiles[x,y].land = noise > cut_off ? true : false;
            }
        }
    }

    // TODO Find a better way to fade island border or store a static const map manually
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

    public TileDataModel this[int x, int y] { get { return this.tiles[x,y]; } set { this.tiles[x,y] = value; } }
}