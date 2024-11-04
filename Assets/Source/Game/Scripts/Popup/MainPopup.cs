using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MainPopup : MonoBehaviour
{
    private readonly float _newScale = 0.9f;
    private readonly float _duration = 0.2f;
    private readonly float _newAlpha = 1f;

    [SerializeField] private Button _closeButton;
    [SerializeField] private CanvasGroup _blackout;

    protected virtual void OnEnable()
    {
        if (_closeButton == null)
        {
            throw new ArgumentNullException(nameof(_closeButton));
        }

        if (_blackout == null)
        {
            throw new ArgumentNullException(nameof(_blackout));
        }

        transform.localScale = Vector3.zero;
        transform.DOScale(_newScale, _duration)
            .SetEase(Ease.InOutSine)
            .OnComplete(() =>
            {
                InputSystem.DisableDevice(Mouse.current);
                Time.timeScale = 0f;
            });

        _blackout.gameObject.SetActive(true);
        _blackout.DOFade(_newAlpha, _duration)
            .SetEase(Ease.InOutSine);

        _closeButton.onClick.AddListener(Close);
    }

    protected virtual void OnDisable()
    {
        _closeButton.onClick.RemoveListener(Close);
    }

    protected virtual void Close()
    {
        Time.timeScale = 1f;
        InputSystem.EnableDevice(Mouse.current);

        transform.DOScale(0f, _duration)
            .SetEase(Ease.InOutSine)
            .OnComplete(() => { gameObject.SetActive(false); });

        _blackout.DOFade(0f, _duration)
            .SetEase(Ease.InOutSine)
            .OnComplete(() => { _blackout.gameObject.SetActive(false); });
    }
}