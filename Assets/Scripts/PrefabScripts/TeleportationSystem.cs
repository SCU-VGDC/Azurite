using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportationSystem : MonoBehaviour
{
    // Start is called before the first frame update
    public string destinationScene;
    //public float[] destinationCoords = new float[2]; //X and Y
    public Vector2 destinationCoords;
    void Start()
    {

    }
    public void OnTriggerEnter2D()
    {
        

    }
    public void Warp()
    {
        Debug.Log("Teleport Collide");
        PersistentDataScript.instance.SetDestinationCoordinates(destinationCoords);
        SceneManager.LoadScene(destinationScene);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
