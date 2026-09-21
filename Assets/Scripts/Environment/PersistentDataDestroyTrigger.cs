using UnityEngine;
using System;

public class PersistentDataDestroyTrigger : MonoBehaviour
{
    [SerializeField] private string key;
    [SerializeField] private bool polarity = true;
    [SerializeField] private bool listenForChanges = true;

    private void Start()
    {
        if (listenForChanges)
            PersistentDataManager.Instance.ListenForKeyChanged<bool>(key, OnKeyChange);

        if (PersistentDataManager.Instance.TryGet(key, out bool val) && !(val ^ polarity))
            Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (listenForChanges)
            PersistentDataManager.Instance.StopListeningForKeyChanged<bool>(key, OnKeyChange);
    }

    private void OnKeyChange(bool newVal)
    {
        if (!(newVal ^ polarity))
            Destroy(gameObject);
    }
}
