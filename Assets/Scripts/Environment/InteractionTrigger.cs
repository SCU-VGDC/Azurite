using System;
using Unity.Scripting.LifecycleManagement;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
[AutoStaticsCleanup]
public partial class InteractionTrigger : MonoBehaviour, IComparable<InteractionTrigger>
{
    private const string popupPrefabPath = "Assets/Prefabs/UI/TextPopup.prefab";
    private static GameObject popupPrefab;

    public UnityEvent<Player> playerInteractEvent;

    public virtual bool CanInteract { get; protected set; } = true;
    public virtual string PopupText => defaultPopupText;

    public KeyCode triggerKey = KeyCode.E;
    public string defaultPopupText = "Interact";
    public int actionCount = 0;
    public KeyCode InteractionKey => triggerKey;
    public Vector3 popupOffset = Vector3.up;
    
    private TextPopup textPopupComponent = null;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
    private static void LoadPopupPrefab()
    {
        popupPrefab = Addressables.LoadAssetAsync<GameObject>(popupPrefabPath).WaitForCompletion();
    }

    protected virtual void OnDisable()
    {
        ToggleTextPopup(false);
    }

    protected virtual void Update()
    {
        if (textPopupComponent != null && textPopupComponent.Text != PopupText)
            textPopupComponent.Text = PopupText;
    }

    protected virtual void OnDestroy()
    {
        if (textPopupComponent != null)
        {
            Destroy(textPopupComponent.gameObject);
            textPopupComponent = null;
        }    
    }

    public int CompareTo(InteractionTrigger other)
    {
        Transform plrTransform = GameManager.Instance.Player.transform;
        float myDist = Vector2.Distance(transform.position, plrTransform.position);
        float otherDist = Vector2.Distance(other.transform.position, plrTransform.position);
        return MathF.Sign(myDist - otherDist);
    }

    public virtual void Trigger(Player interactingPlayer)
    {
        if (!GameManager.Instance.Paused && isActiveAndEnabled && CanInteract)
        {
            playerInteractEvent.Invoke(interactingPlayer);
            ActionManager.Instance.IncrementAction(actionCount);
        }
    }

    public void ToggleTextPopup(bool value)
    {
        if (string.IsNullOrEmpty(PopupText) || (value && textPopupComponent != null) || (!value && textPopupComponent == null))
            return;

        if (value && isActiveAndEnabled)
        {
            if (textPopupComponent != null || popupPrefab == null)
                return;
            textPopupComponent = Instantiate(popupPrefab).GetComponent<TextPopup>();
            textPopupComponent.transform.SetParent(transform, true);
            textPopupComponent.popupOffset = popupOffset;
            textPopupComponent.Text = PopupText;
            textPopupComponent.Show();
        }
        else if (textPopupComponent != null)
        {
            textPopupComponent.Hide(true);
            textPopupComponent = null;
        }
    }
}
