using System;
using DG.Tweening;
using UnityEngine;

public class BombPlatform : MonoBehaviour
{
    private readonly float _bombScaleXOffset = 0.2f;
    private readonly float _bombScaleDuration = 1f;
    private readonly float _newScale = 1f;
    private readonly float _scaleDuration = 0.02f;
    private readonly float _platformScaleYDuration = 0.1f;
    private readonly float _moveYDuration = 0.1f;
    private readonly float _lightningMoveDuration = 0.7f;
    private readonly float _colorDuration = 0.25f;
    private readonly float _minFusePercent = 0.1f;
    private readonly float _maxFusePercent = 0.3f;

    [SerializeField] private Transform _platform;
    [SerializeField] private Transform _bomb;

    [SerializeField] private ParticleSystem _lightningTrail;
    [SerializeField] private ParticleSystem _fuse;

    private IValueGiver _valueGiver;
    private IAttacker _attacker;
    private IMeshChanger _helper;

    private Sequence _sequence;
    private MeshRenderer _bombMeshRenderer;

    private float _startPlatformScaleY;
    private float _startBombY;
    private float _startBombScaleX;

    public event Action<uint> Attacked;

    private void OnEnable()
    {
        if (_platform == null)
        {
            throw new ArgumentNullException(nameof(_platform));
        }

        if (_bomb == null)
        {
            throw new ArgumentNullException(nameof(_bomb));
        }

        if (_lightningTrail == null)
        {
            throw new ArgumentNullException(nameof(_lightningTrail));
        }

        if (_fuse == null)
        {
            throw new ArgumentNullException(nameof(_fuse));
        }

        _bombMeshRenderer = _bomb.GetComponentInChildren<MeshRenderer>();

        _startPlatformScaleY = _platform.localScale.y;
        _startBombY = _bombMeshRenderer.transform.position.y;
        _startBombScaleX = _bombMeshRenderer.transform.localScale.x;

        transform.localScale = Vector3.zero;
        _sequence.Append(transform.DOScale(_newScale, _scaleDuration)
            .SetEase(Ease.InOutSine));

        _valueGiver.ValueChanged += OnValueChanged;
        _attacker.Attacked += OnAttacked;
    }

    private void OnDisable()
    {
        _valueGiver.ValueChanged -= OnValueChanged;
        _attacker.Attacked -= OnAttacked;
    }

    public void Init(Sequence sequence, IValueGiver valueGiver, IAttacker attacker, IMeshChanger helper)
    {
        _helper = helper;
        _sequence = sequence;
        _valueGiver = valueGiver;
        _attacker = attacker;
        enabled = true;
    }

    private void OnValueChanged(float value)
    {
        float maxPercent = 100;
        float percent = value / maxPercent;

        _platform.DOScaleY(_startPlatformScaleY * percent, _platformScaleYDuration)
            .SetEase(Ease.InOutSine)
            .OnComplete(() =>
            {
                _bomb.transform.DOMoveY(_startBombY * percent, _moveYDuration)
                    .SetEase(Ease.OutBounce)
                    .OnComplete(() =>
                    {
                        float bombOffsetMultiplier = 1 - percent;

                        _helper.ChangeMeshColor(_bombMeshRenderer, Color.red, _colorDuration);

                        _bombMeshRenderer.transform.DOScaleX(
                            _startBombScaleX + (_bombScaleXOffset * bombOffsetMultiplier),
                            _bombScaleDuration);

                        if (percent >= _minFusePercent && percent <= _maxFusePercent)
                        {
                            _fuse.Play();
                        }
                    });
            });
    }

    private void OnAttacked(Vector3 position, uint damage)
    {
        _lightningTrail.transform.position = position;
        _lightningTrail.Play();
        _lightningTrail.transform.DOMove(transform.position, _lightningMoveDuration)
            .SetEase(Ease.InOutSine)
            .OnComplete(() =>
            {
                Attacked?.Invoke(damage);
            });
    }
}