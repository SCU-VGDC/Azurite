using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SokobanPlayerTile : TileBase
{
    public SokobanHandler handler;
    public Sprite currentSprite;

    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        Vector3Int goalPos = position + (Vector3Int)handler.moveDirection;
        Sprite goalSprite = tilemap.GetSprite(goalPos);

        if (goalSprite == handler.wallSprite) return;
        if (goalSprite == handler.floorSprite)
        {
            handler.playerTilemapPos = goalPos;
            tilemap.RefreshTile(goalPos);
            return;
        }
        if (goalSprite == handler.boxSprite)
        {
            Vector3Int nextPos = goalPos + (Vector3Int)handler.moveDirection;
            Sprite nextSprite = tilemap.GetSprite(nextPos);
            SokobanPlayerTile nextTile = (SokobanPlayerTile)tilemap.GetTile(nextPos);
            if (nextSprite == null || nextSprite != handler.floorSprite) return;
            nextTile.currentSprite = handler.boxSprite;
            tilemap.RefreshTile(goalPos);
        }
    }

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        tileData.sprite = currentSprite;
    }
}
