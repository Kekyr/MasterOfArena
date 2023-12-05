using System;
using System.Collections;
using UnityEngine;

public abstract class Aiming : MonoBehaviour
{
    [SerializeField] private Catcher _catcher;
    [SerializeField] private Aim _aim;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private uint _speed;

    public event Action<Vector3> Aimed;

    public Catcher Catcher => _catcher;

    protected virtual void OnEnable()
    {
        if (_catcher == null)
            throw new ArgumentNullException(nameof(_catcher));

        if (_aim == null)
            throw new ArgumentNullException(nameof(_aim));

        if (_canvasGroup == null)
            throw new ArgumentNullException(nameof(_canvasGroup));

        Aimed += _catcher.OnAimed;
    }

    protected virtual void OnDisable()
    {
        Aimed -= _catcher.OnAimed;
    }

    public IEnumerator LerpAlpha(float alpha)
    {
        float newAlpha = _canvasGroup.alpha;

        while (_canvasGroup.alpha != alpha)
        {
            newAlpha = Mathf.MoveTowards(newAlpha, alpha, _speed * Time.deltaTime);
            _canvasGroup.alpha = newAlpha;
            yield return null;
        }
    }

    public void RotateTo(Vector3 direction)
    {
        direction.y = 0;
        Quaternion newRotation = Quaternion.LookRotation(direction);
        transform.rotation = newRotation;
        _aim.RotateTo(newRotation);
    }

    protected abstract Vector3 TakeAim();

    protected void InvokeAimed(Vector3 value)
    {
        Action<Vector3> aimed = Aimed;
        Aimed?.Invoke(value);
    }
}