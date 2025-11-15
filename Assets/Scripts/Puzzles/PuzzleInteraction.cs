using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.Rendering.Universal;
using System.Collections.Generic;

public class PuzzleInteraction : MonoBehaviour
{
    private Player playerScript;
    [SerializeField] private MechanicalRoomManager mechanicalRoomManager;
    [SerializeField] private int puzzleIndex;

    /// <summary>
    /// The index to grab a puzzle prefab from the MechanicalRoomManager's puzzlePrefabSets.
    /// </summary>
    [SerializeField] private int puzzleRoomSetIndex;
    private GameObject instantiatePuzzlePrefab;
    static public Vector3 puzzleLocation = new(100, 0, 0);

    private Camera mainCamera;
    private CinemachineCamera mainVirtualCamera;
    private UniversalAdditionalCameraData mainCameraUniversalAdditionalCameraData;
    private int mainVirtualCameraPriority;
    static public Camera puzzleCamera;

    void Start()
    {
        mainCamera = Camera.main;
        mainCameraUniversalAdditionalCameraData = Camera.main.GetUniversalAdditionalCameraData();
        mainVirtualCamera = (CinemachineCamera)Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera;
        mainVirtualCameraPriority = mainVirtualCamera.Priority;
        playerScript = GameManager.inst.player.GetComponent<Player>();
    }

    private bool PuzzleHasBeenCompletedCheck()
    {
        return PersistentDataManager.Instance.Get<List<List<bool>>>("mechanicalRoomPuzzlesCompleted")[puzzleRoomSetIndex][puzzleIndex];
    }

    public void StartGame()
    {
        if (PuzzleHasBeenCompletedCheck()) return;

        // freeze the player
        playerScript.freezeMovement = true;
        GameManager.inst.paused = true;

        // grab a puzzle prefab from the mechanical room manager
        GameObject puzzlePrefab = mechanicalRoomManager.GetAPuzzle(puzzleRoomSetIndex);

        // instantiate puzzle
        instantiatePuzzlePrefab = Instantiate(puzzlePrefab, puzzleLocation, Quaternion.identity);

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

        GameManager.inst.currentEndGameAction = EndGame;
    }

    public void EndGame()
    {
        mainVirtualCamera.Priority = mainVirtualCameraPriority;

        // remove puzzle camera
        mainCameraUniversalAdditionalCameraData.cameraStack.Remove(puzzleCamera);
        Destroy(puzzleCamera.gameObject);

        // remove puzzle prefab
        Destroy(instantiatePuzzlePrefab);

        // resume player
        playerScript.freezeMovement = false;
        GameManager.inst.paused = false;

        // update the persistent data so the puzzle cannot be completed again
        List<List<bool>> puzzlesCompleted = PersistentDataManager.Instance.Get<List<List<bool>>>("mechanicalRoomPuzzlesCompleted");
        puzzlesCompleted[puzzleRoomSetIndex][puzzleIndex] = true;
        
        PersistentDataManager.Instance.Set("mechanicalRoomPuzzlesCompleted", puzzlesCompleted);
    }
}
