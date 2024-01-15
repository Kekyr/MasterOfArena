using System;
using Cinemachine;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private ProjectileHelper _helper;
    [SerializeField] private string _animationTrigger;
    [SerializeField] private ProjectileMovement _movement;
    [SerializeField] private Animator _animator;
    [SerializeField] private CinemachineImpulseSource _impulseSource;
    [SerializeField] private Transform _startFlyPosition;
    [SerializeField] private float _shakeForce;

    private Catcher _catcher;
    private Animator _catcherAnimator;
    private Aiming _aiming;

    private Vector3 _startPosition;
    private Quaternion _startRotation;
    private Transform _startParent;

    public event Action Catched;
    public event Action Ricocheting;

    public bool IsFlying => transform.parent != _startParent;

    public Catcher Catcher => _catcher;

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

        _catcherAnimator = _catcher.GetComponent<Animator>();
        _aiming = _catcher.GetComponent<Aiming>();

        _movement.Init(this);

        _aiming.Aimed += OnAimed;
        _catcher.Throwed += OnThrow;

        _helper.Init(_catcher);

        _startParent = transform.parent;
    }

    private void OnDisable()
    {
        _aiming.Aimed -= OnAimed;
        _catcher.Throwed -= OnThrow;
        _helper.enabled = false;
    }

    public void Init(Catcher catcher)
    {
        if (catcher == null)
            throw new ArgumentNullException(nameof(catcher));

        _catcher = catcher;
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
        if (other.gameObject.TryGetComponent<Catcher>(out Catcher catcher) && IsFlying)
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
            _helper.ChangeScale();
            _helper.ChangeColor();
            Ricocheting?.Invoke();
        }
        else if (collision.gameObject.TryGetComponent<ReturnZone>(out ReturnZone returnZone))
        {
            _movement.Miss();
        }
    }
}