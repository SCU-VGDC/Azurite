using DG.Tweening;
using UnityEngine;

public class DestroyObjectStep : DialogStep
{
    [SerializeField] private GameObject toDestroy;

    public override void OnEnterStep()
    {
        base.OnEnterStep();

        if (toDestroy != null)
        {
            DOTween.Sequence()
                .Append(UIManager.Instance.SetTransitionVisible(true))
                .AppendCallback(() => Destroy(toDestroy))
                .Append(UIManager.Instance.SetTransitionVisible(false));
        }
    }
}
