using System;
using DG.Tweening;
using UnityEngine;

public abstract class Aiming : MonoBehaviour
{
    private readonly float NewScale = 0.02f;
    private readonly float Duration = 0.1f;

    [SerializeField] private Aim _aim;
    [SerializeField] private Circle _circle;
    [SerializeField] private Catcher _catcher;
    [SerializeField] private Projectile[] _projectiles;

    private Sequence _sequence;
    private Health _health;
    private Health _enemyHealth;

    public event Action<Vector3> Aimed;

    protected Catcher Catcher => _catcher;

    public Aim Aim => _aim;

    public Circle Circle => _circle;

    protected virtual void OnEnable()
    {
        int maxLength = 2;

        if (_aim == null)
            throw new ArgumentNullException(nameof(_aim));

        if (_circle == null)
            throw new ArgumentNullException(nameof(_circle));

        if (_catcher == null)
            throw new ArgumentNullException(nameof(_catcher));

        if (_projectiles.Length == 0 || _projectiles.Length > maxLength)
            throw new ArgumentOutOfRangeException(nameof(_projectiles));

        foreach (Projectile projectile in _projectiles)
            projectile.Catched += OnCatch;

        _circle.transform.localScale = Vector3.zero;

        _sequence.Append(_circle.transform.DOScale(NewScale, Duration)
            .SetEase(Ease.InBounce));

        _health.Died += OnDead;
        _enemyHealth.Died += OnDead;
    }

    protected virtual void OnDisable()
    {
        foreach (Projectile projectile in _projectiles)
            projectile.Catched -= OnCatch;

        _health.Died -= OnDead;
        _enemyHealth.Died -= OnDead;
    }

    public void Init(Sequence sequence, Health health, Health enemyHealth)
    {
        if (sequence == null)
            throw new ArgumentNullException(nameof(sequence));

        if (health == null)
            throw new ArgumentNullException(nameof(health));

        if (enemyHealth == null)
            throw new ArgumentNullException(nameof(enemyHealth));

        _health = health;
        _enemyHealth = enemyHealth;
        _sequence = sequence;
    }

    protected void RotateTo(Vector3 direction)
    {
        direction.y = 0;
        Quaternion newRotation = Quaternion.LookRotation(direction);
        transform.rotation = newRotation;
        _aim.RotateTo(newRotation);
    }

    protected void InvokeAimed(Vector3 value)
    {
        Aimed?.Invoke(value);
    }

    protected abstract void OnCatch();

    protected abstract void OnDead();
}