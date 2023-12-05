using System;
using UnityEngine;

public class Catcher : MonoBehaviour
{
    [SerializeField] private string[] _animationTriggers;
    [SerializeField] private Animator _animator;
    [SerializeField] private Movement _movement;
    [SerializeField] private Aiming _aiming;

    private Projectile[] _projectiles;
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
    }

    public void Init(Projectile[] projectiles)
    {
        int maxLength = 2;

        if (projectiles.Length == 0 || projectiles.Length > maxLength)
            throw new ArgumentOutOfRangeException(nameof(projectiles));

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

        StartCoroutine(_aiming.LerpAlpha(0f));
    }
}