using System;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Character))]
public abstract class Targeting : MonoBehaviour
{
    private readonly float NewScale = 0.02f;
    private readonly float Duration = 0.1f;

    [SerializeField] private Arrow _arrow;
    [SerializeField] private Circle _circle;

    private Character _character;
    private Sequence _sequence;
    private Health _health;
    private Health _enemyHealth;

    public event Action<Vector3> Aimed;

    public event Action Aiming;

    protected Character Character => _character;

    public Arrow Arrow => _arrow;

    public Circle Circle => _circle;

    protected virtual void OnEnable()
    {
        if (_arrow == null)
        {
            throw new ArgumentNullException(nameof(_arrow));
        }

        if (_circle == null)
        {
            throw new ArgumentNullException(nameof(_circle));
        }

        _character = GetComponent<Character>();
        _circle.transform.localScale = Vector3.zero;

        _sequence.Append(_circle.transform.DOScale(NewScale, Duration)
            .SetEase(Ease.InBounce));

        _health.Died += OnDead;
        _enemyHealth.Died += OnDead;
    }

    protected virtual void OnDisable()
    {
        _health.Died -= OnDead;
        _enemyHealth.Died -= OnDead;
    }

    public void Init(Sequence sequence, Health health, Health enemyHealth)
    {
        if (sequence == null)
        {
            throw new ArgumentNullException(nameof(sequence));
        }

        if (health == null)
        {
            throw new ArgumentNullException(nameof(health));
        }

        if (enemyHealth == null)
        {
            throw new ArgumentNullException(nameof(enemyHealth));
        }

        _health = health;
        _enemyHealth = enemyHealth;
        _sequence = sequence;
        enabled = true;
    }

    protected void RotateTo(Vector3 direction)
    {
        direction.y = 0;
        Quaternion newRotation = Quaternion.LookRotation(direction);
        transform.rotation = newRotation;
        _arrow.RotateTo(newRotation);
    }

    protected void InvokeAimed(Vector3 value)
    {
        Aimed?.Invoke(value);
    }

    protected void InvokeAiming()
    {
        Aiming?.Invoke();
    }

    private void OnDead()
    {
        enabled = false;
    }
}