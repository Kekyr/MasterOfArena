using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.TestTools;

[RequireComponent(typeof(ProjectileModifier))]
[RequireComponent(typeof(ProjectileMovement))]
[RequireComponent(typeof(TrailRenderer))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CinemachineImpulseSource))]
[RequireComponent(typeof(SFX))]
public class Projectile : MonoBehaviour
{
    private readonly float ColorDuration = 0.06f;
    private readonly Color NewColor = Color.white;

    [SerializeField] private float _shakeForce;
    [SerializeField] private string _animationTrigger;

    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private StartFlyPosition _startFlyPosition;

    [SerializeField] private SFXSO _punch;
    [SerializeField] private Collider _catch;

    private Character _character;
    private Animator _animator;
    private Targeting _targeting;
    private Helper _helper;
    private CinemachineImpulseSource _impulseSource;
    private SFX _sfx;
    private Health _health;

    private ProjectileModifier _modifier;
    private ProjectileMovement _movement;
    private TrailRenderer _trail;

    private Vector3 _startPosition;
    private Quaternion _startRotation;
    private Vector3 _startScale;
    private Transform _startParent;
    private CatchZone _catchZone;

    public event Action Catched;
    public event Action Ricocheting;
    public event Action<string> Throwing;

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

        if (_catch == null)
        {
            throw new ArgumentNullException(nameof(_catch));
        }

        _targeting = _character.GetComponent<Targeting>();
        _modifier = GetComponent<ProjectileModifier>();
        _movement = GetComponent<ProjectileMovement>();
        _trail = GetComponent<TrailRenderer>();
        _animator = GetComponent<Animator>();
        _impulseSource = GetComponent<CinemachineImpulseSource>();
        _sfx = GetComponent<SFX>();

        _movement.Init(_catchZone);
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

    public void Init(Character character, Helper helper, CatchZone catchZone)
    {
        _character = character;
        _helper = helper;
        _catchZone = catchZone;
        enabled = true;
    }

    private void OnAimed(Vector3 flyDirection)
    {
        _startPosition = transform.localPosition;
        _startRotation = transform.localRotation;
        _startScale = transform.localScale;
        Throwing?.Invoke(_animationTrigger);
    }

    private void OnThrow(Transform newParent)
    {
        transform.position = _startFlyPosition.transform.position;
        transform.eulerAngles = _startFlyPosition.transform.rotation.eulerAngles;
        transform.parent = newParent;

        _animator.enabled = true;

        _movement.StartFly(transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Character>(out Character catcher) && _movement.IsFlying == true)
        {
            _movement.StopFly();
            _animator.enabled = false;
            _catch.enabled = false;
            transform.parent = _startParent;
            transform.localPosition = _startPosition;
            transform.localRotation = _startRotation;
            transform.localScale = _startScale;
            Catched?.Invoke();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<BombPlatform>(out BombPlatform bombPlatform))
        {
            OnRicocheting();
            return;
        }

        if (collision.gameObject.transform.TryGetComponent<Cube>(out Cube cube)
            && _movement.IsReturning == false)
        {
            cube.OnCollision(this);
            OnRicocheting();
            return;
        }

        if (collision.gameObject.TryGetComponent<ReturnZone>(out ReturnZone returnZone)
            && _movement.IsReturning == false)
        {
            _movement.Miss();
        }
    }

    private void OnRicocheting()
    {
        _sfx.Play(_punch);
        _impulseSource.GenerateImpulseWithForce(_shakeForce);
        _modifier.ChangeScale();
        _helper.ChangeMeshColor(_meshRenderer, NewColor, ColorDuration);
        Ricocheting?.Invoke();
    }
}