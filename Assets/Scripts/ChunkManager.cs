using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ChunkManager : MonoBehaviour {

    private const short CHUNK_SIZE = 32;

    private List<Chunk> chunksToUpdate;
    private List<Chunk> chunksToRender;

    public World world;
    public GameObject player;
    
    void Start() {

    }

    void FixedUpdate() {
        chunksToUpdate = GetSurroundingChunks();

        if (chunksToUpdate.Count > 0) {
            foreach (Chunk chunk in chunksToUpdate) {
                world.RenderChunk(chunk);
            }
        }
    }

    void LateUpdate() {
        if (Input.GetMouseButtonDown(1))
            print(GetCurrentChunkCoordinate());
    }

    private List<Chunk> GetSurroundingChunks() {
        return new List<Chunk> {
            GetChunkAtCoordinate(GetCurrentChunkCoordinate() + new Coordinate(-1, -1)),
            GetChunkAtCoordinate(GetCurrentChunkCoordinate() + new Coordinate( 0, -1)),
            GetChunkAtCoordinate(GetCurrentChunkCoordinate() + new Coordinate( 1, -1)),
            GetChunkAtCoordinate(GetCurrentChunkCoordinate() + new Coordinate(-1,  0)),
            GetChunkAtCoordinate(GetCurrentChunkCoordinate()),
            GetChunkAtCoordinate(GetCurrentChunkCoordinate() + new Coordinate( 1,  0)),
            GetChunkAtCoordinate(GetCurrentChunkCoordinate() + new Coordinate(-1,  1)),
            GetChunkAtCoordinate(GetCurrentChunkCoordinate() + new Coordinate( 0,  1)),
            GetChunkAtCoordinate(GetCurrentChunkCoordinate() + new Coordinate( 1,  1)) };
    }

    private Coordinate GetCurrentChunkCoordinate() {
        int playerX = Mathf.FloorToInt(player.transform.position.x);
        int playerY = Mathf.FloorToInt(player.transform.position.y);
        return new Coordinate(playerX < 0 ? playerX - CHUNK_SIZE : playerX, playerY < 0 ? playerY - CHUNK_SIZE : playerY) / CHUNK_SIZE;
    }

    private Chunk GetChunkAtCoordinate(Coordinate coord) {
        return world.chunks.ContainsKey(coord) ? world.chunks[coord] : world.CreateChunk(coord);
    }

}
