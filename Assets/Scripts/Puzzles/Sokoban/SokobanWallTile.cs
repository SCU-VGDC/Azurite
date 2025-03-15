using System;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
[Serializable]
public class SokobanWallTile : TileBase
{
    public Sprite[] wallSprites;

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

        tileData.sprite = wallSprites[GetWallSpriteIndex(wallMask)];
    }

    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        for (int x = -1; x <= 1; ++x)
        {
            for (int y = -1; y <= 1; ++y)
            {
                Vector3Int neighbor = position + new Vector3Int(x, y, position.z);
                if (IsTileWall(neighbor, tilemap)) tilemap.RefreshTile(neighbor);
            }
        }
    }

    private bool IsTileWall(Vector3Int position, ITilemap tilemap)
    {
        TileBase tile = tilemap.GetTile(position);
        return tile != null && tile == this;
    }

    // TODO: need to actually get the sprites for this
    private uint GetWallSpriteIndex(byte wallMask)
    {
        return wallMask;
    }
}