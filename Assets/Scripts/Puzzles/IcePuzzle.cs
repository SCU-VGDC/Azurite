using UnityEngine;
using UnityEngine.Tilemaps;

public class IcePuzzle : Puzzle
{
    public override Bounds Bounds
    {
        get
        {
            Bounds? bounds = null;
            
            foreach (var tilemap in GetComponentsInChildren<Tilemap>())
            {
                tilemap.CompressBounds();

                var worldBounds = tilemap.GetComponent<TilemapRenderer>().bounds;
                worldBounds.size += Vector3.one * 5;

                if (bounds == null)
                    bounds = worldBounds;
                else
                    bounds.Value.Encapsulate(worldBounds);
            }

            return bounds.Value;
        }
    }
}
