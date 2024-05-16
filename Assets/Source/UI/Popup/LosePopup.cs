using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LosePopup : Popup
{
    private readonly float NewScale = 1f;

    [SerializeField] private UpperPart _upperPart;
    [SerializeField] private LowerPart _lowerPart;

    [SerializeField] private float Duration = 1f;
    [SerializeField] private Button _nextButton;

    private void OnEnable()
    {
        if (_upperPart == null)
        {
            throw new ArgumentNullException(nameof(_upperPart));
        }

        if (_lowerPart == null)
        {
            throw new ArgumentNullException(nameof(_lowerPart));
        }

        if (_nextButton == null)
        {
            throw new ArgumentNullException(nameof(_nextButton));
        }

        _upperPart.transform.localScale = Vector3.zero;
        _lowerPart.transform.localScale = Vector3.zero;

        _upperPart.transform.DOScale(NewScale, Duration)
            .SetEase(Ease.OutBounce)
            .OnComplete(() =>
            {
                _lowerPart.transform.DOScale(NewScale, Duration)
                    .SetEase(Ease.InOutSine);
            });

        _nextButton.onClick.AddListener(Next);
    }

    private void OnDisable()
    {
        _nextButton.onClick.RemoveListener(Next);
    }

    private void Next()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}