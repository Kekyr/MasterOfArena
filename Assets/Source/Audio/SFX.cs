using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class SFX : MonoBehaviour
{
    private readonly int minPitch = 1;
    private readonly int maxPitch = 4;

    [SerializeField] private AudioSource _audioSource;

    private AudioSettingsSO _audioSettings;
    private SFXButton _button;

    private float _defaultVolume;
    private float _defaultPitch;

    private void OnEnable()
    {
        if (_audioSource == null)
            throw new ArgumentNullException(nameof(_audioSource));

        _button.Switched += OnSwitch;
    }

    private void OnDisable()
    {
        _button.Switched -= OnSwitch;
    }

    private void Start()
    {
        _defaultVolume = _audioSource.volume;
        _defaultPitch = _audioSource.pitch;
    }

    public void Init(SFXButton button, AudioSettingsSO audioSettings)
    {
        if (button == null)
            throw new ArgumentNullException(nameof(button));

        if (audioSettings == null)
            throw new ArgumentNullException(nameof(audioSettings));

        _button = button;
        _audioSettings = audioSettings;
        enabled = true;
    }

    public void Play(SFXSO sfx)
    {
        if (_audioSettings.IsSFXOn == false)
            return;

        AudioClip randomClip = sfx.GetRandomClip();

        if (randomClip == null)
            return;

        int randomPitch = Random.Range(minPitch, maxPitch);

        if (sfx.Volume == 0)
            _audioSource.volume = _defaultVolume;
        else
            _audioSource.volume = sfx.Volume;

        if (sfx.CanPitch)
            _audioSource.pitch = randomPitch;
        else
            _audioSource.pitch = _defaultPitch;

        _audioSource.PlayOneShot(randomClip);
    }

    private void OnSwitch()
    {
        _audioSource.Stop();
    }
}