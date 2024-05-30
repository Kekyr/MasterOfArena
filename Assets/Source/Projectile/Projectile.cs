using System;
using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(ProjectileModifier))]
[RequireComponent(typeof(ProjectileMovement))]
[RequireComponent(typeof(TrailRenderer))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CinemachineImpulseSource))]
[RequireComponent(typeof(SFX))]
public class Projectile : MonoBehaviour
{
    private readonly float ColorDuration = 0.06f;

    [SerializeField] private float _shakeForce;
    [SerializeField] private string _animationTrigger;

    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private StartFlyPosition _startFlyPosition;

    [SerializeField] private SFXSO _punch;

    private Character _character;
    private Animator _animator;
    private Targeting _targeting;
    private Helper _helper;
    private CinemachineImpulseSource _impulseSource;
    private SFX _sfx;

    private ProjectileModifier _modifier;
    private ProjectileMovement _movement;
    private TrailRenderer _trail;

    private Vector3 _startPosition;
    private Quaternion _startRotation;
    private Transform _startParent;

    public event Action Catched;
    public event Action Ricocheting;
    public event Action<string> Throwing;

    public bool IsFlying => transform.parent != _startParent;

    public Character Character => _character;

    public TrailRenderer Trail => _trail;

    private void OnEnable()
    {
        if (_meshRenderer == null)
        {
            throw new ArgumentNullException(nameof(_meshRenderer));
        }

        if (_animationTrigger == null)
        {
            throw new ArgumentNullException(nameof(_animationTrigger));
        }

        if (_startFlyPosition == null)
        {
            throw new ArgumentNullException(nameof(_startFlyPosition));
        }

        if (_punch == null)
        {
            throw new ArgumentNullException(nameof(_punch));
        }

        _targeting = _character.GetComponent<Targeting>();
        _modifier = GetComponent<ProjectileModifier>();
        _movement = GetComponent<ProjectileMovement>();
        _trail = GetComponent<TrailRenderer>();
        _animator = GetComponent<Animator>();
        _impulseSource = GetComponent<CinemachineImpulseSource>();
        _sfx = GetComponent<SFX>();

        _movement.Init(this);

        _targeting.Aimed += OnAimed;
        _character.Throwed += OnThrow;

        _modifier.Init(_character);

        _startParent = transform.parent;
    }

    private void OnDisable()
    {
        _targeting.Aimed -= OnAimed;
        _character.Throwed -= OnThrow;
        _modifier.enabled = false;
    }

    public void Init(Character character, Helper helper)
    {
        if (character == null)
        {
            throw new ArgumentNullException(nameof(character));
        }

        if (helper == null)
        {
            throw new ArgumentNullException(nameof(helper));
        }

        _character = character;
        _helper = helper;
        enabled = true;
    }

    private void OnAimed(Vector3 flyDirection)
    {
        _startPosition = transform.localPosition;
        _startRotation = transform.localRotation;
        Throwing?.Invoke(_animationTrigger);
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
        {
            return;
        }

        if (collision.gameObject.transform.TryGetComponent<Cube>(out Cube cube)
            || collision.gameObject.TryGetComponent<BombPlatform>(out BombPlatform platform))
        {
            _sfx.Play(_punch);
            _impulseSource.GenerateImpulseWithForce(_shakeForce);
            _modifier.ChangeScale();
            _helper.ChangeMeshColor(_meshRenderer, Color.white, ColorDuration);
            Ricocheting?.Invoke();
        }
        else if (collision.gameObject.TryGetComponent<ReturnZone>(out ReturnZone returnZone))
        {
            _movement.Miss();
        }
    }
}