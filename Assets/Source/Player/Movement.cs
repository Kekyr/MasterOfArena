using System;
using System.Collections;
using Cinemachine;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SFX))]
public class Movement : MonoBehaviour
{
    private readonly int IsRunning = Animator.StringToHash("IsRunning");
    private readonly float MinDistance = 0.1f;
    private readonly float Acceleration = 7f;

    [SerializeField] private SFXSO _footsteps;

    [SerializeField] private float _yModifier;
    [SerializeField] private float _duration;

    private CinemachineVirtualCamera _camera;
    private Projectile[] _projectiles;
    private SFX _sfx;

    private Health _health;
    private Rigidbody _rigidbody;
    private Animator _animator;

    private Quaternion _startRotation;
    private bool _isRunning;
    private float _distance;
    private Vector3 _direction;
    private Vector3 _endPosition;

    private void OnEnable()
    {
        if (_footsteps == null)
        {
            throw new ArgumentNullException(nameof(_footsteps));
        }

        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _sfx = GetComponent<SFX>();

        foreach (Projectile projectile in _projectiles)
        {
            projectile.GetComponent<ProjectileMovement>().Ricocheted += OnRicochet;
        }
    }

    private void OnDisable()
    {
        foreach (Projectile projectile in _projectiles)
        {
            projectile.GetComponent<ProjectileMovement>().Ricocheted -= OnRicochet;
        }

        _health.Died -= OnDead;
    }

    private void Awake()
    {
        _startRotation = transform.rotation;
    }

    private void FixedUpdate()
    {
        if (_isRunning == true)
        {
            _rigidbody.velocity = Acceleration * _direction;
            _distance = Vector3.Distance(_endPosition, transform.position);

            if (_distance <= MinDistance)
            {
                _isRunning = false;
                _animator.SetBool(IsRunning, false);
                _rigidbody.velocity = Vector3.zero;
                transform.rotation = _startRotation;
            }
        }
    }

    public void Init(Projectile[] projectiles, Health health, CinemachineVirtualCamera camera)
    {
        _camera = camera;
        _projectiles = projectiles;
        _health = health;

        _health.Died += OnDead;

        enabled = true;
    }

    private void Move()
    {
        //Debug.Log("I'm moving");
        _distance = 1;
        _direction = (_endPosition - transform.position).normalized;

        transform.rotation = Quaternion.LookRotation(_direction);
        _animator.SetBool(IsRunning, true);

        _isRunning = true;
    }

    private void OnStep()
    {
        _sfx.Play(_footsteps);
    }

    private void OnRicochet(Vector3 endPosition)
    {
        _endPosition = new Vector3(endPosition.x, transform.position.y, transform.position.z);
        Move();
    }

    private void OnDead()
    {
        _camera.Follow = null;
        transform.DOMoveY(transform.position.y + _yModifier, _duration)
            .SetEase(Ease.OutQuad);
    }
}