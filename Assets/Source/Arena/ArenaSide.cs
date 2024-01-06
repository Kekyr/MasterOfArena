using System;
using DG.Tweening;
using UnityEngine;

public class ArenaSide : MonoBehaviour
{
    private readonly float NewScale = 0f;
    private readonly float Duration = 0.05f;

    private Health _health;

    private void OnEnable()
    {
        _health.Died += OnDead;
    }

    private void OnDisable()
    {
        _health.Died -= OnDead;
    }

    public void Init(Health health)
    {
        if (health == null)
            throw new ArgumentNullException(nameof(health));

        _health = health;
        enabled = true;
    }

    private void OnDead()
    {
        transform.DOScale(NewScale, Duration)
            .SetEase(Ease.InOutSine);
    }
}