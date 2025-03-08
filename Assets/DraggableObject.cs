using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DraggableObject : MonoBehaviour
{
    // Define the size of each tile in grid units.
    public int tileWidth = 1;
    public int tileHeight = 1;

    public Tilemap restrictedTilemap;
    public int objectWidth = 1;
    public int objectHeight = 1;
    public bool snapToGrid = true;

    private Camera mainCamera;
    private bool isDragging = false;
    // Using Vector3 for positions but ensuring x/y are rounded when needed.
    private Vector3 targetPosition;

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
            // Convert the world position to a tile position, snap it, then convert back.
            Vector3 tilePos = WorldToTilemap(transform.position);
            tilePos = SnapToGrid(tilePos);
            transform.position = TilemapToWorld(tilePos);
        }
    }

    void OnMouseDown()
    {
        if (mainCamera == null) return;

        isDragging = true;
        // Round the tile position values when storing targetPosition.
        targetPosition = WorldToTilemap(transform.position);
        targetPosition = new Vector3(Mathf.RoundToInt(targetPosition.x), Mathf.RoundToInt(targetPosition.y), 0);
    }

    void OnMouseDrag()
    {
        if (!isDragging) return;

        Vector3 mouseTilePos = WorldToTilemap(GetMouseWorldPosition());
        mouseTilePos = new Vector3(Mathf.RoundToInt(mouseTilePos.x), Mathf.RoundToInt(mouseTilePos.y), 0);
        Vector3 snappedTile = SnapToGrid(mouseTilePos);

        // Calculate the direction of movement.
        Vector3 direction = snappedTile - targetPosition;

        // Pass the direction to IsMoveValid.
        if (snappedTile != targetPosition && IsMoveValid(snappedTile, direction))
        {
            StartCoroutine(MoveStepByStep(snappedTile));
        }
    }

    void OnMouseUp()
    {
        isDragging = false;
    }

    IEnumerator MoveSmoothly(Vector3 targetWorldPosition, float duration)
    {
        Vector3 startPosition = transform.position;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetWorldPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = targetWorldPosition;
    }

    IEnumerator MoveStepByStep(Vector3 finalTilePosition)
    {
        // Start with a rounded current tile position.
        Vector3 currentTile = WorldToTilemap(transform.position);
        currentTile = new Vector3(Mathf.RoundToInt(currentTile.x), Mathf.RoundToInt(currentTile.y), 0);
        Vector3 direction = finalTilePosition - currentTile;
        int steps = Mathf.Max(Mathf.Abs(Mathf.RoundToInt(direction.x)), Mathf.Abs(Mathf.RoundToInt(direction.y)));

        for (int i = 0; i < steps; i++)
        {
            Vector3 nextStep = currentTile + new Vector3(
                Mathf.Clamp(Mathf.RoundToInt(direction.x), -1, 1),
                Mathf.Clamp(Mathf.RoundToInt(direction.y), -1, 1),
                0
            );

            // Pass the direction to IsMoveValid.
            if (!IsMoveValid(nextStep, direction))
                break; // Stop if the next tile is invalid

            // Convert the tile position to world position.
            Vector3 targetWorldPos = TilemapToWorld(nextStep);

            // Apply the offset for even dimensions based on the direction of movement.
            if (objectWidth % 2 == 0)
            {
                targetWorldPos.x += (direction.x > 0) ? -0.5f : 0.5f;
            }
            if (objectHeight % 2 == 0)
            {
                targetWorldPos.y += (direction.y > 0) ? -0.5f : 0.5f;
            }

            // Move smoothly to the target position.
            yield return StartCoroutine(MoveSmoothly(targetWorldPos, 0.05f)); // Adjust duration as needed
            currentTile = nextStep;
        }

        targetPosition = currentTile;
    }

    Vector3 GetMouseWorldPosition()
    {
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = Mathf.Abs(mainCamera.transform.position.z - transform.position.z);
        return mainCamera.ScreenToWorldPoint(mouseScreenPos);
    }

    // Snap the position to the grid by rounding x and y to the nearest tile boundaries.
    public Vector3 SnapToGrid(Vector3 tilePosition)
    {
        int snappedX = Mathf.RoundToInt(tilePosition.x / (float)tileWidth) * tileWidth;
        int snappedY = Mathf.RoundToInt(tilePosition.y / (float)tileHeight) * tileHeight;
        return new Vector3(snappedX, snappedY, tilePosition.z);
    }

    bool IsMoveValid(Vector3 tilePosition, Vector3 direction)
    {
        if (restrictedTilemap == null) return true;

        // Handle .5 increments by rounding to the nearest integer.
        int centerCellX = Mathf.FloorToInt(tilePosition.x);
        int centerCellY = Mathf.FloorToInt(tilePosition.y);

        // Adjust for even object dimensions based on the direction of movement.
        int offsetX = 0;
        int offsetY = 0;

        if (objectWidth % 2 == 0)
        {
            // If moving right, offset to the left; if moving left, offset to the right.
            offsetX = (direction.x > 0) ? -1 : 1;
        }
        if (objectHeight % 2 == 0)
        {
            // If moving up, offset downward; if moving down, offset upward.
            offsetY = (direction.y > 0) ? -1 : 1;
        }

        // Calculate the bottom-left cell based on the object's dimensions and direction.
        int bottomLeftCellX = centerCellX - ((objectWidth - 1) / 2) + (offsetX / 2);
        int bottomLeftCellY = centerCellY - ((objectHeight - 1) / 2) + (offsetY / 2);

        // Loop through the object's tiles.
        for (int x = 0; x < objectWidth; x++)
        {
            for (int y = 0; y < objectHeight; y++)
            {
                Vector3Int cellToCheck = new Vector3Int(bottomLeftCellX + x, bottomLeftCellY + y, 0);
                if (restrictedTilemap.GetTile(cellToCheck) != null)
                {
                    return false; // Prevents movement into restricted tiles.
                }
            }
        }
        return true;
    }

    public void OnValidate()
    {
        if (snapToGrid)
        {
            Vector3 tilePos = WorldToTilemap(transform.position);
            tilePos = SnapToGrid(tilePos);
            transform.position = TilemapToWorld(tilePos);
        }
        UpdateColliderSize();
    }

    // Adjust the collider and sprite scale based on the new tile dimensions.
    private void UpdateColliderSize()
    {
        if (boxCollider == null)
            boxCollider = GetComponent<BoxCollider2D>();

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
            return;

        Vector2 spriteSize = spriteRenderer.sprite.bounds.size; // Original sprite size

        // Scale the sprite to match the grid tile dimensions.
        transform.localScale = new Vector3(
            (objectWidth * tileWidth) / spriteSize.x,
            (objectHeight * tileHeight) / spriteSize.y,
            1
        );

        // Update the BoxCollider2D size to match the new object dimensions.
        boxCollider.size = new Vector2(objectWidth * tileWidth, objectHeight * tileHeight);
        boxCollider.offset = Vector2.zero;
    }

    // Converts a world position to a tilemap cell position (as a Vector3 with integer x/y values).
    public Vector3 WorldToTilemap(Vector3 worldPos)
    {
        Vector3Int cellPos = restrictedTilemap.WorldToCell(worldPos);
        return new Vector3(cellPos.x, cellPos.y, cellPos.z);
    }

    // Converts a tilemap cell position (Vector3) back to a world position.
    public Vector3 TilemapToWorld(Vector3 tilePos)
    {
        Vector3Int cellPos = new Vector3Int(
            Mathf.RoundToInt(tilePos.x),
            Mathf.RoundToInt(tilePos.y),
            Mathf.RoundToInt(tilePos.z)
        );
        return restrictedTilemap.GetCellCenterWorld(cellPos);
    }
}