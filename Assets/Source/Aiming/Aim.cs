using DG.Tweening;
using UnityEngine;

public class Aim : MonoBehaviour
{
    private readonly float RotationX = 90;

    private void OnEnable()
    {
        transform.localScale = Vector3.zero;
    }

    public void RotateTo(Quaternion newRotation)
    {
        Vector3 eulerAngles = newRotation.eulerAngles;
        eulerAngles.x = RotationX;
        transform.eulerAngles = eulerAngles;
    }

    public void ChangeScale(float endValue)
    {
        transform.DOScale(endValue, 0.05f)
            .SetEase(Ease.InOutSine);
    }
}