using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(InteractionTrigger))]
public class TeleportationSystem : MonoBehaviour
{
    public string destinationScene;

    private void Start()
    {
        GetComponent<InteractionTrigger>().playerInteractEvent.AddListener(Teleport);

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

    private void OnDestroy()
    {
        GetComponent<InteractionTrigger>().playerInteractEvent.RemoveListener(Teleport);
    }

    public void Teleport(Player player)
    {
        UIManager.Instance.SetTransitionVisible(true).OnComplete(() => SceneManager.LoadScene(destinationScene));
    }
}
