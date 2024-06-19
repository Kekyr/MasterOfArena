using System;
using System.Collections;
using Cinemachine;
using DG.Tweening;
using UnityEngine;
using Sequence = DG.Tweening.Sequence;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SFX))]
public class Character : MonoBehaviour
{
    private readonly float Duration = 0.05f;
    private readonly float Delay = 0.05f;
    private readonly float WaitTime = 1.5f;
    private readonly int IsDancing = Animator.StringToHash("IsDancing");

    [SerializeField] private Color _damageMarkColor;
    [SerializeField] private SFXSO _throw;
    [SerializeField] private SFXSO _win;
    [SerializeField] private Vector3 _startScale;

    private Projectile[] _projectiles;
    private Animator _animator;
    private Health _health;
    private Health _enemyHeath;
    private SFX _sfx;

    private CinemachineVirtualCamera _winCamera;
    private ParticleSystem _confettiVFX;

    private Sequence _sequence;
    private WaitForSeconds _wait;
    private Popup _popup;

    private int _currentProjectileIndex;

    public event Action<Cube> Attacking;
    public event Action<Transform> Throwed;

    public Projectile CurrentProjectile => _projectiles[_currentProjectileIndex];
    public Color DamageMarkColor => _damageMarkColor;

    protected virtual void OnEnable()
    {
        if (_throw == null)
        {
            throw new ArgumentNullException(nameof(_throw));
        }

        if (_win == null)
        {
            throw new ArgumentNullException(nameof(_throw));
        }

        _animator = GetComponent<Animator>();
        _sfx = GetComponent<SFX>();

        _wait = new WaitForSeconds(WaitTime);
        _startScale = transform.localScale;
        transform.localScale = Vector3.zero;
        _winCamera.Follow = transform;
        _winCamera.LookAt = transform;

        _sequence.Append(transform.DOScale(_startScale, Duration)
            .SetEase(Ease.InOutSine)
            .SetDelay(Delay));

        for (int i = 0; i < _projectiles.Length; i++)
        {
            _projectiles[i].Catched += OnCatch;
            _projectiles[i].Throwing += OnThrowing;

            if (i != _currentProjectileIndex)
            {
                _projectiles[i].enabled = false;
            }
        }

        _enemyHeath.Died += OnEnemyDead;
    }

    private void OnDisable()
    {
        for (int i = 0; i < _projectiles.Length; i++)
        {
            _projectiles[i].Catched -= OnCatch;
            _projectiles[i].Throwing -= OnThrowing;
        }

        _enemyHeath.Died -= OnEnemyDead;
    }

    public void Init(Projectile[] projectiles, Sequence sequence, Health health, Health enemyHealth, Popup popup)
    {
        int maxLength = 2;

        if (projectiles.Length == 0 || projectiles.Length > maxLength)
        {
            throw new ArgumentOutOfRangeException(nameof(projectiles));
        }

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

        if (popup == null)
        {
            throw new ArgumentNullException(nameof(popup));
        }

        _sequence = sequence;
        _projectiles = projectiles;
        _health = health;
        _enemyHeath = enemyHealth;
        _popup = popup;
        enabled = true;
    }

    public void Init(ParticleSystem confettiVFX, CinemachineVirtualCamera winCamera)
    {
        if (confettiVFX == null)
        {
            throw new ArgumentNullException(nameof(confettiVFX));
        }

        if (winCamera == null)
        {
            throw new ArgumentNullException(nameof(winCamera));
        }

        _confettiVFX = confettiVFX;
        _winCamera = winCamera;
    }

    public void Attack(Cube cube)
    {
        Attacking?.Invoke(cube);
    }

    private void OnThrowing(string animationTrigger)
    {
        _animator.SetTrigger(animationTrigger);
    }

    private void OnThrowStarted()
    {
        _sfx.Play(_throw);
    }

    private void OnThrowEnded()
    {
        Throwed?.Invoke(transform.parent);
    }

    private void OnCatch()
    {
        _projectiles[_currentProjectileIndex].enabled = false;
        _projectiles[_currentProjectileIndex].Trail.enabled = false;

        _currentProjectileIndex++;

        if (_currentProjectileIndex == _projectiles.Length)
        {
            _currentProjectileIndex = 0;
        }

        _projectiles[_currentProjectileIndex].enabled = true;
        _projectiles[_currentProjectileIndex].Trail.enabled = true;
    }

    private void OnEnemyDead()
    {
        foreach (Projectile projectile in _projectiles)
        {
            projectile.gameObject.SetActive(false);
        }

        if (_health.IsDead != true)
        {
            StartCoroutine(Win());
        }
    }

    protected virtual IEnumerator Win()
    {
        _winCamera.gameObject.SetActive(true);
        _confettiVFX.Play();
        yield return _wait;
        _animator.SetBool(IsDancing, true);
        _sfx.Play(_win);
        _popup.gameObject.SetActive(true);
    }
}