using System;
using DG.Tweening;
using UnityEngine;

public abstract class Aiming : MonoBehaviour
{
    [SerializeField] private Catcher _catcher;
    [SerializeField] private Aim _aim;
    [SerializeField] private Circle _circle;

    private Sequence _sequence;

    public event Action<Vector3> Aimed;

    public Catcher Catcher => _catcher;

    public Aim Aim => _aim;

    public Circle Circle => _circle;

    protected virtual void OnEnable()
    {
        if (_catcher == null)
            throw new ArgumentNullException(nameof(_catcher));

        if (_aim == null)
            throw new ArgumentNullException(nameof(_aim));

        if (_circle == null)
            throw new ArgumentNullException(nameof(_circle));

        Aimed += _catcher.OnAimed;

        _circle.transform.localScale = Vector3.zero;

        _sequence.Append(_circle.transform.DOScale(0.02f, 0.1f)
            .SetEase(Ease.InBounce));
    }

    protected virtual void OnDisable()
    {
        Aimed -= _catcher.OnAimed;
    }

    public void Init(Sequence sequence)
    {
        if (sequence == null)
            throw new ArgumentNullException();

        _sequence = sequence;
    }

    public void RotateTo(Vector3 direction)
    {
        direction.y = 0;
        Quaternion newRotation = Quaternion.LookRotation(direction);
        transform.rotation = newRotation;
        _aim.RotateTo(newRotation);
    }

    protected void InvokeAimed(Vector3 value)
    {
        Action<Vector3> aimed = Aimed;
        Aimed?.Invoke(value);
    }
}