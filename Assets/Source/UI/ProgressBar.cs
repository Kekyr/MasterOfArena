using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    private readonly float Duration = 2f;
    private readonly float RewardDuration = 1f;
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
    [SerializeField] private Coin _coin;

    private PlayerDataSO _playerData;

    private TextMeshProUGUI _coinTextMesh;
    private TextMeshProUGUI _cupTextMesh;

    private float _startScale;
    private float _startButtonScale;
    private float _startCupScale;
    private float _startCoinScale;

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

        if (_coin == null)
        {
            throw new ArgumentNullException(nameof(_coin));
        }

        _coinTextMesh = _coin.GetComponentInChildren<TextMeshProUGUI>();
        _coinTextMesh.text = $"+{_playerData.CoinsReward.ToString()}";

        _cupTextMesh = _cup.GetComponentInChildren<TextMeshProUGUI>();
        _cupTextMesh.text = $"+{_playerData.ScoreReward.ToString()}";

        _currentZone.sprite = _zones.Current.Icon;
        _nextZone.sprite = _zones.Next.Icon;

        _startScale = transform.localScale.x;
        transform.localScale = Vector3.zero;

        _startButtonScale = _nextbutton.transform.localScale.x;
        _nextbutton.transform.localScale = Vector3.zero;
        _rewardButton.transform.localScale = Vector3.zero;

        _startCupScale = _cup.transform.localScale.x;
        _cup.transform.localScale = Vector3.zero;

        _startCoinScale = _coin.transform.localScale.x;
        _coin.transform.localScale = Vector3.zero;

        _data.GetData(out _currentPointIndex, out _startSliderValue, out _endSliderValue);

        for (int i = 0; i < _currentPointIndex; i++)
        {
            _points[i].sprite = _data.Sprite;
        }

        _slider.value = _startSliderValue;

        transform.DOScale(_startScale, Duration)
            .SetEase(Ease.OutBounce)
            .OnComplete(() =>
            {
                _slider.DOValue(_endSliderValue, SliderDuration)
                    .SetEase(Ease.OutBounce)
                    .OnComplete(() =>
                    {
                        _points[_currentPointIndex].sprite = _data.Sprite;
                        _cup.transform.DOScale(_startCupScale, RewardDuration)
                            .SetEase(Ease.InBounce)
                            .OnComplete(() =>
                            {
                                _coin.transform.DOScale(_startCoinScale, RewardDuration)
                                    .SetEase(Ease.InBounce)
                                    .OnComplete(() =>
                                    {
                                        _nextbutton.transform.DOScale(_startButtonScale, Duration)
                                            .SetEase(Ease.OutBounce);
                                        _rewardButton.transform.DOScale(_startButtonScale, Duration)
                                            .SetEase(Ease.OutBounce);
                                    });
                            });
                    });
            });
    }

    public void Init(PlayerDataSO playerData)
    {
        _playerData = playerData;
        enabled = true;
    }
}