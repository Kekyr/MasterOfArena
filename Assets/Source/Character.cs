using System;
using System.Collections;
using Cinemachine;
using DG.Tweening;
using UnityEngine;
using Sequence = DG.Tweening.Sequence;

public class Character : MonoBehaviour
{
    private readonly float NewScale = 0.8f;
    private readonly float NewAimScale = 0f;
    private readonly float NewCircleScale = 0f;
    private readonly float Duration = 0.05f;
    private readonly float Delay = 0.05f;
    private readonly float WaitTime = 1.5f;
    private readonly int IsDancing = Animator.StringToHash("IsDancing");

    [SerializeField] private Color _damageMarkColor;
    [SerializeField] private CinemachineVirtualCamera _winCamera;
    [SerializeField] private Movement _movement;
    [SerializeField] private Aiming _aiming;
    [SerializeField] private Animator _animator;
    [SerializeField] private ParticleSystem _confettiVFX;
    [SerializeField] private SFX _sfx;
    [SerializeField] private SFXSO _throw;
    [SerializeField] private SFXSO _win;

    private Projectile[] _projectiles;
    private Sequence _sequence;
    private Health _enemyHeath;
    private WaitForSeconds _wait;

    private int _currentProjectileIndex;

    public event Action<Cube> Attacking;
    public event Action<Transform> Throwed;

    public Projectile CurrentProjectile => _projectiles[_currentProjectileIndex];

    public Color DamageMarkColor => _damageMarkColor;

    private void OnEnable()
    {
        if (_winCamera == null)
            throw new ArgumentNullException(nameof(_winCamera));

        if (_movement == null)
            throw new ArgumentNullException(nameof(_movement));

        if (_aiming == null)
            throw new ArgumentNullException(nameof(_aiming));

        if (_animator == null)
            throw new ArgumentNullException(nameof(_animator));

        if (_sfx == null)
            throw new ArgumentNullException(nameof(_sfx));

        if (_throw == null)
            throw new ArgumentNullException(nameof(_throw));

        if (_win == null)
            throw new ArgumentNullException(nameof(_throw));

        _wait = new WaitForSeconds(WaitTime);

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

        _enemyHeath.Died += OnEnemyDead;
    }

    private void OnDisable()
    {
        for (int i = 0; i < _projectiles.Length; i++)
        {
            _projectiles[i].Catched -= OnCatch;
        }

        _enemyHeath.Died -= OnEnemyDead;
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
        _enemyHeath = enemyHealth;
        enabled = true;
    }

    public void Attack(Cube cube)
    {
        Attacking?.Invoke(cube);
    }

    private void OnThrowStarted()
    {
        _sfx.Play(_throw);
    }

    private void OnThrowEnded()
    {
        Throwed?.Invoke(transform.parent);
        _aiming.Aim.ChangeScale(NewAimScale);
        _aiming.Circle.ChangeScale(NewCircleScale);
    }

    private void OnCatch()
    {
        _projectiles[_currentProjectileIndex].enabled = false;
        _projectiles[_currentProjectileIndex].Trail.enabled = false;

        _currentProjectileIndex++;

        if (_currentProjectileIndex == _projectiles.Length)
            _currentProjectileIndex = 0;

        _projectiles[_currentProjectileIndex].enabled = true;
        _projectiles[_currentProjectileIndex].Trail.enabled = true;
    }

    private void OnEnemyDead()
    {
        foreach (Projectile projectile in _projectiles)
            projectile.gameObject.SetActive(false);

        StartCoroutine(Win());
    }

    private IEnumerator Win()
    {
        _winCamera.gameObject.SetActive(true);
        _confettiVFX.Play();
        yield return _wait;
        _animator.SetBool(IsDancing, true);
        _sfx.Play(_win);
    }
}