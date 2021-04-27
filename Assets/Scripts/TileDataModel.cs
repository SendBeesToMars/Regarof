

public class TileDataModel {

    public Vector coord { get; set; }

    public bool land { get; set; }
    public short tile_index { get; set; }
    public byte neighbours { get; set; }

    public TileDataModel(Vector coord, bool land = false, short tile_index = -1, byte neighbours = 0b_0000_0000) {
        this.coord = coord;
        this.land = land;
        this.tile_index = tile_index;
        this.neighbours = neighbours;
    }

    public void UpdateTileIndex(short index) {
        tile_index = index;
    }


}