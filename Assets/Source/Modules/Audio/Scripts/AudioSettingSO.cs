using UnityEngine;

[CreateAssetMenu(fileName = "new AudioSettingSO", menuName = "AudioSettingSO/Create new AudioSettingSO")]
public class AudioSettingSO : ScriptableObject
{
    [SerializeField] private bool _isOn;

    public bool IsOn => _isOn;

    public void Init(bool isOn)
    {
        _isOn = isOn;
    }

    public void Switch()
    {
        _isOn = !_isOn;
    }
}
