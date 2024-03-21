using System;
using DG.Tweening;
using UnityEngine;

public class Aim : MonoBehaviour
{
    private readonly float Duration = 0.05f;
    private readonly float RotationX = 90;
    private readonly float MinScale = 0f;
    private readonly float MaxScale = 0.05f;

    [SerializeField] private Character _character;
    [SerializeField] private Targeting _targeting;

    private void OnEnable()
    {
        if (_character == null)
            throw new ArgumentNullException(nameof(_character));

        if (_targeting == null)
            throw new ArgumentNullException(nameof(_targeting));

        transform.localScale = Vector3.zero;

        _character.Throwed += DecreaseScale;
        _targeting.Aiming += IncreaseScale;
    }

    private void OnDisable()
    {
        _character.Throwed -= DecreaseScale;
        _targeting.Aiming -= IncreaseScale;
    }

    public void RotateTo(Quaternion newRotation)
    {
        Vector3 eulerAngles = newRotation.eulerAngles;
        eulerAngles.x = RotationX;
        transform.eulerAngles = eulerAngles;
    }

    private void IncreaseScale()
    {
        ChangeScale(MaxScale);
    }

    private void DecreaseScale(Transform transform)
    {
        ChangeScale(MinScale);
    }

    private void ChangeScale(float endValue)
    {
        transform.DOScale(endValue, Duration)
            .SetEase(Ease.InOutSine);
    }
}