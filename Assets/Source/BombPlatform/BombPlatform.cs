using System;
using DG.Tweening;
using UnityEngine;

public class BombPlatform : MonoBehaviour
{
    private readonly float NewScale = 1f;
    private readonly float ScaleDuration = 0.05f;
    private readonly float PlatformScaleYDuration = 0.1f;
    private readonly float BombMoveYDuration = 0.1f;
    private readonly float LightningMoveDuraion = 0.7f;

    [SerializeField] private Platform _platform;
    [SerializeField] private Bomb _bomb;
    [SerializeField] private ParticleSystem _lightningTrail;

    private Sequence _sequence;
    private Health _health;
    private Character _enemy;

    private float _startPlatformScaleY;
    private float _startBombY;

    public event Action<uint> Attacked;

    private void OnEnable()
    {
        if (_platform == null)
            throw new ArgumentNullException(nameof(_platform));

        if (_bomb == null)
            throw new ArgumentNullException(nameof(_bomb));

        if (_lightningTrail == null)
            throw new ArgumentNullException(nameof(_lightningTrail));

        _startPlatformScaleY = _platform.transform.localScale.y;
        _startBombY = _bomb.transform.position.y;

        transform.localScale = Vector3.zero;
        _sequence.Append(transform.DOScale(NewScale, ScaleDuration)
            .SetEase(Ease.InOutSine));

        _health.HealthChanged += OnHealthChanged;
        _enemy.Attacking += OnAttacking;
    }

    private void OnDisable()
    {
        _health.HealthChanged -= OnHealthChanged;
        _enemy.Attacking -= OnAttacking;
    }

    public void Init(Sequence sequence, Health health, Character enemy)
    {
        if (sequence == null)
            throw new ArgumentNullException(nameof(sequence));

        if (health == null)
            throw new ArgumentNullException(nameof(health));

        if (enemy == null)
            throw new ArgumentNullException(nameof(enemy));

        _sequence = sequence;
        _health = health;
        _enemy = enemy;
        enabled = true;
    }

    private void OnHealthChanged(float value)
    {
        float maxPercent = 100;
        float percent = value / maxPercent;

        _platform.transform.DOScaleY(_startPlatformScaleY * percent, PlatformScaleYDuration)
            .SetEase(Ease.InOutSine)
            .OnComplete(() =>
            {
                _bomb.transform.DOMoveY(_startBombY * percent, BombMoveYDuration)
                    .SetEase(Ease.OutBounce);
            });
    }

    private void OnAttacking(Cube cube)
    {
        _lightningTrail.transform.position = cube.transform.position;
        _lightningTrail.Play();
        _lightningTrail.transform.DOMove(transform.position, LightningMoveDuraion)
            .SetEase(Ease.InOutSine)
            .OnComplete(() => { Attacked?.Invoke(cube.Damage); });
    }
}