using UnityEngine;

namespace ProgressBarFeature
{
    [CreateAssetMenu(fileName = "new ProgressBarSO", menuName = "ProgressBarSO/Create new ProgressBarSO")]
    public class ProgressBarSO : ScriptableObject
    {
        private readonly int _sliderMaxValue = 1;

        [SerializeField] private Sprite _sprite;

        [SerializeField] private int _pointsCount;
        [SerializeField] private float _pointsInterval;

        [SerializeField] private float _startSliderValue;
        [SerializeField] private float _endSliderValue;
        [SerializeField] private int _currentPointIndex;

        private IBiomesData _biomesData;

        public int PointsCount => _pointsCount;
        public Sprite Sprite => _sprite;
        public float StartSliderValue => _startSliderValue;
        public float EndSliderValue => _endSliderValue;
        public int CurrentPointIndex => _currentPointIndex;

        public void Init(IBiomesData biomesData)
        {
            _biomesData = biomesData;
        }

        public void Init(int currentPointIndex, float startSliderValue, float endSliderValue)
        {
            _currentPointIndex = currentPointIndex;
            _startSliderValue = startSliderValue;
            _endSliderValue = endSliderValue;
        }

        public void GetData(out int currentPointIndex, out float startSliderValue, out float endSliderValue)
        {
            currentPointIndex = _currentPointIndex;
            startSliderValue = _startSliderValue;
            endSliderValue = _endSliderValue;

            _currentPointIndex++;

            if (_currentPointIndex >= _pointsCount)
            {
                _currentPointIndex = 0;
                _biomesData.Change();
            }

            _startSliderValue += _pointsInterval;

            if (_startSliderValue == _sliderMaxValue)
            {
                _startSliderValue = 0;
            }

            _endSliderValue += _pointsInterval;

            if (_endSliderValue > _sliderMaxValue)
            {
                _endSliderValue = _pointsInterval;
            }
        }
    }
}