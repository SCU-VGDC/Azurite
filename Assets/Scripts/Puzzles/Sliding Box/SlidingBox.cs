using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SlidingBox : MonoBehaviour
{
    public Tilemap puzzleTilemap; // Assign in Inspector
    public TileBase emptyTile; // A blank tile for clearing spaces
    public Vector2Int gridSize = new Vector2Int(6, 6); // Puzzle dimensions

    private Vector3Int selectedTile;
    private bool tileSelected = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left Click
        {
            Vector3Int tilePos = GetTileAtMousePosition();
            if (puzzleTilemap.HasTile(tilePos))
            {
                selectedTile = tilePos;
                tileSelected = true;
            }
        }

        if (tileSelected && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))) MoveTile(Vector3Int.up);
        if (tileSelected && (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))) MoveTile(Vector3Int.down);
        if (tileSelected && (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))) MoveTile(Vector3Int.left);
        if (tileSelected && (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))) MoveTile(Vector3Int.right);
    }

    Vector3Int GetTileAtMousePosition()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return puzzleTilemap.WorldToCell(mouseWorldPos);
    }

    void MoveTile(Vector3Int direction)
    {
        Vector3Int newTilePos = selectedTile + direction;

        // Check if new position is within bounds
        if (newTilePos.x < 0 || newTilePos.y < 0 || newTilePos.x >= gridSize.x || newTilePos.y >= gridSize.y) return;

        // Check if the new position is empty
        if (!puzzleTilemap.HasTile(newTilePos))
        {
            // Move the tile
            TileBase movingTile = puzzleTilemap.GetTile(selectedTile);
            puzzleTilemap.SetTile(newTilePos, movingTile);
            puzzleTilemap.SetTile(selectedTile, emptyTile);
            selectedTile = newTilePos; // Update selected tile's new position
        }
    }
}
