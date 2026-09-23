using UnityEngine;

public class TilePlayerController : MonoBehaviour
{
    [Tooltip("The maximum amount of tiles the player can travel.")]
    public int maxScanDistance = 25;

    [Tooltip("The goal hitbox.")]
    public Collider2D goalCollider = null;

    [Tooltip("Whether or not the puzzle has been completed.")]
    public bool puzzleComplete = false;

    [Tooltip("The duration of the slide animation in seconds.")]
    public float slideDuration = 0.5f;

    [Tooltip("Time delay between continuous movement steps.")]
    public float moveDelay = 0.1f;

    private Vector3 startPos = Vector3.zero;
    private Vector3 finalPos = Vector3.zero;
    private float animationTime = 1;
    private float moveTimer = 0;
    private Vector2Int moveDirection = Vector2Int.zero;

    private void Update()
    {
        if (animationTime < 1)
        {
            animationTime += Time.deltaTime / slideDuration;
            if (animationTime > 1) animationTime = 1;

            float interpolatedTime = animationTime * animationTime * animationTime *
                                     (animationTime * (6f * animationTime - 15f) + 10f);

            transform.position = new Vector3(
                (finalPos.x - startPos.x) * interpolatedTime + startPos.x,
                (finalPos.y - startPos.y) * interpolatedTime + startPos.y,
                0
            );
        }

        if (puzzleComplete) return;

        moveTimer -= Time.deltaTime;

        Vector2Int newDirection = Vector2Int.zero;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) newDirection = Vector2Int.up;
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) newDirection = Vector2Int.down;
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) newDirection = Vector2Int.left;
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) newDirection = Vector2Int.right;

        if (newDirection != Vector2Int.zero)
        {
            if (newDirection != moveDirection || moveTimer <= 0)
            {
                moveDirection = newDirection;
                moveTimer = moveDelay;
                Move(moveDirection);
            }
        }
        else
        {
            moveDirection = Vector2Int.zero;
        }
    }

    private async void GoalComplete()
    {
        Debug.Log("Goal Completed!");
        await Awaitable.WaitForSecondsAsync(1);
        GameManager.Instance.EndCurrentPuzzle(true);
    }

    private void Move(Vector2Int direction)
    {
        if (animationTime < 1) return;

        Vector2Int position = new((int)transform.position.x, (int)transform.position.y);
        RaycastHit2D[] raycasts = null;

        for (int i = 0; i < maxScanDistance && (raycasts = Physics2D.LinecastAll(position, position + direction)).Length == 0; ++i)
        {
            position += direction;
        }

        for (int i = 0; i < raycasts.Length; ++i)
        {
            if (raycasts[i].collider == goalCollider)
            {
                puzzleComplete = true;
                position += direction;
                GoalComplete();
                break;
            }
        }

        animationTime = 0;
        startPos = transform.position;
        finalPos = (Vector3Int)position;
    }
}
