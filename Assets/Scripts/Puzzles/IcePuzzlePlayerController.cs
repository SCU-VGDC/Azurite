using DG.Tweening;
using UnityEngine;

public class IcePuzzlePlayerController : MonoBehaviour
{
    /// <summary>The maximum amount of tiles the player can travel.</summary>
    [Tooltip("The maximum amount of tiles the player can travel.")]
    public int maxScanDistance = 25;
    /// <summary>The goal hitbox.</summary>
    [Tooltip("The goal hitbox.")]
    public Collider2D goalCollider = null;
    /// <summary>The duration of the slide animation in seconds.</summary>
    [Tooltip("The duration of the slide animation in seconds.")]
    public float slideDuration = 0.5f;

    private Tween slide;
    private bool solved = false;

    private void Start()
    {
        // snap the player to the grid! round, not truncate
        transform.position = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), Mathf.Round(transform.position.z));
    }

    private void Update()
    {
        // Player movement is controlled by WASD or arrow keys.
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            Move(Vector2Int.up);
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            Move(Vector2Int.down);
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Move(Vector2Int.left);
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            Move(Vector2Int.right);
        }
    }

    private void OnDestroy()
    {
        slide?.Kill();
    }

    private async void OnSolve()
    {
        if (solved)
            return;
        solved = true;

        await Awaitable.WaitForSecondsAsync(1);
        GameManager.Instance.EndCurrentPuzzle(true);
    }

    /// <summary>
    /// Move the player in a straight line.
    /// </summary>
    /// <param name="direction">The unit vector representing the player's driection.</param>
    private void Move(Vector2Int direction)
    {
        if (slide != null && slide.active)
            return;

        if (solved)
            return;

        Vector2Int position = new((int)transform.position.x, (int)transform.position.y);
        RaycastHit2D[] raycasts = null;

        // Continually move the player by one tile until a wall has been hit or the player has moved the maximum amount of tiles.
        for (int i = 0; i < maxScanDistance && (raycasts = Physics2D.LinecastAll(position, position + direction)).Length == 0; ++i)
        {
            position += direction;
        }

        // Check if the wall hit is the goal.
        // If so, move the player one tile more and complete the puzzle.
        for (int i = 0; i < raycasts.Length; ++i)
        {
            if (raycasts[i].collider == goalCollider)
            {
                OnSolve();
                position += direction;
                break;
            }
        }

        // Start the slide animation.
        slide = transform.DOMove((Vector3)(Vector3Int)position, slideDuration).SetEase(Ease.OutQuart);
    }
}