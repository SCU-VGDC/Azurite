using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PuzzleInteraction : MonoBehaviour
{
    private Movement playerMovement;
    [SerializeField] private InteractionTrigger interaction;

    [SerializeField] private GameObject puzzlePrefab;
    [SerializeField] private Vector3 puzzleLocation = new Vector3(100, 0, 0);

    [SerializeField] private CinemachineVirtualCamera mainCamera; 
    public CinemachineVirtualCamera puzzleCamera;

    void Start()
    {
        mainCamera = Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera;
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
        Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera
    }

    public void EndGame()
    {
        playerMovement.freezeMovement = false;
    }
}
