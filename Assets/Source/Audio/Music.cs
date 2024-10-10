using System;
using UnityEngine;

public class Music : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private MusicSO _music;

    private Health _playerHealth;
    private Health _enemyHealth;

    private MusicButton _button;
    private AudioSettingsSO _audioSettings;

    private float _volume;

    private void Awake()
    {
        if (_audioSource == null)
        {
            throw new ArgumentNullException(nameof(_audioSource));
        }

        if (_music == null)
        {
            throw new ArgumentNullException(nameof(_music));
        }

        _volume = _audioSource.volume;
        Play(_music.GetRandomClip());
    }

    private void OnDisable()
    {
        _button.Switched -= OnSwitch;
        _playerHealth.Died -= OnDead;
        _enemyHealth.Died -= OnDead;
    }

    public void Init(Health playerHealth, Health enemyHealth, MusicButton button, AudioSettingsSO audioSettings)
    {
        _playerHealth = playerHealth;
        _enemyHealth = enemyHealth;
        _button = button;
        _audioSettings = audioSettings;

        _playerHealth.Died += OnDead;
        _enemyHealth.Died += OnDead;

        _button.Switched += OnSwitch;
    }

    public void Pause()
    {
        _audioSource.volume = 0f;
    }

    public void Continue()
    {
        _audioSource.volume = _volume;
    }

    private void Play(AudioClip clip)
    {
        if (_audioSettings.IsMusicOn == false)
        {
            return;
        }

        if (clip == null)
        {
            return;
        }

        _audioSource.clip = clip;
        _audioSource.Play();
    }

    private void OnDead()
    {
        _audioSource.Stop();
    }

    private void OnSwitch()
    {
        _audioSource.Stop();
        Play(_music.GetRandomClip());
    }
}