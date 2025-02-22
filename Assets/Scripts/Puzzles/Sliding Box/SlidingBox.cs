using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SlidingBox : MonoBehaviour
{
    public Tilemap gameTilemap;  // Assign in Inspector
    public TileBase playerTile;  // Assign Player Tile in Inspector
    public TileBase emptyTile;   // Assign an Empty Tile in Inspector
    public float moveSpeed = 5f; // Adjust movement speed

    private Vector3Int playerPosition;
    private bool isMoving = false;

    void Start()
    {
        playerPosition = gameTilemap.WorldToCell(transform.position);
        gameTilemap.SetTile(playerPosition, playerTile);
    }

    void Update()
    {
        if (isMoving) return; // Prevent movement spam while animating

        if (Input.GetKeyDown(KeyCode.W)) Move(Vector3Int.up);
        if (Input.GetKeyDown(KeyCode.S)) Move(Vector3Int.down);
        if (Input.GetKeyDown(KeyCode.A)) Move(Vector3Int.left);
        if (Input.GetKeyDown(KeyCode.D)) Move(Vector3Int.right);
    }

    void Move(Vector3Int direction)
    {
        Vector3Int newPosition = playerPosition + direction;

        // Check if the new position is within bounds
        if (!gameTilemap.HasTile(newPosition)) return;

        // Check for obstacles
        TileBase targetTile = gameTilemap.GetTile(newPosition);
        if (targetTile != null && targetTile.name == "WallTile") return;

        // Update Tilemap before moving
        gameTilemap.SetTile(playerPosition, emptyTile);
        gameTilemap.SetTile(newPosition, playerTile);

        // Start smooth movement
        StartCoroutine(SmoothMove(transform.position, gameTilemap.GetCellCenterWorld(newPosition)));

        // Update position
        playerPosition = newPosition;
    }

    IEnumerator SmoothMove(Vector3 start, Vector3 end)
    {
        isMoving = true;
        float elapsedTime = 0f;
        float duration = 1f / moveSpeed;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(start, end, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = end; // Ensure final position
        isMoving = false; // Allow next movement
    }
}
