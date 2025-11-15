using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Collider2D))]
public class InteractionTrigger : MonoBehaviour, IComparable<InteractionTrigger>
{
    public UnityEvent onInteract;
    public KeyCode triggerKey = KeyCode.E;
    public TextMeshProUGUI textPopUp;
    public int ActionCount = 0;
    
    private static readonly string INTERACT_CANVAS_PREFAB_PATH = "Assets/Prefabs/UI/InteractCanvas.prefab";
    private static readonly string INTERACT_CANVAS_RESOURCES_PATH = "Prefabs/UI/InteractCanvas";

    public int CompareTo(InteractionTrigger other)
    {
        Transform plrTransform = GameManager.inst.player.transform;
        float myDist = Vector2.Distance(transform.position, plrTransform.position);
        float otherDist = Vector2.Distance(other.transform.position, plrTransform.position);
        return MathF.Sign(myDist - otherDist);
    }

    void Reset()
    {
        // Called when component is first added to a GameObject in the editor
        SetupInteractCanvas();
    }

    void Awake()
    {
        // Runtime fallback: if textPopUp is not assigned, instantiate the InteractCanvas prefab
        if (textPopUp == null)
        {
            SetupInteractCanvas();
        }
    }

    private void SetupInteractCanvas()
    {
        // Don't create if already exists
        if (textPopUp != null)
        {
            return;
        }

        // Check if canvas already exists as a child
        Transform existingCanvas = transform.Find("InteractCanvas");
        if (existingCanvas != null)
        {
            Canvas existingCanvasComponent = existingCanvas.GetComponent<Canvas>();
            if (existingCanvasComponent != null)
            {
                existingCanvasComponent.sortingOrder = 32767; // Ensure high sorting order
            }
            textPopUp = existingCanvas.GetComponentInChildren<TextMeshProUGUI>(true);
            if (textPopUp != null)
            {
                return;
            }
        }

        GameObject prefabToInstantiate = null;

#if UNITY_EDITOR
        // In editor (both playing and not playing), try asset path first
        if (!Application.isPlaying)
        {
            prefabToInstantiate = AssetDatabase.LoadAssetAtPath<GameObject>(INTERACT_CANVAS_PREFAB_PATH);
        }
        else
        {
            // Editor play mode: try Resources first, then asset path as fallback
            prefabToInstantiate = Resources.Load<GameObject>(INTERACT_CANVAS_RESOURCES_PATH);
            if (prefabToInstantiate == null)
            {
                prefabToInstantiate = AssetDatabase.LoadAssetAtPath<GameObject>(INTERACT_CANVAS_PREFAB_PATH);
            }
        }
#else
        // In build, load from Resources
        prefabToInstantiate = Resources.Load<GameObject>(INTERACT_CANVAS_RESOURCES_PATH);
#endif

        if (prefabToInstantiate != null)
        {
            GameObject canvasInstance;
            
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                // In edit mode (not playing), use PrefabUtility to instantiate
                canvasInstance = (GameObject)PrefabUtility.InstantiatePrefab(prefabToInstantiate, transform);
                Undo.RegisterCreatedObjectUndo(canvasInstance, "Create InteractCanvas");
            }
            else
#endif
            {
                // At runtime (editor play mode or build), use regular Instantiate
                canvasInstance = Instantiate(prefabToInstantiate, transform);
            }
            
            canvasInstance.name = "InteractCanvas";
            
            // Set high sorting order on the Canvas to render on top
            Canvas canvas = canvasInstance.GetComponent<Canvas>();
            if (canvas != null)
            {
                canvas.sortingOrder = 32767; // Maximum sorting order to ensure it renders on top
            }
            
            // Find the TextMeshProUGUI component in the instantiated prefab
            textPopUp = canvasInstance.GetComponentInChildren<TextMeshProUGUI>(true);
            
            if (textPopUp == null)
            {
                Debug.LogError($"InteractionTrigger: Could not find TextMeshProUGUI component in instantiated InteractCanvas prefab on {gameObject.name}");
            }
#if UNITY_EDITOR
            else if (!Application.isPlaying)
            {
                // Mark the object as dirty so changes are saved
                EditorUtility.SetDirty(this);
            }
#endif
        }
        else
        {
            Debug.LogError($"InteractionTrigger: Could not load InteractCanvas prefab at {INTERACT_CANVAS_PREFAB_PATH} on {gameObject.name}");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (textPopUp != null)
        {
            Canvas canvas = textPopUp.GetComponentInParent<Canvas>(true);
            canvas.transform.localPosition = new Vector3(0, 1, 0);
            canvas.worldCamera = Camera.main;
            textPopUp.text = triggerKey.ToString();
            ToggleTextPopup(false);
        }
    }

    public void Trigger()
    {
        if (!GameManager.inst.paused)
        {
            this.onInteract?.Invoke();
            ActionManager.Instance.IncrementAction(ActionCount);
        }        
    }

    public void ToggleTextPopup(bool value)
    {
        if (textPopUp != null)
        {
            textPopUp.GetComponentInParent<Canvas>(true).gameObject.SetActive(value);
        }
    }
}
