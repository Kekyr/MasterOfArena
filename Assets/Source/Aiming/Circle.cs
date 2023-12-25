using DG.Tweening;
using UnityEngine;

public class Circle : MonoBehaviour
{
    public void ChangeScale(float endValue)
    {
        transform.DOScale(endValue, 0.05f)
            .SetEase(Ease.InOutSine);
    }
}