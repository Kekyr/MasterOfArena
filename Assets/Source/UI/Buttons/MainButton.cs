using DG.Tweening;
using UnityEngine;

public class MainButton : MonoBehaviour
{
    private readonly float NewScale = 2f;
    private readonly float Duration = 1f;

    protected virtual void OnEnable()
    {
        transform.localScale = Vector3.zero;

        transform.DOScale(NewScale, Duration)
            .SetEase(Ease.InOutSine);
    }
}