using System;
using DG.Tweening;
using UnityEngine;

public class Catcher : MonoBehaviour
{
    private readonly float NewScale = 0.8f;
    private readonly float NewAimScale = 0f;
    private readonly float NewCircleScale = 0f;
    private readonly float Duration = 0.05f;
    private readonly float Delay = 0.05f;

    [SerializeField] private Movement _movement;
    [SerializeField] private Aiming _aiming;

    private Projectile[] _projectiles;
    private Sequence _sequence;
    private int _currentProjectileIndex;

    public Projectile CurrentProjectile => _projectiles[_currentProjectileIndex];

    private void OnEnable()
    {
        int maxLength = 2;

        if (_movement == null)
            throw new ArgumentNullException(nameof(_movement));

        if (_aiming == null)
            throw new ArgumentNullException(nameof(_aiming));

        transform.localScale = Vector3.zero;
        _sequence.Append(transform.DOScale(NewScale, Duration)
            .SetEase(Ease.InOutSine)
            .SetDelay(Delay));
    }

    public void Init(Projectile[] projectiles, Sequence sequence)
    {
        int maxLength = 2;

        if (projectiles.Length == 0 || projectiles.Length > maxLength)
            throw new ArgumentOutOfRangeException(nameof(projectiles));

        if (sequence == null)
            throw new ArgumentNullException(nameof(sequence));

        _sequence = sequence;
        _projectiles = projectiles;
        enabled = true;
    }

    private void OnThrow()
    {
        _projectiles[_currentProjectileIndex].OnThrow(transform.parent);

        _currentProjectileIndex++;

        if (_currentProjectileIndex == _projectiles.Length)
            _currentProjectileIndex = 0;

        _aiming.Aim.ChangeScale(NewAimScale);
        _aiming.Circle.ChangeScale(NewCircleScale);
    }
}