using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class SokobanHandler : MonoBehaviour
{
    private Tilemap tilemap;
    public Sprite playerSprite;
    public Vector3Int playerTilemapPos;

    // Start is called before the first frame update
    void Start()
    {
        // Find player's initial pos
        tilemap = GetComponent<Tilemap>();
        BoundsInt bounds = tilemap.cellBounds;
        
        for (int x = 0; x < bounds.size.x; ++x)
        {
            for (int y = 0; y < bounds.size.y; ++y)
            {
                Vector3Int pos = new(x, y);
                if (!tilemap.HasTile(pos)) continue;
                SokobanTile tile = (SokobanTile) tilemap.GetTile(pos);
                if (tile.tileType != SokobanTile.TileType.Player) continue;
                if (playerTilemapPos != Vector3Int.zero) Debug.LogError("Multiple player tiles found");
                playerTilemapPos = pos;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3Int moveDir = new()
        {
            x = (Input.GetButtonDown("right") ? 1 : 0) + (Input.GetButtonDown("left") ? -1 : 0),
            y = (Input.GetButtonDown("up") ? 1 : 0) + (Input.GetButtonDown("down") ? -1 : 0)
        };
        // Don't allow diagonal movement
        if (moveDir == Vector3Int.zero || (moveDir.x != 0 && moveDir.y != 0)) return;

        Vector3Int next1 = playerTilemapPos + moveDir;
        Vector3Int next2 = next1 + moveDir;
        SokobanTile nextTile1 = (SokobanTile)tilemap.GetTile(next1);
        SokobanTile nextTile2 = (SokobanTile)tilemap.GetTile(next2);
        SokobanTile playerTile = (SokobanTile)tilemap.GetTile(playerTilemapPos);

        // Situations where the player can't move dat way
        if (nextTile1 == null) return;
        if (nextTile1.tileType == SokobanTile.TileType.Wall) return;

        if (nextTile1.tileType == SokobanTile.TileType.Box)
        {
            if (nextTile2 == null) return;
            nextTile2.tileType = SokobanTile.TileType.Box;
        }
        nextTile1.tileType = SokobanTile.TileType.Player;
        playerTile.tileType = SokobanTile.TileType.Floor;

        tilemap.RefreshAllTiles();
    }
}
