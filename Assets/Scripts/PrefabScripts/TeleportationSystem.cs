using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportationSystem : MonoBehaviour
{
    // Start is called before the first frame update
    public string destinationScene;
    public float[] destinationCoords = new float[2]; //X and Y
    void Start()
    {

    }
    public void OnTriggerEnter2D()
    {
        Debug.Log("Teleport Collide");
        PersistentDataScript.instance.SetDestinationCoordinates(destinationCoords[0], destinationCoords[1]);
        SceneManager.LoadScene(destinationScene);

    }
    // Update is called once per frame
    void Update()
    {

    }
}