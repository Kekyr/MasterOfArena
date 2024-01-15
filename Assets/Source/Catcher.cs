using System;
using Cinemachine;
using DG.Tweening;
using UnityEngine;

public class Catcher : MonoBehaviour
{
    private readonly float NewScale = 0.8f;
    private readonly float NewAimScale = 0f;
    private readonly float NewCircleScale = 0f;
    private readonly float Duration = 0.05f;
    private readonly float Delay = 0.05f;

    [SerializeField] private CinemachineVirtualCamera _winCamera;
    [SerializeField] private Movement _movement;
    [SerializeField] private Aiming _aiming;

    private Projectile[] _projectiles;
    private Sequence _sequence;
    private Health _enemyHealth;
    private int _currentProjectileIndex;

    public event Action<Transform> Throwed;

    public Projectile CurrentProjectile => _projectiles[_currentProjectileIndex];

    private void OnEnable()
    {
        if (_winCamera == null)
            throw new ArgumentNullException(nameof(_winCamera));

        if (_movement == null)
            throw new ArgumentNullException(nameof(_movement));

        if (_aiming == null)
            throw new ArgumentNullException(nameof(_aiming));

        transform.localScale = Vector3.zero;
        _sequence.Append(transform.DOScale(NewScale, Duration)
            .SetEase(Ease.InOutSine)
            .SetDelay(Delay));

        for (int i = 0; i < _projectiles.Length; i++)
        {
            _projectiles[i].Catched += OnCatch;

            if (i != _currentProjectileIndex)
                _projectiles[i].enabled = false;
        }

        _enemyHealth.Died += OnEnemyDead;
    }

    private void OnDisable()
    {
        for (int i = 0; i < _projectiles.Length; i++)
        {
            _projectiles[i].Catched -= OnCatch;
        }

        _enemyHealth.Died -= OnEnemyDead;
    }

    public void Init(Projectile[] projectiles, Sequence sequence, Health enemyHealth)
    {
        int maxLength = 2;

        if (projectiles.Length == 0 || projectiles.Length > maxLength)
            throw new ArgumentOutOfRangeException(nameof(projectiles));

        if (sequence == null)
            throw new ArgumentNullException(nameof(sequence));

        if (enemyHealth == null)
            throw new ArgumentNullException(nameof(enemyHealth));

        _sequence = sequence;
        _projectiles = projectiles;
        _enemyHealth = enemyHealth;
        enabled = true;
    }

    private void OnThrow()
    {
        Throwed?.Invoke(transform.parent);
        _aiming.Aim.ChangeScale(NewAimScale);
        _aiming.Circle.ChangeScale(NewCircleScale);
    }

    private void OnCatch()
    {
        _projectiles[_currentProjectileIndex].enabled = false;

        _currentProjectileIndex++;

        if (_currentProjectileIndex == _projectiles.Length)
            _currentProjectileIndex = 0;

        _projectiles[_currentProjectileIndex].enabled = true;
    }

    private void OnEnemyDead()
    {
        _winCamera.gameObject.SetActive(true);
    }
}