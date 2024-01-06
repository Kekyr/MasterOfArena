using System;
using DG.Tweening;
using UnityEngine;

public class BombPlatform : MonoBehaviour
{
    private readonly float NewScale = 1f;
    private readonly float ScaleDuration = 0.05f;
    private readonly float PlatformScaleYDuration = 0.1f;
    private readonly float BombMoveYDuration = 0.1f;

    [SerializeField] private Transform _platform;
    [SerializeField] private Transform _bomb;

    private Sequence _sequence;
    private Health _health;

    private float _startPlatformScaleY;
    private float _startBombY;

    private void OnEnable()
    {
        _startPlatformScaleY = _platform.localScale.y;
        _startBombY = _bomb.transform.position.y;

        transform.localScale = Vector3.zero;
        _sequence.Append(transform.DOScale(NewScale, ScaleDuration)
            .SetEase(Ease.InOutSine));

        _health.HealthChanged += OnHealthChanged;
    }

    private void OnDisable()
    {
        _health.HealthChanged -= OnHealthChanged;
    }

    public void Init(Sequence sequence, Health health)
    {
        if (sequence == null)
            throw new ArgumentNullException(nameof(sequence));

        if (health == null)
            throw new ArgumentNullException(nameof(health));

        _sequence = sequence;
        _health = health;
        enabled = true;
    }

    private void OnHealthChanged(float value)
    {
        float maxPercent = 100;
        float percent = value / maxPercent;

        _platform.DOScaleY(_startPlatformScaleY * percent, PlatformScaleYDuration)
            .SetEase(Ease.InOutSine)
            .OnComplete(() =>
            {
                _bomb.DOMoveY(_startBombY * percent, BombMoveYDuration)
                    .SetEase(Ease.OutBounce);
            });
    }
}