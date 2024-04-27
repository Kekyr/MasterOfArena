using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    private readonly float NewScale = 0.9f;
    private readonly float Duration = 2f;

    private readonly float ButtonNewScale = 0.8f;

    private readonly float CupNewScale = 3f;
    private readonly float CupDuration = 0.5f;

    private readonly float SliderDuration = 2f;

    [SerializeField] private Slider _slider;
    [SerializeField] private Image[] _points;
    [SerializeField] private ProgressBarSO _data;
    [SerializeField] private NextButton _button;
    [SerializeField] private Cup _cup;

    private int _currentPointIndex;
    private float _startSliderValue;
    private float _endSliderValue;

    private void OnEnable()
    {
        if (_data == null)
            throw new ArgumentNullException(nameof(_data));

        if (_slider == null)
            throw new ArgumentNullException(nameof(_slider));

        if (_points.Length != _data.PointsCount)
            throw new ArgumentOutOfRangeException(nameof(_points));

        if (_button == null)
            throw new ArgumentNullException(nameof(_button));

        if (_cup == null)
            throw new ArgumentNullException(nameof(_cup));

        transform.localScale = Vector3.zero;
        _button.transform.localScale = Vector3.zero;
        _cup.transform.localScale = Vector3.zero;

        _data.GetData(out _currentPointIndex, out _startSliderValue, out _endSliderValue);

        for (int i = 0; i < _currentPointIndex; i++)
            _points[i].sprite = _data.Sprite;

        _slider.value = _startSliderValue;

        transform.DOScale(NewScale, Duration)
            .SetEase(Ease.OutBounce)
            .OnComplete(() =>
            {
                _slider.DOValue(_endSliderValue, SliderDuration)
                    .SetEase(Ease.OutBounce)
                    .OnComplete(() =>
                    {
                        _cup.transform.DOScale(CupNewScale, CupDuration)
                            .SetEase(Ease.OutBounce)
                            .OnComplete(() =>
                            {
                                _points[_currentPointIndex].sprite = _data.Sprite;
                                _button.transform.DOScale(NewScale, Duration)
                                    .SetEase(Ease.OutBounce);
                            });
                    });
            });
    }
}