using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportationSystem : MonoBehaviour
{
    // Start is called before the first frame update
    public string destinationScene;
    public Vector2 destinationCoords; //X and Y
    
    public void OnTriggerEnter2D()
    {
        Debug.Log("Teleport Collide");
        PersistentDataScript.instance.SetDestinationCoordinates(destinationCoords.x, destinationCoords.y);
        SceneManager.LoadScene(destinationScene);
    }
}
