using System;
using System.Collections;
using Cinemachine;
using DG.Tweening;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private readonly int IsRunning = Animator.StringToHash("IsRunning");
    private readonly float MinDistance = 0.1f;

    [SerializeField] private CinemachineVirtualCamera _camera;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Animator _animator;
    [SerializeField] private SFX _sfx;
    [SerializeField] private SFXSO _footsteps;

    [SerializeField] private float _acceleration;

    [SerializeField] private float _yModifier;
    [SerializeField] private float _duration;

    private Projectile[] _projectiles;
    private Health _health;

    private Quaternion _startRotation;

    private void OnEnable()
    {
        if (_rigidbody == null)
        {
            throw new ArgumentNullException(nameof(_rigidbody));
        }

        if (_animator == null)
        {
            throw new ArgumentNullException(nameof(_animator));
        }

        foreach (Projectile projectile in _projectiles)
        {
            projectile.GetComponent<ProjectileMovement>().Ricocheted += OnRicochet;
        }

        _health.Died += OnDead;
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

    public void Init(Projectile[] projectiles, Health health)
    {
        int maxLength = 2;

        if (projectiles.Length == 0 || projectiles.Length > maxLength)
        {
            throw new ArgumentOutOfRangeException(nameof(projectiles));
        }

        if (health == null)
        {
            throw new ArgumentNullException(nameof(health));
        }

        _projectiles = projectiles;
        _health = health;
        enabled = true;
    }

    private IEnumerator Move(Vector3 endPosition)
    {
        float distance = 1;
        Vector3 direction = (endPosition - transform.position).normalized;

        transform.rotation = Quaternion.LookRotation(direction);
        _animator.SetBool(IsRunning, true);

        while (distance > MinDistance)
        {
            _rigidbody.velocity = _acceleration * direction;
            distance = Vector3.Distance(endPosition, transform.position);
            yield return null;
        }

        _animator.SetBool(IsRunning, false);
        _rigidbody.velocity = Vector3.zero;
        transform.rotation = _startRotation;
    }

    private void OnStep()
    {
        _sfx.Play(_footsteps);
    }

    private void OnRicochet(Vector3 endPosition)
    {
        endPosition = new Vector3(endPosition.x, transform.position.y, transform.position.z);
        StartCoroutine(Move(endPosition));
    }

    private void OnDead()
    {
        _camera.Follow = null;
        transform.DOMoveY(transform.position.y + _yModifier, _duration)
            .SetEase(Ease.OutQuad);
    }
}