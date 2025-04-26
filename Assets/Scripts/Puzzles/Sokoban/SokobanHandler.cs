using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class SokobanHandler : MonoBehaviour
{
    public UnityEvent onSolved;
    [SerializeField] private SokobanFloorTile floorTile;
    [SerializeField] private SokobanBoxTile boxTile;
    [SerializeField] private SokobanPlayerTile playerTile;
    [SerializeField] private SokobanWallTile wallTile;
    [SerializeField] private SokobanGoalTile goalTile;
    private bool solved = false;
    private Tilemap tilemap;
    private Vector3Int playerTilemapPos;
    private readonly List<Vector3Int> goalPositions = new();

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
                Vector3Int pos = bounds.position + new Vector3Int(x, y);
                if (!tilemap.HasTile(pos)) continue;
                TileBase tile = tilemap.GetTile(pos);

                if (tile == playerTile)
                {
                    if (playerTilemapPos != Vector3Int.zero) Debug.LogError("Multiple player tiles found");
                    playerTilemapPos = pos;
                }
                if (tile == goalTile)
                {
                    goalPositions.Add(pos);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (solved) return;

        Vector3Int moveDir = new()
        {
            x = Input.GetButtonDown("Horizontal") ? (int)Input.GetAxisRaw("Horizontal") : 0,
            y = Input.GetButtonDown("Vertical") ? (int)Input.GetAxisRaw("Vertical") : 0
        };
        // Don't allow diagonal movement
        if (moveDir == Vector3Int.zero || (moveDir.x != 0 && moveDir.y != 0)) return;

        Vector3Int next1 = playerTilemapPos + moveDir;
        Vector3Int next2 = next1 + moveDir;
        TileBase nextTile1 = tilemap.GetTile(next1);
        TileBase nextTile2 = tilemap.GetTile(next2);

        // Situations where the player can't move dat way
        if (nextTile1 == null) return;
        if (nextTile1 == wallTile) return;

        if (nextTile1 == boxTile)
        {
            if (nextTile2 == null || (nextTile2 != floorTile && nextTile2 != goalTile)) return;
            tilemap.SetTile(next2, boxTile);
        }
        tilemap.SetTile(next1, playerTile);
        tilemap.SetTile(playerTilemapPos, goalPositions.Contains(playerTilemapPos) ? goalTile : floorTile);

        playerTilemapPos = next1;
        tilemap.RefreshAllTiles();

        if (CheckSolution())
        {
            Debug.Log("wow you did it");
            solved = true;
            onSolved.Invoke();
        }
    }

    private bool CheckSolution()
    {
        foreach (Vector3Int pos in goalPositions)
        {
            if (tilemap.GetTile(pos) != boxTile) return false;
        }
        return true;
    }
}
