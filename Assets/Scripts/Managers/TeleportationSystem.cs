using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportationSystem : MonoBehaviour
{
    // Start is called before the first frame update
    public string destinationScene;
    [SerializeField] private InteractionTrigger interaction;

    void Start()
    {
        interaction.playerInteractEvent.AddListener(Teleport);

        if (GameManager.Instance == null)
        {
            Debug.LogWarning("GameManager didn't exist on scene startup!");
            return;
        }

        if (!string.IsNullOrEmpty(destinationScene) && GameManager.Instance.PreviousScene == destinationScene)
        {
            GameManager.Instance.Player.transform.position = transform.position;
            GameManager.Instance.MainCameraContainer.GetComponentInChildren<CinemachineCamera>().ForceCameraPosition(GameManager.Instance.Player.transform.position, Quaternion.identity);
        }
    }
    
    public void Teleport(Player player)
    {
        if(string.IsNullOrEmpty(destinationScene) || destinationScene == SceneManager.GetActiveScene().name)
        {
            if (player != null)
            {
                //player.transform.position = new Vector3(destinationCoords.x, destinationCoords.y, player.transform.position.z);
                GameManager.Instance.MainCameraContainer.GetComponentInChildren<CinemachineCamera>().ForceCameraPosition(player.transform.position, Quaternion.identity);
            }
        }
        else
        {
            SceneManager.LoadScene(destinationScene);
            PersistentDataManager.Instance.Set("currentLocation", destinationScene);
        }
    }
}
