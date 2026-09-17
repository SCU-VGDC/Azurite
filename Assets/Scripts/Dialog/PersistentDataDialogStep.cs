using UnityEngine;

public class PersistentDataDialogStep : DialogStep
{
    public bool conditionalEntry = true;
    public string boolDataKey;
    public bool conditionalPolarity = true;
    public bool writeOnEnter = false;

    public override bool TransitionAllowed => !conditionalEntry || (!conditionalPolarity ^ (PersistentDataManager.Instance.TryGet(boolDataKey, out bool value) && value));

    public override void OnEnterStep()
    {
        base.OnEnterStep();
        if (writeOnEnter)
            PersistentDataManager.Instance.Set(boolDataKey, true);
    }
}
