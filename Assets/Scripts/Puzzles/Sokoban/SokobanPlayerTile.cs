using System;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Azurite Objects/Sokoban/Player")]
[Serializable]
public class SokobanPlayerTile : TileBase
{
    public Sprite editorSprite;

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        if (!Application.isPlaying)
            tileData.sprite = editorSprite;
    }
}