using System;
using Cinemachine;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private ProjectileModifier modifier;
    [SerializeField] private ProjectileMovement _movement;

    [SerializeField] private TrailRenderer _trail;
    [SerializeField] private Animator _animator;
    [SerializeField] private CinemachineImpulseSource _impulseSource;
    [SerializeField] private StartFlyPosition _startFlyPosition;

    [SerializeField] private float _shakeForce;
    [SerializeField] private string _animationTrigger;

    private Character _character;
    private Animator _catcherAnimator;
    private Aiming _aiming;

    private Vector3 _startPosition;
    private Quaternion _startRotation;
    private Transform _startParent;

    public event Action Catched;
    public event Action Ricocheting;

    public bool IsFlying => transform.parent != _startParent;

    public Character Character => _character;

    public TrailRenderer Trail => _trail;

    private void OnEnable()
    {
        if (_animationTrigger == null)
            throw new ArgumentNullException(nameof(_animationTrigger));

        if (_animator == null)
            throw new ArgumentNullException(nameof(_animator));

        if (_movement == null)
            throw new ArgumentNullException(nameof(_movement));

        if (_impulseSource == null)
            throw new ArgumentNullException(nameof(_impulseSource));

        if (_startFlyPosition == null)
            throw new ArgumentNullException(nameof(_startFlyPosition));

        _catcherAnimator = _character.GetComponent<Animator>();
        _aiming = _character.GetComponent<Aiming>();

        _movement.Init(this);

        _aiming.Aimed += OnAimed;
        _character.Throwed += OnThrow;

        modifier.Init(_character);

        _startParent = transform.parent;
    }

    private void OnDisable()
    {
        _aiming.Aimed -= OnAimed;
        _character.Throwed -= OnThrow;
        modifier.enabled = false;
    }

    public void Init(Character character)
    {
        if (character == null)
            throw new ArgumentNullException(nameof(character));

        _character = character;
        enabled = true;
    }

    private void OnAimed(Vector3 flyDirection)
    {
        _startPosition = transform.localPosition;
        _startRotation = transform.localRotation;
        _catcherAnimator.SetTrigger(_animationTrigger);
    }

    private void OnThrow(Transform newParent)
    {
        transform.position = _startFlyPosition.transform.position;
        transform.eulerAngles = _startFlyPosition.transform.rotation.eulerAngles;
        transform.parent = newParent;

        _animator.enabled = true;

        StartCoroutine(_movement.Fly(transform.position));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Character>(out Character catcher) && IsFlying)
        {
            _animator.enabled = false;
            transform.parent = _startParent;
            transform.localPosition = _startPosition;
            transform.localRotation = _startRotation;
            Catched?.Invoke();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_movement.IsReturning == true)
            return;

        if (collision.gameObject.TryGetComponent<Cube>(out Cube cube)
            || collision.gameObject.TryGetComponent<BombPlatform>(out BombPlatform platform))
        {
            _impulseSource.GenerateImpulseWithForce(_shakeForce);
            modifier.ChangeScale();
            modifier.ChangeColor();
            Ricocheting?.Invoke();
        }
        else if (collision.gameObject.TryGetComponent<ReturnZone>(out ReturnZone returnZone))
        {
            _movement.Miss();
        }
    }
}