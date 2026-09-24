using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using Unity.Scripting.LifecycleManagement;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[AutoStaticsCleanup]
public partial class PuzzleInteraction : InteractionTrigger
{
    public static Camera puzzleCamera;
    public static Vector3 puzzleLocation = new(100, 0, 0);

    [SerializeField] private List<Puzzle> puzzlePrefabs;
    public KeyCode quitKey = KeyCode.Q;

    public override bool CanInteract => !Solved;
    public bool Solved { get; private set; } = false;
    public event Action OnSolved;

    private Player playerScript;
    private Puzzle activePuzzle;

    private Camera mainCamera;
    private CinemachineCamera mainVirtualCamera;
    private UniversalAdditionalCameraData mainCameraUniversalAdditionalCameraData;
    private int mainVirtualCameraPriority;

    private void Start()
    {
        mainCamera = Camera.main;
        mainCameraUniversalAdditionalCameraData = Camera.main.GetUniversalAdditionalCameraData();
        mainVirtualCamera = (CinemachineCamera)Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera;
        mainVirtualCameraPriority = mainVirtualCamera.Priority;
        playerScript = GameManager.Instance.Player;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        GameManager.Instance.OnPuzzleEnd -= EndGame;
        EndGame(false);
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(quitKey))
            GameManager.Instance.EndCurrentPuzzle(false);
    }

    public override void Trigger(Player interactingPlayer)
    {
        StartGame();
    }

    public void StartGame()
    {
        if (activePuzzle != null)
            return;

        if (puzzleCamera != null)
            Destroy(puzzleCamera.gameObject);

        GameManager.Instance.OnPuzzleEnd += EndGame;
        UIManager.Instance.ControlDisplay.ShowControl(quitKey, "Close");

        // select a random puzzle
        int randomPuzzleIndex = UnityEngine.Random.Range(0, puzzlePrefabs.Count);
        var puzzlePrefab = puzzlePrefabs[randomPuzzleIndex];

        // freeze the player
        playerScript.Freeze("PuzzleInteraction");

        // instantiate puzzle
        activePuzzle = Instantiate(puzzlePrefab, puzzleLocation, Quaternion.identity);

        // create new camera at puzzle
        puzzleCamera = new GameObject("TempCamera").AddComponent<Camera>();
        puzzleCamera.enabled = false;

        // copy same settings from main camera
        puzzleCamera.CopyFrom(mainCamera);

        // set puzzle camera to an overlay render
        puzzleCamera.GetUniversalAdditionalCameraData().renderType = CameraRenderType.Overlay;

        // move puzzle camera to the puzzle
        var bounds = activePuzzle.Bounds;
        float vertFovRads = puzzleCamera.fieldOfView * Mathf.Deg2Rad;
        float distY = (bounds.size.y / 2) / Mathf.Tan(vertFovRads / 2);
        float horiFovRads = Camera.VerticalToHorizontalFieldOfView(puzzleCamera.fieldOfView, puzzleCamera.aspect) * Mathf.Deg2Rad;
        float distX = (bounds.size.x / 2) / Mathf.Tan(horiFovRads / 2);
        float dist = Mathf.Max(distX, distY);
        puzzleCamera.orthographicSize = bounds.size.y / 2;
        puzzleCamera.transform.localPosition = bounds.center + Vector3.back * dist;

        // stack main camera into puzzle camera
        mainCameraUniversalAdditionalCameraData.cameraStack.Add(puzzleCamera);

        // turn on puzzle camera
        puzzleCamera.enabled = true;
        mainVirtualCamera.Priority = -1;
    }

    public void EndGame(bool success)
    {
        GameManager.Instance.OnPuzzleEnd -= EndGame;
        UIManager.Instance.ControlDisplay.RemoveControl(quitKey);
        Solved = success;
        mainVirtualCamera.Priority = mainVirtualCameraPriority;

        // remove puzzle camera
        if (puzzleCamera != null)
        {
            mainCameraUniversalAdditionalCameraData.cameraStack.Remove(puzzleCamera);
            Destroy(puzzleCamera.gameObject);
        }

        // remove puzzle prefab
        if (activePuzzle != null)
            Destroy(activePuzzle.gameObject);

        // resume player
        playerScript.Unfreeze("PuzzleInteraction");

        if (success && TryGetComponent<SpriteRenderer>(out var sprite))
            sprite.color = Color.gray4;

        if (success)
            OnSolved?.Invoke();
    }
}
