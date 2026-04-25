using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

[RequireComponent(typeof(Collider2D))]
public class InteractionTrigger : MonoBehaviour, IComparable<InteractionTrigger>
{
    [Tooltip("This event is called whenever the player interacts with this object.")]
    public UnityEvent<Player> playerInteractEvent;

    [Tooltip("The key used to trigger an interaction.")]
    [SerializeField] private KeyCode triggerKey = KeyCode.E;

    [Tooltip("The text popup that appears over the interactable object.")]
    [SerializeField] private string popupText;

    [Tooltip("The amount of actions interacting with this object costs.")]
    [SerializeField] private int actionCount = 0;

    [SerializeField] private string popupPrefabAddress = "Assets/Prefabs/UI/TextPopup.prefab";
    private static AsyncOperationHandle<GameObject>? assetLoader = null;
    private static GameObject popupPrefab = null;

    public KeyCode InteractionKey => triggerKey;

    private TextPopup textPopup = null;
    public Vector3 popupOffset = Vector3.up;

    public int CompareTo(InteractionTrigger other)
    {
        Transform plrTransform = GameManager.inst.player.transform;
        float myDist = Vector2.Distance(transform.position, plrTransform.position);
        float otherDist = Vector2.Distance(other.transform.position, plrTransform.position);
        return MathF.Sign(myDist - otherDist);
    }

    private void Start()
    {
        if (assetLoader != null) return;
        assetLoader = Addressables.LoadAssetAsync<GameObject>(popupPrefabAddress);
        assetLoader.Value.WaitForCompletion();

        if (assetLoader.Value.Status == AsyncOperationStatus.Succeeded)
        {
            popupPrefab = assetLoader.Value.Result;
        }
    }

    private void Update()
    {
        if (this.textPopup != null)
        {
            this.textPopup.transform.position = this.transform.position + popupOffset;
        }
    }

    public void Trigger(Player interactingPlayer)
    {
        if (!GameManager.inst.paused)
        {
            playerInteractEvent.Invoke(interactingPlayer);
            ActionManager.Instance.IncrementAction(actionCount);
        }
    }

    public void ToggleTextPopup(bool value)
    {
        if (value)
        {
            if (textPopup != null || popupPrefab == null)
                return;
            textPopup = Instantiate(popupPrefab).GetComponent<TextPopup>();
            textPopup.showOnStart = true;
        }
        else if (textPopup != null)
        {
            textPopup.Hide(true);
            textPopup = null;
        }
    }
}
