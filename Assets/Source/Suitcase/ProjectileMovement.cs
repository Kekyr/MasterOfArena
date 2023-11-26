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

    private Vector3 _flyDirection;

    public bool IsReturning { get; private set; }

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
        while (_projectile.IsFlying)
        {
            float distance = Vector3.Distance(startPosition, transform.position);

            if (distance >= _maxDistance && !IsReturning)
            {
                _flyDirection = -_flyDirection;
                IsReturning = true;
            }

            _rigidbody.velocity = _acceleration * _flyDirection;

            yield return null;
        }

        IsReturning = false;
        _rigidbody.velocity = Vector3.zero;
    }

    public Vector3 Ricochet()
    {
        _flyDirection.z = -_flyDirection.z;
        _flyDirection.x = Random.Range(_minRandomX, _maxRandomX);
        IsReturning = true;
        return _flyDirection;
    }
}