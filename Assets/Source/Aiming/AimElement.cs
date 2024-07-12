using System;
using DG.Tweening;
using UnityEngine;

public class AimElement : MonoBehaviour
{
    private readonly float Duration = 0.05f;
    private readonly float MinScale = 0f;

    [SerializeField] private float _maxScale;

    private Health _health;
    private Character _character;
    private Targeting _targeting;

    protected virtual void OnEnable()
    {
        transform.localScale = Vector3.zero;
        _character.Throwed += DecreaseScale;
        _targeting.Aiming += IncreaseScale;
    }

    protected virtual void OnDisable()
    {
        _character.Throwed -= DecreaseScale;
        _targeting.Aiming -= IncreaseScale;
        _health.Died -= DecreaseScale;
    }

    public void Init(Character character, Targeting targeting, Health health)
    {
        if (character == null)
        {
            throw new ArgumentNullException(nameof(character));
        }

        if (targeting == null)
        {
            throw new ArgumentNullException(nameof(targeting));
        }

        if (health == null)
        {
            throw new ArgumentNullException(nameof(health));
        }

        _character = character;
        _targeting = targeting;
        _health = health;

        _health.Died += DecreaseScale;

        enabled = true;
    }

    protected void IncreaseScale()
    {
        ChangeScale(_maxScale);
    }

    private void DecreaseScale(Transform transform)
    {
        ChangeScale(MinScale);
    }

    private void DecreaseScale()
    {
        ChangeScale(MinScale);
    }

    private void ChangeScale(float endValue)
    {
        transform.DOScale(endValue, Duration)
            .SetEase(Ease.InOutSine);
    }
}