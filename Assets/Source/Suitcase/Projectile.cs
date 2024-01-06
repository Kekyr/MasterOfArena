using System;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private readonly float RayDistance = 7;
    private readonly float ScaleModifier = 1.3f;
    private readonly Vector3 HalfExtents = new Vector3(0.05f, 0.05f, 0.05f);
    private readonly float ScaleDuration = 0.05f;
    private readonly float ColorDuration = 0.02f;

    [SerializeField] private string _animationTrigger;
    [SerializeField] private ProjectileMovement _movement;
    [SerializeField] private Animator _animator;
    [SerializeField] private CinemachineImpulseSource _impulseSource;
    [SerializeField] private Transform _startFlyPosition;
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private float _shakeForce;

    private Catcher _catcher;
    private Animator _catcherAnimator;
    private Aiming _aiming;

    private Vector3 _startPosition;
    private Vector3 _originalScale;
    private Quaternion _startRotation;
    private Transform _startParent;

    public event Action<Vector3> Ricocheted;

    public event Action Catched;

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

        if (_meshRenderer == null)
            throw new ArgumentNullException(nameof(_meshRenderer));

        _catcherAnimator = _catcher.GetComponent<Animator>();
        _aiming = _catcher.GetComponent<Aiming>();
        _originalScale = transform.localScale;

        _movement.Init(this);
        _aiming.Aimed += OnAimed;
    }

    private void OnDisable()
    {
        _aiming.Aimed -= OnAimed;
    }

    private void Start()
    {
        _startParent = transform.parent;
    }

    public void Init(Catcher catcher)
    {
        if (catcher == null)
            throw new ArgumentNullException(nameof(catcher));

        _catcher = catcher;
        enabled = true;
    }

    private void ChangeScale()
    {
        Vector3 newScale = _originalScale * ScaleModifier;

        transform.DOScale(newScale, ScaleDuration)
            .SetEase(Ease.InOutSine)
            .OnComplete(() =>
            {
                transform.DOScale(_originalScale, ScaleDuration)
                    .SetEase(Ease.InOutSine);
            });
    }

    private void ChangeColor()
    {
        Sequence sequence = DOTween.Sequence();
        List<Color> startColors = new List<Color>();

        for (int i = 0; i < _meshRenderer.materials.Length; i++)
        {
            Material material = _meshRenderer.materials[i];
            startColors.Add(material.color);
            sequence.Append(material.DOColor(Color.white, ColorDuration)
                .SetEase(Ease.InOutSine));
        }

        sequence.OnComplete(() =>
        {
            for (int i = 0; i < _meshRenderer.materials.Length; i++)
            {
                Material material = _meshRenderer.materials[i];
                material.DOColor(startColors[i], ColorDuration)
                    .SetEase(Ease.InOutSine);
            }
        });
    }

    private void Ricochet()
    {
        bool canRicochet = false;
        RaycastHit hit = default(RaycastHit);

        do
        {
            Vector3 direction = _movement.Ricochet();
            Physics.BoxCast(transform.position, HalfExtents, direction, out hit, transform.rotation, RayDistance);

            if (hit.point != Vector3.zero &&
                hit.collider.gameObject.TryGetComponent<CatchZone>(out CatchZone catchZone))
                canRicochet = true;
        } while (canRicochet == false);

        Ricocheted?.Invoke(hit.point);
    }

    private void OnAimed(Vector3 flyDirection)
    {
        _startPosition = transform.localPosition;
        _startRotation = transform.localRotation;
        _catcherAnimator.SetTrigger(_animationTrigger);
    }

    public void OnThrow(Transform newParent)
    {
        transform.position = _startFlyPosition.transform.position;
        transform.eulerAngles = _startFlyPosition.transform.rotation.eulerAngles;
        transform.parent = newParent;

        _originalScale = transform.localScale;

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
            ChangeScale();
            ChangeColor();
            Ricochet();
        }
        else if (collision.gameObject.TryGetComponent<ReturnZone>(out ReturnZone returnZone))
        {
            _movement.Miss();
        }
    }
}