using System;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private Image[] _points;
    [SerializeField] private ProgressBarSO _data;

    private void OnEnable()
    {
        int imageNumber = 5;

        if (_slider == null)
            throw new ArgumentNullException(nameof(_slider));

        if (_points.Length != imageNumber)
            throw new ArgumentOutOfRangeException(nameof(_points));
        
    }
    
    
}