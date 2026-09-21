using System;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Azurite Objects/Sokoban/Goal")]
[Serializable]
public class SokobanGoalTile : TileBase
{
    public Sprite goalSprite;

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        tileData.sprite = goalSprite;
    }
}