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

    private void OnEnable()
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
        _button.Switched += OnSwitch;
    }

    private void OnDisable()
    {
        _button.Switched -= OnSwitch;
        _playerHealth.Died -= OnDead;
        _enemyHealth.Died -= OnDead;
    }

    private void Start()
    {
        Play(_music.GetRandomClip());
    }

    public void Init(Health playerHealth, Health enemyHealth, MusicButton button, AudioSettingsSO audioSettings)
    {
        if (playerHealth == null)
        {
            throw new ArgumentNullException(nameof(playerHealth));
        }

        if (enemyHealth == null)
        {
            throw new ArgumentNullException(nameof(enemyHealth));
        }

        if (button == null)
        {
            throw new ArgumentNullException(nameof(button));
        }

        if (audioSettings == null)
        {
            throw new ArgumentNullException(nameof(audioSettings));
        }

        _playerHealth = playerHealth;
        _enemyHealth = enemyHealth;
        _button = button;
        _audioSettings = audioSettings;

        _playerHealth.Died += OnDead;
        _enemyHealth.Died += OnDead;

        enabled = true;
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