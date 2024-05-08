using UnityEngine;

[CreateAssetMenu(fileName = "new ProgressBarSO", menuName = "ProgressBarSO/Create new ProgressBarSO")]
public class ProgressBarSO : ScriptableObject
{
    private readonly int SliderMaxValue = 1;

    [SerializeField] private int _pointsCount;
    [SerializeField] private float _pointsInterval;

    [SerializeField] private Sprite _sprite;

    [SerializeField] private float _startSliderValue;
    [SerializeField] private float _endSliderValue;

    private int _currentPointIndex;

    public int PointsCount => _pointsCount;

    public Sprite Sprite => _sprite;

    public void GetData(out int currentPointIndex, out float startSliderValue, out float endSliderValue)
    {
        currentPointIndex = _currentPointIndex;
        startSliderValue = _startSliderValue;
        endSliderValue = _endSliderValue;

        _currentPointIndex++;

        if (_currentPointIndex >= _pointsCount)
        {
            _currentPointIndex = 0;
        }

        _startSliderValue += _pointsInterval;

        if (_startSliderValue == SliderMaxValue)
        {
            _startSliderValue = 0;
        }

        _endSliderValue += _pointsInterval;

        if (_endSliderValue > SliderMaxValue)
        {
            _endSliderValue = _pointsInterval;
        }
    }
}