using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportationSystem : MonoBehaviour
{
    // Start is called before the first frame update
    public string destinationScene;
    private Vector2 destinationCoords;
    [SerializeField] private InteractionTrigger interaction;

    void Start()
    {
        interaction.onInteract.AddListener(Teleport);

        if (GameManager.inst == null)
        {
            Debug.LogWarning("GameManager didn't exist on scene startup!");
            return;
        }

        if (!string.IsNullOrEmpty(destinationScene) && GameManager.inst.PreviousScene == destinationScene)
        {
            GameManager.inst.player.transform.position = transform.position;
        }
    }
    
    public void Teleport()
    {
        if (string.IsNullOrEmpty(destinationScene) || destinationScene == SceneManager.GetActiveScene().name)
        {
            Player player = GameManager.inst.player;
            if (player != null)
            {
                // teleport the player!
                player.transform.position = new Vector3(destinationCoords.x, destinationCoords.y, player.transform.position.z);
            }
        }
        else
        {
            SceneManager.LoadScene(destinationScene);
        }
    }
}
