using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PuzzleInteraction : MonoBehaviour
{
    private Movement playerMovement;
    [SerializeField] private InteractionTrigger interaction;

    [SerializeField] private GameObject puzzlePrefab;
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
        Instantiate(puzzlePrefab, puzzleLocation, Quaternion.identity);

        // create new camera at puzzle
        puzzleCamera = new GameObject("TempVirtualCamera").AddComponent<CinemachineVirtualCamera>();
        puzzleCamera.m_Lens.OrthographicSize = 5;
        puzzleCamera.transform.localPosition = puzzleLocation;
        puzzleCamera.Priority = 10;

        // turn "off" main camera
        mainCamera.Priority = 0;
    }

    public void EndGame()
    {
        playerMovement.freezeMovement = false;

        // turn "on" main camera
        mainCamera.Priority = 10;

        // remove puzzle camera
        puzzleCamera.Priority = 0;
        Destroy(puzzleCamera);

        // remove puzzle prefab
        Destroy(puzzlePrefab);
    }
}
