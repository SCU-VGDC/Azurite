using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DraggableObject : MonoBehaviour
{
    public int gridSize = 1;  // Now an integer to align with Vector3Int
    public Tilemap restrictedTilemap;
    public int objectWidth = 2;
    public int objectHeight = 2;
    public bool snapToGrid = true;

    private Camera mainCamera;
    private bool isDragging = false;
    private Vector3Int targetPosition;

    private BoxCollider2D boxCollider;

    void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();

        if (boxCollider == null)
            boxCollider = gameObject.AddComponent<BoxCollider2D>();

        UpdateColliderSize();
    }

    void Start()
    {
        mainCamera = Camera.main;
        if (snapToGrid)
        {
            transform.position = TilemapToWorld(SnapToGrid(WorldToTilemap(transform.position)));
        }
    }

    void OnMouseDown()
    {
        if (mainCamera == null) return;

        isDragging = true;
        targetPosition = WorldToTilemap(transform.position);
    }

    void OnMouseDrag()
    {
        if (!isDragging) return;

        Vector3Int mousePosTile = WorldToTilemap(GetMouseWorldPosition());
        Vector3Int snappedTile = SnapToGrid(mousePosTile);

        if (snappedTile != targetPosition && IsMoveValid(snappedTile))
        {
            StartCoroutine(MoveStepByStep(snappedTile));
        }
    }

    void OnMouseUp()
    {
        isDragging = false;
    }

    IEnumerator MoveStepByStep(Vector3Int finalTilePosition)
    {
        Vector3Int currentTile = WorldToTilemap(transform.position);
        Vector3Int direction = (finalTilePosition - currentTile);
        int steps = Mathf.Max(Mathf.Abs(direction.x), Mathf.Abs(direction.y));

        for (int i = 0; i < steps; i++)
        {
            Vector3Int nextStep = currentTile + new Vector3Int(
                Mathf.Clamp(direction.x, -1, 1),
                Mathf.Clamp(direction.y, -1, 1),
                0
            );

            if (!IsMoveValid(nextStep))
                break; // Stop if the next tile is invalid

            transform.position = TilemapToWorld(nextStep);
            currentTile = nextStep;
            yield return new WaitForSeconds(0.05f); // Simulates step-by-step movement
        }
    }

    Vector3 GetMouseWorldPosition()
    {
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = Mathf.Abs(mainCamera.transform.position.z - transform.position.z);
        return mainCamera.ScreenToWorldPoint(mouseScreenPos);
    }

    public Vector3Int SnapToGrid(Vector3Int tilePosition)
    {
        int snappedX = Mathf.RoundToInt(tilePosition.x / (float)gridSize) * gridSize;
        int snappedY = Mathf.RoundToInt(tilePosition.y / (float)gridSize) * gridSize;
        return new Vector3Int(snappedX, snappedY, 0);
    }

    bool IsMoveValid(Vector3Int tilePosition)
    {
        if (restrictedTilemap == null) return true;

        Vector3Int bottomLeftCell = tilePosition - new Vector3Int((objectWidth - 1) / 2, (objectHeight - 1) / 2, 0);

        for (int x = 0; x < objectWidth; x++)
        {
            for (int y = 0; y < objectHeight; y++)
            {
                Vector3Int cellToCheck = bottomLeftCell + new Vector3Int(x, y, 0);
                if (restrictedTilemap.GetTile(cellToCheck) != null)
                {
                    return false; // Prevents movement into restricted tiles
                }
            }
        }

        return true;
    }

    public void OnValidate()
    {
        if (snapToGrid)
        {
            transform.position = TilemapToWorld(SnapToGrid(WorldToTilemap(transform.position)));
        }
        UpdateColliderSize();
    }

    private void UpdateColliderSize()
    {
        if (boxCollider == null)
            boxCollider = GetComponent<BoxCollider2D>();

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
            return;

        Vector2 spriteSize = spriteRenderer.sprite.bounds.size; // Get original sprite size

        // Scale the sprite to match grid size
        transform.localScale = new Vector3(
            (objectWidth * gridSize) / spriteSize.x,
            (objectHeight * gridSize) / spriteSize.y,
            1
        );

        // Update BoxCollider2D size to match new object dimensions
        boxCollider.size = new Vector2(objectWidth * gridSize, objectHeight * gridSize);
        boxCollider.offset = Vector2.zero; // Ensure proper alignment
    }


    // **Helper functions to convert between world space and tile space**
    public Vector3Int WorldToTilemap(Vector3 worldPos)
    {
        return restrictedTilemap.WorldToCell(worldPos);
    }

    public Vector3 TilemapToWorld(Vector3Int tilePos)
    {
        return restrictedTilemap.GetCellCenterWorld(tilePos);
    }
}
