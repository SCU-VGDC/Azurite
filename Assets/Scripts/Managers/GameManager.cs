using DG.Tweening;
using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using Unity.Scripting.LifecycleManagement;

[RequireComponent(typeof(EventSystem))]
[AutoStaticsCleanup]
public partial class GameManager : MonoBehaviour
{
    private const string GameManagerPrefabPath = "Assets/Prefabs/Critical/GameManager.prefab";
    private const string CameraPrefabPath = "Assets/Prefabs/Critical/CameraMain.prefab";
    private const string PlayerPrefabPath = "Assets/Prefabs/Critical/Player.prefab";

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

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
    private static void CreateManager()
    {
        var handle = Addressables.LoadAssetAsync<GameObject>(GameManagerPrefabPath);
        handle.WaitForCompletion();
        DontDestroyOnLoad(Instantiate(handle.Result));
    }

    private void Start()
    {
        Paused = false;
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DOTween.KillAll();  // needed because of fast play mode preserving tweens

        var cameraPrefab = Addressables.LoadAssetAsync<GameObject>(CameraPrefabPath).WaitForCompletion();
        MainCameraContainer = Instantiate(cameraPrefab);
        DontDestroyOnLoad(MainCameraContainer);

        var playerPrefab = Addressables.LoadAssetAsync<GameObject>(PlayerPrefabPath).WaitForCompletion();
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

    private void OnDestroy()
    {
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneUnloaded(Scene scene)
    {
        PreviousScene = scene.name;
    }

    private async void OnSceneLoaded(Scene scene, LoadSceneMode loadMode)
    {
        var bounds = GameObject.FindWithTag("Camera Bounds");
        if (bounds != null && bounds.TryGetComponent(out Collider2D collider))
            MainCameraContainer.GetComponentInChildren<CinemachineConfiner2D>().BoundingShape2D = collider;
        else
            Debug.LogWarning($"Scene '{SceneManager.GetActiveScene().name}' is missing a Collider2D tagged as 'Camera Bounds'!");

        await Awaitable.WaitForSecondsAsync(0.3f);
        UIManager.Instance.SetTransitionVisible(false);
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
