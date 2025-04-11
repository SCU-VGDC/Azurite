using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;


public class DraggableObject : MonoBehaviour
{
    // Define the size of each tile in grid units.
    private int TileWidth = 1;
    private int TileHeight = 1;
    public Tilemap RestrictedTilemap;
    //private Tilemap RestrictedTilemap;
    public int ObjectWidth = 1;
    public int ObjectHeight = 1;
    public bool DoesSnap = true;
    public float Duration = 0.05f;
    //Axis Lock: Keeps dragging restricted to a single plane of movement
    public enum AxisLock { None, X, Y, Dynamic }
    public AxisLock DirectionLock = AxisLock.None;
    private AxisLock _currentDynamicLock = AxisLock.None;
    private Camera mainCamera;
    private bool IsDragging = false;
    // Using Vector3 for positions but ensuring x/y are rounded when needed.
    private Vector3 TargetPosition;
    private BoxCollider2D BoxCollider;
    private float UpdateTimer;
    public float UpdateCooldown;
    public float MoveTolerance = 0.3f;
    
    void Awake()
    {
        //Allocates tilemap for ease of use. Keeps players from having to specify which tilemap their object interacts with
        if (RestrictedTilemap == null)
        {
            Debug.Log("No Tile Map Found, Choosing First Available");
            RestrictedTilemap = FindObjectOfType<Tilemap>();
        }
        //Allocates BoxCollider
        if (BoxCollider == null)
        {
            BoxCollider = GetComponent<BoxCollider2D>();
        }
        //Updates object size to match specified length and width
        UpdateColliderSize();
    }

    void Start()
    {
        mainCamera = Camera.main;
        if (DoesSnap)
        {
            // Convert the world position to a tile position, snap it, then convert back.
            Vector3 tilePos = WorldToTilemap(transform.position);
            tilePos = SnapToGrid(tilePos);
            transform.position = TilemapToWorld(tilePos);
        }
    }

    void OnMouseDown()
    {
        //Stores the position of the tile that the object is on when the mouse is clicked.
        IsDragging = true;
        TargetPosition = WorldToTilemap(transform.position);
        TargetPosition = new Vector3(Mathf.RoundToInt(TargetPosition.x), Mathf.RoundToInt(TargetPosition.y), 0);
    }

    void OnMouseDrag()
    {
        if (!IsDragging)
        {
            //Breaks the control if the mouse is no longer dragging the box
            return;
        }
        UpdateTimer -= Time.deltaTime;
        if (UpdateTimer > 0f)
        {
            return;
        }

        UpdateTimer = UpdateCooldown;

        //Gets coordinates of the mouse to drag box to position
        Vector3 rawMouseTilePos = WorldToTilemap(GetMouseWorldPosition());
        Vector3 mouseTilePos = new Vector3(Mathf.RoundToInt(rawMouseTilePos.x), Mathf.RoundToInt(rawMouseTilePos.y), 0);
        
        // Use the raw position for a tolerance check
        if (Vector3.Distance(rawMouseTilePos, TargetPosition) < MoveTolerance)
        {
            // The mouse hasn't moved enough – no update.
            return;
        }
        // Apply axis locking:
        if (DirectionLock == AxisLock.Dynamic)
        {
            // If no axis is chosen yet, pick one based on which delta is greater.
            if (_currentDynamicLock == AxisLock.None)
            {
                float deltaX = Mathf.Abs(mouseTilePos.x - TargetPosition.x);
                float deltaY = Mathf.Abs(mouseTilePos.y - TargetPosition.y);
                _currentDynamicLock = (deltaX > deltaY) ? AxisLock.X : AxisLock.Y;
            }

            // Now use the cached dynamic lock to clamp movement.
            // If the mouse is not "over" the box on the locked axis, keep the lock.
            if (_currentDynamicLock == AxisLock.X)
            {
                // For axis X locked, we fix Y.
                // Check whether the raw mouse Y is within half the object's height.
                if (Mathf.Abs(rawMouseTilePos.y - TargetPosition.y) < (ObjectHeight / 2f))
                {
                    // Mouse is hovering over the block's vertical extent – reset the dynamic lock.
                    _currentDynamicLock = AxisLock.None;
                }
                else
                {
                    mouseTilePos.y = TargetPosition.y;
                }
            }
            else if (_currentDynamicLock == AxisLock.Y)
            {
                // For axis Y locked, we fix X.
                // Check whether the raw mouse X is within half the object's width.
                if (Mathf.Abs(rawMouseTilePos.x - TargetPosition.x) < (ObjectWidth / 2f))
                {
                    // Mouse is hovering over the block's horizontal extent – reset the dynamic lock.
                    _currentDynamicLock = AxisLock.None;
                }
                else
                {
                    mouseTilePos.x = TargetPosition.x;
                }
            }
        }
        else if (DirectionLock == AxisLock.X)
        {
            // Lock the Y position so only X changes
            mouseTilePos.y = TargetPosition.y;
        }
        else if (DirectionLock == AxisLock.Y)
        {
            // Lock the X position so only Y changes
            mouseTilePos.x = TargetPosition.x;
        }
        //Determines position of the mouse and the directionality in order to determine whether the move is valid
        Vector3 snappedTile = SnapToGrid(mouseTilePos);
        Vector3 direction = snappedTile - TargetPosition;

        // Pass the direction to IsMoveValid
        if (snappedTile != TargetPosition && IsMoveValid(snappedTile, direction))
        {
            //Actually moves tiles
            StartCoroutine(MoveStepByStep(snappedTile));
        }
    }

    void OnMouseUp()
    {
        IsDragging = false;
    }

    IEnumerator MoveSmoothly(Vector3 targetWorldPosition, float duration)
    {
        //Smooth movement for moving tile. Moves over a set period of time to look good.
        Vector3 startPosition = transform.position;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            //Transforms the object to the end position
            transform.position = Vector3.Lerp(startPosition, targetWorldPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = targetWorldPosition;
    }

    IEnumerator MoveStepByStep(Vector3 finalTilePosition)
    {
        // Figures out the current position and steps needed to arrive at the final destination.
        Vector3 currentTile = WorldToTilemap(transform.position);
        currentTile = new Vector3(Mathf.RoundToInt(currentTile.x), Mathf.RoundToInt(currentTile.y), 0);
        Vector3 direction = finalTilePosition - currentTile;
        int steps = Mathf.Max(Mathf.Abs(Mathf.RoundToInt(direction.x)), Mathf.Abs(Mathf.RoundToInt(direction.y)));

        for (int i = 0; i < steps; i++)
        {
            //Iterates through current position to final tile position.
            Vector3 nextStep = currentTile + new Vector3(
                Mathf.Clamp(Mathf.RoundToInt(direction.x), -1, 1),
                Mathf.Clamp(Mathf.RoundToInt(direction.y), -1, 1),
                0
            );

            // Pass the direction to IsMoveValid.
            if (!IsMoveValid(nextStep, direction))
            {
                break; // Stop if the next tile is invalid
            }

            // Convert the tile position to world position.
            Vector3 targetWorldPos = TilemapToWorld(nextStep);

            // Apply the offset for even dimensions based on the direction of movement. This ensures that any sized tile remains in tilemap
            if (ObjectWidth % 2 == 0)
            {
                targetWorldPos.x += (direction.x > 0) ? -0.5f : 0.5f;
            }
            if (ObjectHeight % 2 == 0)
            {
                targetWorldPos.y += (direction.y > 0) ? -0.5f : 0.5f;
            }

            // Move smoothly to the target position.
            yield return transform.DOMove(targetWorldPos, Duration).WaitForCompletion();
            //yield return StartCoroutine(MoveSmoothly(targetWorldPos, 0.05f)); // Adjust duration as needed
            currentTile = nextStep;
        }

        TargetPosition = currentTile; //Updates target position to the current tile for future movement
    }

    Vector3 GetMouseWorldPosition()
    {
        //Gets the position of the mouse on the screen.
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = Mathf.Abs(mainCamera.transform.position.z - transform.position.z);
        return mainCamera.ScreenToWorldPoint(mouseScreenPos);
    }

    public Vector3 SnapToGrid(Vector3 tilePosition)
    {
        // Snap the position to the grid by rounding x and y to the nearest tile boundaries.
        int snappedX = Mathf.RoundToInt(tilePosition.x / (float)TileWidth) * TileWidth;
        int snappedY = Mathf.RoundToInt(tilePosition.y / (float)TileHeight) * TileHeight;
        return new Vector3(snappedX, snappedY, tilePosition.z);
    }

    bool IsMoveValid(Vector3 tilePosition, Vector3 direction)
    {
        if (RestrictedTilemap == null) return true;

        // Handle .5 increments by rounding to the nearest integer.
        int centerCellX = Mathf.FloorToInt(tilePosition.x);
        int centerCellY = Mathf.FloorToInt(tilePosition.y);

        // Adjust for even object dimensions based on the direction of movement.
        int offsetX = 0;
        int offsetY = 0;
        
        if (ObjectWidth % 2 == 0)
        {
            // If moving right, offset to the left; if moving left, offset to the right.
            offsetX = (direction.x > 0) ? -3 : 1; //Ensures that ISMoveValid() checks for proper spacing of tile and wall.
        }
        if (ObjectHeight % 2 == 0)
        {
            // If moving up, offset downward; if moving down, offset upward.
            offsetY = (direction.y > 0) ? -3 : 1; //Ensures that ISMoveValid() checks for proper spacing of tile and wall.
        }

        // Calculate the bottom-left cell based on the object's dimensions and direction.
        int bottomLeftCellX = centerCellX - ((ObjectWidth - 1) / 2) + (offsetX / 2);
        int bottomLeftCellY = centerCellY - ((ObjectHeight - 1) / 2) + (offsetY / 2);

        // Loop through the object's tiles.
        for (int x = 0; x < ObjectWidth; x++)
        {
            for (int y = 0; y < ObjectHeight; y++)
            {
                Vector3Int cellToCheck = new Vector3Int(bottomLeftCellX + x, bottomLeftCellY + y, 0);
                if (RestrictedTilemap.GetTile(cellToCheck) != null)
                {
                    return false; // Prevents movement into restricted tiles.
                }

                // Also check for any players occupying this cell.
                GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
                //Makes sure there is no collision with any player objects
                foreach (GameObject player in players)
                {
                    Vector3Int playerCell = RestrictedTilemap.WorldToCell(player.transform.position);
                    if (cellToCheck == playerCell)
                    {
                        return false;
                    }
                }
            }
        }

        return true;
    }
    void OnMouseEnter()
    {
        if (IsDragging && DirectionLock == AxisLock.Dynamic)
        {
            _currentDynamicLock = AxisLock.None;
        }
    }
    public void OnValidate()
    {
        //Updates size of game object every time a change to the object is made
        /*if (DoesSnap)
        {
            Vector3 tilePos = WorldToTilemap(transform.position);
            tilePos = SnapToGrid(tilePos);
            transform.position = TilemapToWorld(tilePos);
        }*/
        UpdateColliderSize();
    }

    
    private void UpdateColliderSize()
    {
        // Adjust the collider and sprite scale based on the new tile dimensions
        if (BoxCollider == null)
            BoxCollider = GetComponent<BoxCollider2D>();

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
            return;

        Vector2 spriteSize = spriteRenderer.sprite.bounds.size; // Original sprite size

        // Scale the sprite to match the grid tile dimensions
        transform.localScale = new Vector3(
            (ObjectWidth * TileWidth) / spriteSize.x,
            (ObjectHeight * TileHeight) / spriteSize.y,
            1
        );
    }

    
    public Vector3 WorldToTilemap(Vector3 worldPos)
    {
        // Converts a world position to a tilemap cell position
        Vector3Int cellPos = RestrictedTilemap.WorldToCell(worldPos);
        return new Vector3(cellPos.x, cellPos.y, cellPos.z);
    }

    public Vector3 TilemapToWorld(Vector3 tilePos)
    {
        // Converts a tilemap cell position (Vector3) back to a world position
        Vector3Int cellPos = new Vector3Int(
            Mathf.RoundToInt(tilePos.x),
            Mathf.RoundToInt(tilePos.y),
            Mathf.RoundToInt(tilePos.z)
        );
        return RestrictedTilemap.GetCellCenterWorld(cellPos);
    }
}
