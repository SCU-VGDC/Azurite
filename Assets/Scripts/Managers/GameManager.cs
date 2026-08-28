using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(EventSystem))]
public class GameManager : MonoBehaviour
{
    private const string gameManagerPrefabKey = "Assets/Prefabs/Managers/GameManager.prefab";
    private const string playerPrefabKey = "Assets/Prefabs/Player.prefab";
    private const string cameraPrefabKey = "Assets/Prefabs/CameraMain.prefab";

    public static GameManager Instance { get; private set; }
    public Player Player { get; private set; }

    public GameObject MainCameraContainer { get; private set; }
    public Camera MainCamera => MainCameraContainer.GetComponentInChildren<Camera>();
    public string PreviousScene { get; private set; }

    // game states
    private bool _paused = false;
    public bool Paused
    {
        get => _paused;
        set
        {
            Time.timeScale = value ? 0f : 1f;
            _paused = value;
        }
    }

    public event Action OnPuzzleEnd;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void GameStart()
    {
        Addressables.LoadAssetAsync<GameObject>(gameManagerPrefabKey).Completed += handle =>
        {
            if (handle.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
                DontDestroyOnLoad(Instantiate(handle.Result));
            else
                Debug.LogError($"Failed to load GameManager prefab: {handle.OperationException}");
        };
    }

    private void Start()
    {
        Paused = false;
    }

    private async void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        var cameraPrefab = await Addressables.LoadAssetAsync<GameObject>(cameraPrefabKey).Task;
        MainCameraContainer = Instantiate(cameraPrefab);
        DontDestroyOnLoad(MainCameraContainer);

        var playerPrefab = await Addressables.LoadAssetAsync<GameObject>(playerPrefabKey).Task;
        GameObject playerObj = Instantiate(playerPrefab);
        DontDestroyOnLoad(playerObj);
        Player = playerObj.GetComponent<Player>();

        CinemachineCamera cineCam = MainCameraContainer.GetComponentInChildren<CinemachineCamera>();
        cineCam.Target = new CameraTarget()
        {
            TrackingTarget = Player.transform,
            LookAtTarget = Player.transform,
        };

        SceneManager.sceneUnloaded += OnSceneUnloaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneUnloaded(Scene scene)
    {
        PreviousScene = scene.name;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadMode)
    {
        var bounds = GameObject.FindWithTag("Camera Bounds");
        if (bounds != null && bounds.TryGetComponent(out PolygonCollider2D collider))
        {
            MainCameraContainer.GetComponentInChildren<CinemachineConfiner2D>().BoundingShape2D = collider;
        }
        else
        {
            Debug.LogWarning($"Scene '{SceneManager.GetActiveScene().name}' is missing a PolygonCollider2D tagged as 'Camera Bounds'!");
        }
    }


    [Obsolete("Extra boilerplate code, just use an Awaitable")]
    public IEnumerator Sleep(float seconds, Action action)
    {
        yield return new WaitForSeconds(seconds);
        action?.Invoke();
    }

    public void EndCurrentPuzzle()
    {
        OnPuzzleEnd?.Invoke();
    }
}
