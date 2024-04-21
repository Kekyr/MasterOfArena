using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public abstract class MainButton : MonoBehaviour
{
    private readonly float Duration = 1f;

    [SerializeField] private Button _button;
    [SerializeField] private float _newScale;

    protected virtual void OnEnable()
    {
        if (_button == null)
            throw new ArgumentNullException(nameof(_button));

        transform.localScale = Vector3.zero;
        transform.DOScale(_newScale, Duration)
            .SetEase(Ease.InOutSine);

        _button.onClick.AddListener(OnClick);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnClick);
    }

    protected abstract void OnClick();
}