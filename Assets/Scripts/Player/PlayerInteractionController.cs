using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class PlayerInteractionController : MonoBehaviour
{
    [SerializeField] private float _interactionDist = 3;
    public float InteractionDistance
    {
        get => _interactionDist;
        set
        {
            _interactionDist = value;
            GetComponent<CircleCollider2D>().radius = value;
        }
    }

    private readonly Dictionary<KeyCode, List<InteractionTrigger>> triggers = new();

    private void Start()
    {
        InteractionDistance = _interactionDist;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out InteractionTrigger interaction))
        {
            if (!triggers.ContainsKey(interaction.InteractionKey))
                triggers.Add(interaction.InteractionKey, new List<InteractionTrigger>());

            triggers[interaction.InteractionKey].Add(interaction);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out InteractionTrigger interaction) && triggers.TryGetValue(interaction.InteractionKey, out var ilist))
        {
            ilist.Remove(interaction);
            interaction.ToggleTextPopup(false);
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

            var player = GetComponentInParent<Player>();
            bool allowInteraction = !player.Frozen;

            InteractionTrigger closest = triggerPair.Value.Min();

            foreach (var interaction in triggerPair.Value)
                interaction.ToggleTextPopup(allowInteraction && interaction == closest);

            if (allowInteraction && (Input.GetKeyDown(triggerPair.Key) || (Input.GetMouseButtonDown(0) && hit.collider == closest.GetComponent<Collider2D>())))
                closest.Trigger(GetComponentInParent<Player>());
        }
    }
}
