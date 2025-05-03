using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportationSystem : MonoBehaviour
{
    // Start is called before the first frame update
    public string destinationScene;
    public GameObject destinationObject;
    private Vector2 destinationCoords;
    [SerializeField] private InteractionTrigger interaction;
    [SerializeField] private Collider2D destinationCameraBorder;

    void Start()
    {
        interaction.onInteract.AddListener(this.Teleport);

        destinationCoords = destinationObject.transform.position;

    }
    public void Teleport()
    {
        if (string.IsNullOrEmpty(destinationScene) || destinationScene == SceneManager.GetActiveScene().name)
        {
            Debug.Log("Teleporting player within the same scene.");
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                // teleport the player!
                player.transform.position = new Vector3(destinationCoords.x, destinationCoords.y, player.transform.position.z);

                // change camera's border
                GameObject.FindGameObjectWithTag("Virtual Camera").GetComponent<CinemachineConfiner2D>().m_BoundingShape2D = destinationCameraBorder;
            }

        }
        else
        {
            Debug.Log("Teleport Collide");
            PersistentDataScript.instance.SetDestinationCoordinates(destinationCoords);
            SceneManager.LoadScene(destinationScene);
        }

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
