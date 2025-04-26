using System;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
[Serializable]
public class SokobanPlayerTile : TileBase
{
    public Sprite playerSprite;

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        tileData.sprite = playerSprite;
    }
}