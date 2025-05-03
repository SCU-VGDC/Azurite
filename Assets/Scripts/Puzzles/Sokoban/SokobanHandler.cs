using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;
using DG.Tweening;

[RequireComponent(typeof(Tilemap))]
public class SokobanHandler : MonoBehaviour
{
    [SerializeField] private SokobanFloorTile floorTile;
    [SerializeField] private SokobanBoxTile boxTile;
    [SerializeField] private SokobanPlayerTile playerTile;
    [SerializeField] private SokobanWallTile wallTile;
    [SerializeField] private SokobanGoalTile goalTile;
    [SerializeField] private GameObject playerFloatSpritePrefab;
    [SerializeField] private GameObject boxFloatSpritePrefab;

    public UnityEvent onSolved;
    private Transform playerFloatSprite;
    private Grid tileGrid;
    private bool solved = false;
    private Tilemap tilemap;
    private Vector3Int playerTilemapPos;
    private readonly List<Vector3Int> goalPositions = new();
    private readonly List<Vector3Int> boxPositions = new();
    private readonly List<Transform> boxSprites = new();

    // Start is called before the first frame update
    void Start()
    {
        // Find player's initial pos
        tilemap = GetComponent<Tilemap>();
        tileGrid = GetComponentInParent<Grid>();
        playerFloatSprite = Instantiate(playerFloatSpritePrefab, transform).transform;
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
                if (tile == boxTile)
                {
                    Transform boxFloatSprite = Instantiate(boxFloatSpritePrefab, transform).transform;
                    boxFloatSprite.position = GetTileWorldPos(pos);
                    boxPositions.Add(pos);
                    boxSprites.Add(boxFloatSprite);
                    tilemap.SetTile(pos, floorTile);
                }
            }
        }

        playerFloatSprite.position = GetTileWorldPos(playerTilemapPos);
        tilemap.SetTile(playerTilemapPos, floorTile);
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

        if (boxPositions.Contains(next1))
        {
            if (nextTile2 == null || boxPositions.Contains(next2) || (nextTile2 != floorTile && nextTile2 != goalTile)) return;
            int i = boxPositions.FindIndex(p => p == next1);
            boxSprites[i].DOMove(GetTileWorldPos(next2), 0.1f);
            boxPositions[i] = next2;
        }

        playerTilemapPos = next1;
        tilemap.RefreshAllTiles();
        playerFloatSprite.DOMove(GetTileWorldPos(playerTilemapPos), 0.1f);

        if (CheckSolution())
        {
            Debug.Log("wow you did it");
            solved = true;
            onSolved.Invoke();
        }
    }

    private Vector3 GetTileWorldPos(Vector3Int cell)
    {
        return tileGrid.CellToWorld(cell) + tileGrid.cellSize / 2 + Vector3.forward;
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
