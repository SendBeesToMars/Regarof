using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
 
public class HighlightInFront : MonoBehaviour {

    public Tile highlightTile;
    public Tilemap highlightMap;
 
    private Vector3Int previous;
 
    private void LateUpdate() {
        Vector3Int currentCell = highlightMap.WorldToCell(transform.position);
        currentCell.x += 1;
 
        if(currentCell != previous) {
            highlightMap.SetTile(currentCell, highlightTile);
 
            highlightMap.SetTile(previous, null);
 
            previous = currentCell;
        }
    }
}