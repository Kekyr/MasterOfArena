using UnityEngine;

[CreateAssetMenu(fileName = "new ProgressBarSO", menuName = "ProgressBarSO/Create new ProgressBarSO")]
public class ProgressBarSO : ScriptableObject
{
    private readonly int SliderMaxValue = 1;

    [SerializeField] private ZonesSO _zones;
    [SerializeField] private Sprite _sprite;

    [SerializeField] private int _pointsCount;
    [SerializeField] private float _pointsInterval;

    [SerializeField] private float _startSliderValue;
    [SerializeField] private float _endSliderValue;
    [SerializeField] private int _currentPointIndex;

    public int PointsCount => _pointsCount;

    public Sprite Sprite => _sprite;

    public float StartSliderValue => _startSliderValue;
    public float EndSliderValue => _endSliderValue;

    public int CurrentPointIndex => _currentPointIndex;

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
            _zones.Change();
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