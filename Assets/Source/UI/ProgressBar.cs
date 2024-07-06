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
    [SerializeField] private Image _currentZone;
    [SerializeField] private Image _nextZone;

    [SerializeField] private ProgressBarSO _data;
    [SerializeField] private ZonesSO _zones;
    [SerializeField] private NextButton _nextbutton;
    [SerializeField] private RewardButton _rewardButton;
    [SerializeField] private Cup _cup;

    private int _currentPointIndex;
    private float _startSliderValue;
    private float _endSliderValue;

    private void OnEnable()
    {
        if (_data == null)
        {
            throw new ArgumentNullException(nameof(_data));
        }

        if (_zones == null)
        {
            throw new ArgumentNullException(nameof(_zones));
        }

        if (_slider == null)
        {
            throw new ArgumentNullException(nameof(_slider));
        }

        if (_points.Length != _data.PointsCount)
        {
            throw new ArgumentOutOfRangeException(nameof(_points));
        }

        if (_currentZone == null)
        {
            throw new ArgumentNullException(nameof(_currentZone));
        }

        if (_nextZone == null)
        {
            throw new ArgumentNullException(nameof(_nextZone));
        }

        if (_nextbutton == null)
        {
            throw new ArgumentNullException(nameof(_nextbutton));
        }

        if (_rewardButton == null)
        {
            throw new ArgumentNullException(nameof(_rewardButton));
        }

        if (_cup == null)
        {
            throw new ArgumentNullException(nameof(_cup));
        }

        _currentZone.sprite = _zones.Current.Icon;
        _nextZone.sprite = _zones.Next.Icon;

        transform.localScale = Vector3.zero;
        _nextbutton.transform.localScale = Vector3.zero;
        _rewardButton.transform.localScale = Vector3.zero;
        _cup.transform.localScale = Vector3.zero;

        _data.GetData(out _currentPointIndex, out _startSliderValue, out _endSliderValue);

        for (int i = 0; i < _currentPointIndex; i++)
        {
            _points[i].sprite = _data.Sprite;
        }

        _slider.value = _startSliderValue;

        transform.DOScale(NewScale, Duration)
            .SetEase(Ease.OutBounce)
            .OnComplete(() =>
            {
                _slider.DOValue(_endSliderValue, SliderDuration)
                    .SetEase(Ease.OutBounce)
                    .OnComplete(() =>
                    {
                        _points[_currentPointIndex].sprite = _data.Sprite;
                        _cup.transform.DOScale(CupNewScale, CupDuration)
                            .SetEase(Ease.OutBounce)
                            .OnComplete(() =>
                            {
                                _nextbutton.transform.DOScale(ButtonNewScale, Duration)
                                    .SetEase(Ease.OutBounce);
                                _rewardButton.transform.DOScale(ButtonNewScale, Duration)
                                    .SetEase(Ease.OutBounce);
                            });
                    });
            });
    }
}