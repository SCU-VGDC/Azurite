using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(InteractionTrigger))]
public class TeleportationSystem : MonoBehaviour
{
    public string destinationScene;
    [SerializeField] private SpriteRenderer indicator;

    private Vector3 indicStartLocal;

    private void Start()
    {
        indicStartLocal = indicator.transform.localPosition;

        GetComponent<InteractionTrigger>().playerInteractEvent.AddListener(Teleport);

        if (GameManager.Instance == null)
        {
            Debug.LogWarning("GameManager didn't exist on scene startup!");
            return;
        }

        if (!string.IsNullOrEmpty(destinationScene) && GameManager.Instance.PreviousScene == destinationScene)
            ArrivedFromDestination();
    }

    private void Update()
    {
        indicator.transform.localPosition = indicStartLocal + 0.3f * Mathf.Sin(Time.time * 2f) * indicator.transform.up;
    }

    private async void ArrivedFromDestination()
    {
        GameManager.Instance.Player.transform.position = transform.position;
        GameManager.Instance.MainCameraContainer.GetComponentInChildren<CinemachineCamera>().ForceCameraPosition(GameManager.Instance.Player.transform.position, Quaternion.identity);
        await Awaitable.WaitForSecondsAsync(0.4f);
        UIManager.Instance.SetTransitionVisible(false);
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
