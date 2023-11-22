using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class ProjectileMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;

    [SerializeField] private float _maxDistance;
    [SerializeField] private float _acceleration;
    [SerializeField] private float _minRandomX;
    [SerializeField] private float _maxRandomX;

    private Projectile _projectile;

    private bool _isReturning;
    private Vector3 _flyDirection;

    private void OnEnable()
    {
        if (_rigidbody == null)
            throw new ArgumentNullException(nameof(_rigidbody));
    }

    public void Init(Projectile projectile)
    {
        if (projectile == null)
            throw new ArgumentNullException(nameof(projectile));

        _projectile = projectile;
        enabled = true;
    }

    public void OnAimed(Vector3 flyDirection)
    {
        _flyDirection = flyDirection;
    }

    public IEnumerator Fly(Vector3 startPosition)
    {
        while (_projectile.CanFly)
        {
            float distance = Vector3.Distance(startPosition, transform.position);

            if (distance >= _maxDistance && !_isReturning)
            {
                _flyDirection = -_flyDirection;
                _isReturning = true;
            }

            _rigidbody.velocity = _acceleration * _flyDirection;

            yield return null;
        }

        _isReturning = false;
        _rigidbody.velocity = Vector3.zero;
    }

    public Vector3 Ricochet()
    {
        _flyDirection.z = -_flyDirection.z;
        _flyDirection.x = Random.Range(_minRandomX, _maxRandomX);
        _isReturning = true;
        return _flyDirection;
    }
}