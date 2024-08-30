using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MainPopup : Popup
{
    private readonly float NewScale = 0.9f;
    private readonly float Duration = 0.2f;
    private readonly float NewAlpha = 1f;

    [SerializeField] private Button _button;
    [SerializeField] private CanvasGroup _blackout;

    protected virtual void OnEnable()
    {
        if (_button == null)
        {
            throw new ArgumentNullException(nameof(_button));
        }

        if (_blackout == null)
        {
            throw new ArgumentNullException(nameof(_blackout));
        }

        transform.localScale = Vector3.zero;
        transform.DOScale(NewScale, Duration)
            .SetEase(Ease.InOutSine)
            .OnComplete(() =>
            {
                InputSystem.DisableDevice(Mouse.current);
                Time.timeScale = 0f;
            });

        _blackout.gameObject.SetActive(true);
        _blackout.DOFade(NewAlpha, Duration)
            .SetEase(Ease.InOutSine);

        _button.onClick.AddListener(Close);
    }

    protected virtual void OnDisable()
    {
        _button.onClick.RemoveListener(Close);
    }

    protected virtual void Close()
    {
        Time.timeScale = 1f;
        InputSystem.EnableDevice(Mouse.current);

        transform.DOScale(0f, Duration)
            .SetEase(Ease.InOutSine)
            .OnComplete(() => { gameObject.SetActive(false); });

        _blackout.DOFade(0f, Duration)
            .SetEase(Ease.InOutSine)
            .OnComplete(() => { _blackout.gameObject.SetActive(false); });
    }
}