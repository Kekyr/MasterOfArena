using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public abstract class MainButton : MonoBehaviour
{
    private readonly float NewScale = 2f;
    private readonly float Duration = 1f;

    [SerializeField] private Button _button;

    protected virtual void OnEnable()
    {
        if (_button == null)
            throw new ArgumentNullException(nameof(_button));

        transform.localScale = Vector3.zero;
        transform.DOScale(NewScale, Duration)
            .SetEase(Ease.InOutSine);

        _button.onClick.AddListener(OnClick);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnClick);
    }

    protected abstract void OnClick();
}