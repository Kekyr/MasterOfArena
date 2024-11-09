using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProgressBarFeature
{
    public class ProgressBar : MonoBehaviour
    {
        private readonly float _duration = 2f;
        private readonly float _rewardDuration = 1f;
        private readonly float _sliderDuration = 2f;

        [SerializeField] private Slider _slider;
        [SerializeField] private Image[] _points;
        [SerializeField] private Image _currentZone;
        [SerializeField] private Image _nextZone;

        [SerializeField] private ProgressBarSO _data;
        [SerializeField] private Transform _nextbutton;
        [SerializeField] private Transform _rewardButton;
        [SerializeField] private GameObject _cup;
        [SerializeField] private GameObject _coin;

        private IRewardsData _rewardsData;
        private IBiomesData _biomesData;

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
            _coinTextMesh.text = $"+{_rewardsData.CoinsReward.ToString()}";

            _cupTextMesh = _cup.GetComponentInChildren<TextMeshProUGUI>();
            _cupTextMesh.text = $"+{_rewardsData.ScoreReward.ToString()}";

            _currentZone.sprite = _biomesData.CurrentIcon;
            _nextZone.sprite = _biomesData.NextIcon;

            _startScale = transform.localScale.x;
            transform.localScale = Vector3.zero;

            _startButtonScale = _nextbutton.localScale.x;
            _nextbutton.localScale = Vector3.zero;
            _rewardButton.localScale = Vector3.zero;

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

            transform.DOScale(_startScale, _duration)
                .SetEase(Ease.OutBounce)
                .OnComplete(() =>
                {
                    _slider.DOValue(_endSliderValue, _sliderDuration)
                        .SetEase(Ease.OutBounce)
                        .OnComplete(() =>
                        {
                            _points[_currentPointIndex].sprite = _data.Sprite;
                            _cup.transform.DOScale(_startCupScale, _rewardDuration)
                                .SetEase(Ease.InBounce)
                                .OnComplete(() =>
                                {
                                    _coin.transform.DOScale(_startCoinScale, _rewardDuration)
                                        .SetEase(Ease.InBounce)
                                        .OnComplete(() =>
                                        {
                                            _nextbutton.DOScale(_startButtonScale, _duration)
                                                .SetEase(Ease.OutBounce);
                                            _rewardButton.DOScale(_startButtonScale, _duration)
                                                .SetEase(Ease.OutBounce);
                                        });
                                });
                        });
                });
        }

        public void Init(IRewardsData rewardsData, IBiomesData biomesData)
        {
            _biomesData = biomesData;
            _rewardsData = rewardsData;
            enabled = true;
        }
    }
}