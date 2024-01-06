using DG.Tweening;
using UnityEngine;

public class Circle : MonoBehaviour
{
    private readonly float Duration = 0.05f;

    public void ChangeScale(float endValue)
    {
        transform.DOScale(endValue, Duration)
            .SetEase(Ease.InOutSine);
    }
}