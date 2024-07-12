using System;
using DG.Tweening;
using UnityEngine;

public class BombPlatform : MonoBehaviour
{
    private readonly float BombScaleXOffset = 0.2f;
    private readonly float BombScaleDuration = 1f;
    private readonly float NewScale = 1f;
    private readonly float ScaleDuration = 0.02f;
    private readonly float PlatformScaleYDuration = 0.1f;
    private readonly float MoveYDuration = 0.1f;
    private readonly float LightningMoveDuration = 0.7f;
    private readonly float ColorDuration = 0.25f;
    private readonly float MinFusePercent = 0.1f;
    private readonly float MaxFusePercent = 0.3f;

    [SerializeField] private Platform _platform;
    [SerializeField] private Bomb _bomb;
    [SerializeField] private HealthView _view;

    [SerializeField] private ParticleSystem _lightningTrail;
    [SerializeField] private ParticleSystem _fuse;

    private Sequence _sequence;
    private Health _health;
    private Health _enemyHealth;
    private Character _enemy;
    private Helper _helper;

    private float _startPlatformScaleY;
    private float _startBombY;
    private float _startBombScaleX;

    public event Action<uint> Attacked;

    private void OnEnable()
    {
        if (_platform == null)
        {
            throw new ArgumentNullException(nameof(_platform));
        }

        if (_bomb == null)
        {
            throw new ArgumentNullException(nameof(_bomb));
        }

        if (_view == null)
        {
            throw new ArgumentNullException(nameof(_view));
        }

        if (_lightningTrail == null)
        {
            throw new ArgumentNullException(nameof(_lightningTrail));
        }

        if (_fuse == null)
        {
            throw new ArgumentNullException(nameof(_fuse));
        }

        _enemyHealth = _enemy.GetComponent<Health>();

        _startPlatformScaleY = _platform.transform.localScale.y;
        _startBombY = _bomb.MeshRenderer.transform.position.y;
        _startBombScaleX = _bomb.MeshRenderer.transform.localScale.x;

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

    public void Init(Sequence sequence, Health health, Character enemy, Helper helper)
    {
        if (sequence == null)
        {
            throw new ArgumentNullException(nameof(sequence));
        }

        if (health == null)
        {
            throw new ArgumentNullException(nameof(health));
        }

        if (enemy == null)
        {
            throw new ArgumentNullException(nameof(enemy));
        }

        if (helper == null)
        {
            throw new ArgumentNullException(nameof(helper));
        }

        _helper = helper;
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
                _bomb.transform.DOMoveY(_startBombY * percent, MoveYDuration)
                    .SetEase(Ease.OutBounce)
                    .OnComplete(() =>
                    {
                        float bombOffsetMultiplier = (1 - percent);

                        _helper.ChangeMeshColor(_bomb.MeshRenderer, Color.red, ColorDuration);

                        _bomb.MeshRenderer.transform.DOScaleX(
                            _startBombScaleX + (BombScaleXOffset * bombOffsetMultiplier),
                            BombScaleDuration);

                        if (percent >= MinFusePercent && percent <= MaxFusePercent)
                        {
                            _fuse.Play();
                        }
                    });
            });
    }

    private void OnAttacking(Cube cube)
    {
        _lightningTrail.transform.position = cube.transform.position;
        _lightningTrail.Play();
        _lightningTrail.transform.DOMove(transform.position, LightningMoveDuration)
            .SetEase(Ease.InOutSine)
            .OnComplete(() =>
            {
                if (_enemyHealth.IsDead == false)
                {
                    Attacked?.Invoke(cube.Damage);
                }
            });
    }
}