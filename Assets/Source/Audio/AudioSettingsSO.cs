using UnityEngine;

[CreateAssetMenu(fileName = "new AudioSettingsSO", menuName = "AudioSettingsSO/Create new AudioSettingsSO")]
public class AudioSettingsSO : ScriptableObject
{
    [SerializeField] private bool _isMusicOn;
    [SerializeField] private bool _isSFXOn;

    public bool IsMusicOn => _isMusicOn;
    public bool IsSFXOn => _isSFXOn;

    public void Init(bool isMusicOn, bool isSFXOn)
    {
        _isMusicOn = isMusicOn;
        _isSFXOn = isSFXOn;
    }

    public void SwitchMusic()
    {
        _isMusicOn = !_isMusicOn;
    }

    public void SwitchSFX()
    {
        _isSFXOn = !_isSFXOn;
    }
}