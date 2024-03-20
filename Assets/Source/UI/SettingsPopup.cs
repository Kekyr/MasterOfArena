using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPopup : MonoBehaviour
{
    private readonly float NewScale = 1f;
    private readonly float Duration = 0.2f;

    [SerializeField] private ContinueButton _continueButton;

    private void OnEnable()
    {
        if (_continueButton == null)
            throw new ArgumentNullException(nameof(_continueButton));

        transform.localScale = Vector3.zero;
        transform.DOScale(NewScale, Duration)
            .SetEase(Ease.InOutSine)
            .OnComplete(() => { Time.timeScale = 0f; });

        _continueButton.GetComponent<Button>().onClick.AddListener(Continue);
    }

    private void OnDisable()
    {
        _continueButton.GetComponent<Button>().onClick.RemoveListener(Continue);
    }

    private void Continue()
    {
        Time.timeScale = 1f;
        transform.DOScale(0f, Duration)
            .SetEase(Ease.InOutSine)
            .OnComplete(() => { gameObject.SetActive(false); });
    }
}