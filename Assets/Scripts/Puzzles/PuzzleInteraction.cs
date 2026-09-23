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

    public override bool CanInteract => !Solved;
    public bool Solved { get; private set; } = false;

    private Player playerScript;
    [SerializeField] private List<GameObject> puzzlePrefabs;
    private GameObject activePuzzle;

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
        GameManager.Instance.OnPuzzleEnd += EndGame;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        GameManager.Instance.OnPuzzleEnd -= EndGame;
        EndGame(false);
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

        // select a random puzzle
        int randomPuzzleIndex = UnityEngine.Random.Range(0, puzzlePrefabs.Count);
        GameObject puzzlePrefab = puzzlePrefabs[randomPuzzleIndex];

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
        puzzleCamera.transform.localPosition = puzzleLocation + new Vector3(0, 0, mainVirtualCamera.transform.position.z);

        // stack main camera into puzzle camera
        mainCameraUniversalAdditionalCameraData.cameraStack.Add(puzzleCamera);

        // turn on puzzle camera
        puzzleCamera.enabled = true;
        mainVirtualCamera.Priority = -1;
    }

    public void EndGame(bool success)
    {
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
            Destroy(activePuzzle);

        // resume player
        playerScript.Unfreeze("PuzzleInteraction");

        if (success && TryGetComponent<SpriteRenderer>(out var sprite))
            sprite.color = Color.gray4;
    }
}
