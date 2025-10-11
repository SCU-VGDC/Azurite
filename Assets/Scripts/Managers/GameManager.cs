using Unity.Cinemachine;
using System;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private string firstGameScene;
    [SerializeField] private string startupScene;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject mainCameraPrefab;
    [NonSerialized] public static GameManager inst;
    [NonSerialized] public Player player;
    public GameObject MainCameraContainer { get; private set; } = null;
    public string PreviousScene { get; private set; } = null;

    // game states
    [NonSerialized] public bool paused;

    // Puzzles:
    public Action currentEndGameAction;

    void Start()
    {
        paused = false;
    }

    void Awake()
    {
        // Basic singleton pattern. Make sure there is only ever 1 GameManager in the scene and updates inst accordingly.

        // If no GameManager assigned...
        if (inst == null)
        {
            // This is our GameManager
            inst = this;
        }
        // If there is a GameManager assigned, and it's not this
        else if (inst != this)
        {
            // Then this shouldn't exist, destroy it.
            Destroy(this);
            return;
        }

        DontDestroyOnLoad(gameObject);

        var playerObj = Instantiate(playerPrefab);
        DontDestroyOnLoad(playerObj);
        player = playerObj.GetComponent<Player>();

        MainCameraContainer = Instantiate(mainCameraPrefab);
        DontDestroyOnLoad(MainCameraContainer);

        var cineCam = MainCameraContainer.GetComponentInChildren<CinemachineCamera>();
        cineCam.Target = new CameraTarget()
        {
            TrackingTarget = player.transform,
            LookAtTarget = player.transform,
        };

        SceneManager.sceneUnloaded += OnSceneUnloaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadSceneAsync(firstGameScene, LoadSceneMode.Single);
    }

    private void OnSceneUnloaded(Scene scene)
    {
        PreviousScene = scene.name;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadMode)
    {
        if (scene.name == startupScene) return;

        var bounds = GameObject.FindWithTag("Camera Bounds");
        if (bounds != null && bounds.TryGetComponent<PolygonCollider2D>(out var collider))
        {
            MainCameraContainer.GetComponentInChildren<CinemachineConfiner2D>().BoundingShape2D = collider;
        }
        else
        {
            Debug.LogWarning($"Scene '{SceneManager.GetActiveScene().name}' is missing a PolygonCollider2D tagged as 'Camera Bounds'!");
        }
    }


    public IEnumerator Sleep(float seconds, Action action)
    {
        yield return new WaitForSeconds(seconds);

        action?.Invoke();
    }


    public void EndCurrentPuzzle()
    {
        currentEndGameAction?.Invoke();
    }
}
