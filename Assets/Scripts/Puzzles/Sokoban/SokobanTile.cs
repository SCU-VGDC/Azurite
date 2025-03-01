using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
[Serializable]
public class SokobanTile : TileBase
{
    public Sprite playerSprite;
    public Sprite boxSprite;
    public Sprite floorSprite;
    public Sprite[] wallSprites;

    public TileType tileType;

    public enum TileType
    {
        Player,
        Floor,
        Wall,
        Box
    }

    public void Awake()
    {
        
    }

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        byte currentMaskMod = 1;
        byte wallMask = 0;
        for (int i = 0; i < 4; ++i)
        {
            // Order for finding neighbors is clockwise starting at top (top, right, down, left)
            Vector3Int offset = new()
            {
                x = i % 2 == 1 ? -(i - 2) : 0,
                y = i % 2 == 0 ? -(i - 1) : 0,
                z = 0
            };
            if (IsTileWall(position + offset, tilemap)) wallMask |= currentMaskMod;
            currentMaskMod <<= 1;
        }

        switch (tileType)
        {
            case TileType.Player: tileData.sprite = playerSprite; break;
            case TileType.Floor: tileData.sprite = floorSprite; break;
            case TileType.Box: tileData.sprite = boxSprite; break;
            case TileType.Wall: tileData.sprite = wallSprites[GetWallSpriteIndex(wallMask)]; break;
        }
    }

    private bool IsTileWall(Vector3Int position, ITilemap tilemap)
    {
        TileBase tile = tilemap.GetTile(position);
        return tile != null && tile == this && ((SokobanTile)tile).tileType == TileType.Wall;
    }

    // TODO: need to actually get the sprites for this
    private uint GetWallSpriteIndex(byte wallMask)
    {
        return 0;
    }
}