using DG.Tweening;
using System;
using UnityEngine;

public class Catcher : MonoBehaviour
{
    [SerializeField] private string[] _animationTriggers;
    [SerializeField] private Animator _animator;
    [SerializeField] private Movement _movement;
    [SerializeField] private Aiming _aiming;

    private Projectile[] _projectiles;
    private Sequence _sequence;
    private int _currentProjectileIndex;

    public Projectile CurrentProjectile => _projectiles[_currentProjectileIndex];

    private void OnEnable()
    {
        int maxLength = 2;

        if (_animationTriggers.Length == 0 || _animationTriggers.Length > maxLength)
            throw new ArgumentOutOfRangeException(nameof(_animationTriggers));

        if (_movement == null)
            throw new ArgumentNullException(nameof(_movement));

        if (_animator == null)
            throw new ArgumentNullException(nameof(_animator));

        if (_aiming == null)
            throw new ArgumentNullException(nameof(_aiming));

        transform.localScale = Vector3.zero;
        _sequence.Append(transform.DOScale(0.8f, 0.05f)
            .SetEase(Ease.InOutSine)
            .SetDelay(0.05f));
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

    public void OnRicochet(Vector3 endPoint)
    {
        endPoint = new Vector3(endPoint.x, transform.position.y, transform.position.z);
        _movement.OnRicochet(endPoint);
    }

    public void OnAimed(Vector3 flyDirection)
    {
        _projectiles[_currentProjectileIndex].OnAimed(flyDirection);
        _animator.SetTrigger(_animationTriggers[_currentProjectileIndex]);
    }

    public void OnThrow()
    {
        _projectiles[_currentProjectileIndex].OnThrow(transform.parent);

        _currentProjectileIndex++;

        if (_currentProjectileIndex == _projectiles.Length)
            _currentProjectileIndex = 0;

        _aiming.Aim.ChangeScale(0f);
        _aiming.Circle.ChangeScale(0f);
    }
}