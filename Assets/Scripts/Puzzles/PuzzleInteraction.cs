using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.Rendering.Universal;
using System.Collections.Generic;

public class PuzzleInteraction : MonoBehaviour
{
    private Player playerScript;
    [SerializeField] private List<GameObject> puzzlePrefabs;
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

    public void StartGame()
    {
        // select a random puzzle
        int randomPuzzleIndex = Random.Range(0, puzzlePrefabs.Count);
        GameObject puzzlePrefab = puzzlePrefabs[randomPuzzleIndex];

        // freeze the player
        playerScript.freezeMovement = true;
        GameManager.inst.paused = true;

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
    }
}
