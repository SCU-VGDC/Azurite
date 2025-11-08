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
    [SerializeField] private GameObject submarinePrefab;
    [NonSerialized] public static GameManager inst;
    [NonSerialized] public Player player;
    public bool debugMode;
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
        if(!debugMode)
        {
            SceneManager.LoadSceneAsync(firstGameScene, LoadSceneMode.Single);
        }
    }

    private void OnSceneUnloaded(Scene scene)
    {
        PreviousScene = scene.name;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadMode)
    {
        if (scene.name == startupScene) return;
        string SubmarineLocation;
        string CurrentLocation;

        var bounds = GameObject.FindWithTag("Camera Bounds");
        if (bounds != null && bounds.TryGetComponent<PolygonCollider2D>(out var collider))
        {
            MainCameraContainer.GetComponentInChildren<CinemachineConfiner2D>().BoundingShape2D = collider;
        }
        else
        {
            Debug.LogWarning($"Scene '{SceneManager.GetActiveScene().name}' is missing a PolygonCollider2D tagged as 'Camera Bounds'!");
        }
        if (PersistentDataManager.Instance.TryGet<string>("submarineInRoom", out SubmarineLocation))
        {
            Debug.Log($"Current world state = {SubmarineLocation}");
            if (PersistentDataManager.Instance.TryGet<string>("currentLocation", out CurrentLocation))
            {
                Debug.Log($"Current world state = {CurrentLocation}");
                if (SubmarineLocation == CurrentLocation)
                {
                    GameObject location = GameObject.Find("SubmarineDock");
                    if (location != null)
                    {
                        Debug.Log("Found Submarine!");
                    }
                    else
                    {
                        Debug.LogWarning("No object named Submarine found in scene!");
                    }
                    Instantiate(submarinePrefab, location.transform);
                }
            }
            else
            {
                Debug.Log("Submarine location not found or wrong type");
            }
        }
        else
        {
            Debug.Log("Current location not found or wrong type");
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
