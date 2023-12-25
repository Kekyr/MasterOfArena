using System;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private readonly float RayDistance = 7;
    private readonly float ScaleModifier = 1.3f;

    [SerializeField] private ProjectileMovement _movement;
    [SerializeField] private Animator _animator;
    [SerializeField] private CinemachineImpulseSource _impulseSource;
    [SerializeField] private Transform _startFlyPosition;
    [SerializeField] private MeshRenderer _meshRenderer;

    private Catcher _catcher;
    private CameraShake _cameraShake;

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

        Ricocheted += _catcher.OnRicochet;

        _originalScale = transform.localScale;
        _movement.Init(this);
    }

    private void OnDisable()
    {
        Ricocheted -= _catcher.OnRicochet;
    }

    private void Start()
    {
        _startParent = transform.parent;
    }

    public void Init(Catcher catcher, CameraShake cameraShake)
    {
        if (catcher == null)
            throw new ArgumentNullException(nameof(catcher));

        if (cameraShake == null)
            throw new ArgumentNullException(nameof(cameraShake));

        _catcher = catcher;
        _cameraShake = cameraShake;
        enabled = true;
    }

    private void ChangeScale()
    {
        Vector3 newScale = _originalScale * ScaleModifier;

        transform.DOScale(newScale, 0.05f)
            .SetEase(Ease.InOutSine)
            .OnComplete(() =>
            {
                transform.DOScale(_originalScale, 0.05f)
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
            sequence.Append(material.DOColor(Color.white, 0.02f)
                .SetEase(Ease.InOutSine));
        }

        sequence.OnComplete(() =>
        {
            for (int i = 0; i < _meshRenderer.materials.Length; i++)
            {
                Material material = _meshRenderer.materials[i];
                material.DOColor(startColors[i], 0.02f)
                    .SetEase(Ease.InOutSine);
            }
        });
    }

    public void OnAimed(Vector3 flyDirection)
    {
        _startPosition = transform.localPosition;
        _startRotation = transform.localRotation;
        _movement.OnAimed(flyDirection);
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
        if (collision.gameObject.TryGetComponent<Cube>(out Cube cube) && _movement.IsReturning == false)
        {
            ChangeScale();
            ChangeColor();
            Vector3 direction = _movement.Ricochet();
            Physics.Raycast(transform.position, direction, out RaycastHit hit, RayDistance);
            Debug.DrawRay(transform.position, direction * RayDistance, Color.red, 3f);
            Ricocheted?.Invoke(hit.point);
        }
    }
}