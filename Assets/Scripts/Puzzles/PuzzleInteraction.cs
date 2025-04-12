using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PuzzleInteraction : MonoBehaviour
{
    private Movement playerMovement;
    [SerializeField] private InteractionTrigger interaction;

    [SerializeField] private GameObject puzzlePrefab;
    private GameObject instantiatePuzzlePrefab;
    private Vector3 puzzleLocation = new Vector3(100, 0, 0);

    [SerializeField] private CinemachineVirtualCamera mainCamera; 
    public CinemachineVirtualCamera puzzleCamera;

    void Start()
    {
        mainCamera = (CinemachineVirtualCamera)Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera;
        playerMovement = GameManager.inst.player.GetComponent<Movement>();

        interaction.OnInteract += StartGame;
    }

    public void StartGame()
    {
        // freeze the player
        playerMovement.freezeMovement = true;

        // instantiate puzzle
        instantiatePuzzlePrefab = Instantiate(puzzlePrefab, puzzleLocation, Quaternion.identity);

        // create new camera at puzzle
        puzzleCamera = new GameObject("TempVirtualCamera").AddComponent<CinemachineVirtualCamera>();
        puzzleCamera.m_Lens.OrthographicSize = 5;
        puzzleCamera.transform.localPosition = puzzleLocation + new Vector3(0, 0, mainCamera.transform.position.z);
        puzzleCamera.Priority = 10;

        // turn "off" main camera
        mainCamera.Priority = 0;

        GameManager.inst.currentEndGameAction = EndGame;
    }

    public void EndGame()
    {
        playerMovement.freezeMovement = false;

        // turn "on" main camera
        mainCamera.Priority = 10;

        // remove puzzle camera
        puzzleCamera.Priority = 0;
        Destroy(puzzleCamera.gameObject);

        // remove puzzle prefab
        Destroy(instantiatePuzzlePrefab);
    }
}
