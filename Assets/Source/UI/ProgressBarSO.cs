using UnityEngine;

[CreateAssetMenu(fileName = "new ProgressBarSO", menuName = "ProgressBarSO/Create new ProgressBarSO")]
public class ProgressBarSO : ScriptableObject
{
    [SerializeField] private int _currentPointIndex;
    [SerializeField] private int _currentSliderValue;

}