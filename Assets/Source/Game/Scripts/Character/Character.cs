using System;
using System.Collections;
using Cinemachine;
using DG.Tweening;
using UnityEngine;
using Sequence = DG.Tweening.Sequence;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SFX))]
public class Character : MonoBehaviour, IAttacker
{
    private readonly float _duration = 0.02f;
    private readonly float _delay = 0.05f;
    private readonly float _waitTime = 1.5f;
    private readonly int _isDancing = Animator.StringToHash("IsDancing");

    [SerializeField] private Color _damageMarkColor;
    [SerializeField] private SFXSO _throw;
    [SerializeField] private SFXSO _win;

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
    private Vector3 _startScale;

    public event Action<Vector3, uint> Attacked;
    public event Action<Transform> Throwed;
    public event Action Aimed;
    public event Action Won;

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

        _wait = new WaitForSeconds(_waitTime);
        _startScale = transform.localScale;
        transform.localScale = Vector3.zero;
        _winCamera.Follow = transform;
        _winCamera.LookAt = transform;

        _sequence.Append(transform.DOScale(_startScale, _duration)
            .SetEase(Ease.InOutSine)
            .SetDelay(_delay));

        for (int i = 0; i < _projectiles.Length; i++)
        {
            _projectiles[i].Catched += OnCatch;
            _projectiles[i].Throwing += OnThrowing;

            if (i != _currentProjectileIndex)
            {
                _projectiles[i].enabled = false;
            }
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < _projectiles.Length; i++)
        {
            _projectiles[i].Catched -= OnCatch;
            _projectiles[i].Throwing -= OnThrowing;
        }

        _health.Died -= OnDead;
        _enemyHeath.Died -= OnEnemyDead;
    }

    public void Init(ParticleSystem confettiVFX, CinemachineVirtualCamera winCamera)
    {
        _confettiVFX = confettiVFX;
        _winCamera = winCamera;
    }

    public void Init(Health health, Sequence sequence)
    {
        _health = health;
        _sequence = sequence;
        _health.Died += OnDead;
    }

    public void Init(Health enemyHealth)
    {
        _enemyHeath = enemyHealth;
        _enemyHeath.Died += OnEnemyDead;
    }

    public void Init(Projectile[] projectiles, Popup popup)
    {
        _projectiles = projectiles;
        _popup = popup;
        enabled = true;
    }

    public void Attack(Cube cube)
    {
        if (_health.IsDead == false)
        {
            Attacked?.Invoke(cube.transform.position, cube.Damage);
        }
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
        Aimed?.Invoke();
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

        StartCoroutine(Win());
    }

    private void OnDead()
    {
        gameObject.SetActive(false);
    }

    protected virtual IEnumerator Win()
    {
        _winCamera.gameObject.SetActive(true);
        _confettiVFX.Play();
        Won?.Invoke();
        yield return _wait;
        _animator.SetBool(_isDancing, true);
        _sfx.Play(_win);
        _popup.gameObject.SetActive(true);
    }
}