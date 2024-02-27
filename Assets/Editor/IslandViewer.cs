using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

[CustomEditor (typeof (IslandGenerator))]
public class IslandGeneration : Editor {

    public override void OnInspectorGUI() {
        IslandGenerator generator = (IslandGenerator)target;
        Tilemap tiles = FindObjectOfType<Tilemap>();

        ushort tileSize = 16;
        Tile blackTile = CreateTile(tileSize, Color.black);
        Tile whiteTile = CreateTile(tileSize, Color.white);
        Tile blueTile = CreateTile(tileSize, Color.blue);
        Tile redTile = CreateTile(tileSize, Color.red);
        Tile yellowTile = CreateTile(tileSize, Color.yellow);
        Tile greyTile = CreateTile(tileSize, new Color(0.9f,0.9f,0.9f));

        DrawDefaultInspector();

        if(GUI.changed) {
            generate();
        }

        if(GUILayout.Button("Generate")) {
            generate();
        }

        void generate() {
            byte[,] island = generator.GenerateIsland();
            
            tiles.ClearAllTiles();
            for(ushort x = 0; x < IslandGenerator.ISLAND_SIZE; x++) {
                for(ushort y = 0; y < IslandGenerator.ISLAND_SIZE; y++) {
                    if(island[x,y] == 5)
                        SetTile(ref tiles, (x,y), greyTile);
                    else if(island[x,y] == 4)
                        SetTile(ref tiles, (x,y), yellowTile);
                    else if(island[x,y] == 3)
                        SetTile(ref tiles, (x,y), redTile);
                    else if(island[x,y] == 2)
                        SetTile(ref tiles, (x,y), blueTile);
                    else if(island[x,y] == 1)
                        SetTile(ref tiles, (x,y), blackTile);
                    else
                        SetTile(ref tiles, (x,y), whiteTile);
                }
            }
        }
    }

    private Tile CreateTile(ushort s, Color c) {
        Texture2D texture = new(s, s);
        for (int x = 0; x < s; x++) {
            for (int y = 0; y < s; y++) {
                texture.SetPixel(x, y, c);
            }
        }
        texture.Apply();

        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, s, s), new Vector2(0.5f, 0.5f), s);
        Tile tile = CreateInstance<Tile>();
        tile.sprite = sprite;

        return tile;
    }

    private void SetTile(ref Tilemap tiles, (ushort x, ushort y) coord, Tile t) {
        tiles.SetTile(new Vector3Int(coord.x - IslandGenerator.ISLAND_SIZE / 2, coord.y - IslandGenerator.ISLAND_SIZE / 2, 0), t);
    }
}
