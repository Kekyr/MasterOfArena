using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public abstract class MainButton : MonoBehaviour
{
    private readonly float _duration = 1f;

    [SerializeField] private float _newScale;

    private Button _button;

    protected virtual void OnEnable()
    {
        _button = GetComponent<Button>();

        transform.localScale = Vector3.zero;
        transform.DOScale(_newScale, _duration)
            .SetEase(Ease.InOutSine);

        _button.onClick.AddListener(OnClick);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnClick);
    }

    protected abstract void OnClick();
}