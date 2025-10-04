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

    void Update()
    {
        if (this.animationTime < 1)
        {
            this.animationTime += Time.deltaTime / this.slideDuration;
            if (this.animationTime > 1) this.animationTime = 1;

            float interpolatedTime = this.animationTime * this.animationTime * this.animationTime *
                                     (this.animationTime * (6f * this.animationTime - 15f) + 10f);

            this.transform.position = new Vector3(
                (this.finalPos.x - this.startPos.x) * interpolatedTime + this.startPos.x,
                (this.finalPos.y - this.startPos.y) * interpolatedTime + this.startPos.y,
                0
            );
        }

        if (this.puzzleComplete) return;

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
    void GoalComplete()
    {
        Debug.Log("Goal Completed!");

        StartCoroutine(GameManager.inst.Sleep(1.0f, GameManager.inst.EndCurrentPuzzle));
    }
    private void Move(Vector2Int direction)
    {
        if (this.animationTime < 1) return;

        Vector2Int position = new Vector2Int((int)this.transform.position.x, (int)this.transform.position.y);
        RaycastHit2D[] raycasts = null;

        for (int i = 0; i < this.maxScanDistance && (raycasts = Physics2D.LinecastAll(position, position + direction)).Length == 0; ++i)
        {
            position += direction;
        }

        for (int i = 0; i < raycasts.Length; ++i)
        {
            if (raycasts[i].collider == this.goalCollider)
            {
                this.puzzleComplete = true;
                position += direction;
                GoalComplete();
                break;
            }
        }

        this.animationTime = 0;
        this.startPos = this.transform.position;
        this.finalPos = (Vector3Int)position;
    }
}
