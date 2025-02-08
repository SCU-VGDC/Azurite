using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportationSystem : MonoBehaviour
{
    public string destinationScene;
    public GameObject destinationObject;
    private Vector2 destinationCoords;
    [SerializeField] private InteractionTrigger interaction;
    [SerializeField] private Collider2D destinationCameraBorder;

    void Start()
    {
        interaction.OnInteract += Teleport;
        destinationCoords = destinationObject.transform.position;
    }

    public void Teleport()
    {
        if (string.IsNullOrEmpty(destinationScene) || destinationScene == SceneManager.GetActiveScene().name)
        {
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
            SceneManager.LoadScene(destinationScene);
        }
    }

    public void Warp()
    {
        SceneManager.LoadScene(destinationScene);
    }
}
