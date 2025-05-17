using System;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
[Serializable]
public class SokobanBoxTile : TileBase
{
    public Sprite boxSprite;

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        if (!Application.isPlaying)
            tileData.sprite = boxSprite;
    }
}