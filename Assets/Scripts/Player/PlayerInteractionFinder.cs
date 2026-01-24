using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(CircleCollider2D))]
public class PlayerInteractionFinder : MonoBehaviour
{
    [SerializeField] private float _interactionDist = 3;
    private readonly Dictionary<KeyCode, List<InteractionTrigger>> triggers = new();

	private Player player;

    public float InteractionDistance
    {
        get => _interactionDist;
        set
        {
            _interactionDist = value;
            GetComponent<CircleCollider2D>().radius = value;
        }
    }

	public void Awake()
	{
		this.player = this.GetComponentInParent<Player>();

		if(this.player == null)
		{
			Debug.LogError("Failed to find the player.");
		}
	}

	private void Start()
    {
        InteractionDistance = _interactionDist;
    }

    private void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        if(collision.TryGetComponent<InteractionTrigger>(out InteractionTrigger interaction))
        {
            if(!triggers.ContainsKey(interaction.GetInteractionKey()))
            {
                triggers.Add(interaction.GetInteractionKey(), new List<InteractionTrigger>());
            }
            triggers[interaction.GetInteractionKey()].Add(interaction);
        }
    }

    private void OnTriggerExit2D(UnityEngine.Collider2D collision)
    {
        if (collision.TryGetComponent<InteractionTrigger>(out InteractionTrigger interaction))
        {
            if (triggers.TryGetValue(interaction.GetInteractionKey(), out var ilist))
            {
                ilist.Remove(interaction);
                interaction.ToggleTextPopup(false);
            }
        }    
    }

    private void Update()
    {
        Vector2 mpos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mpos, Camera.main.transform.forward, 100);

        foreach (var triggerPair in triggers)
        {
            if (triggerPair.Value.Count == 0)
                continue;

            InteractionTrigger closest = triggerPair.Value.Min();
            foreach(var interaction in triggerPair.Value)
            {
                interaction.ToggleTextPopup(interaction == closest);
            }
            if (Input.GetKeyDown(triggerPair.Key) || (Input.GetMouseButtonDown(0) && hit.collider == closest.GetComponent<Collider2D>()))
            {
                closest.Trigger(this.player);
            }
        }
    }
}
