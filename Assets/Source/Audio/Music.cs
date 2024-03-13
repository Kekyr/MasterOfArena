using System;
using UnityEngine;

public class Music : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private MusicSO _music;

    private bool _canPlay = true;

    private Health _playerHealth;
    private Health _enemyHealth;

    private void OnEnable()
    {
        if (_audioSource == null)
            throw new ArgumentNullException(nameof(_audioSource));

        if (_music == null)
            throw new ArgumentNullException(nameof(_music));

        _playerHealth.Died += OnDead;
        _enemyHealth.Died += OnDead;
    }

    private void OnDisable()
    {
        _playerHealth.Died -= OnDead;
        _enemyHealth.Died -= OnDead;
    }

    private void Start()
    {
        Debug.Log("Start");
        Play(_music.GetRandomClip());
    }

    public void Init(Health playerHealth, Health enemyHealth)
    {
        if (playerHealth == null)
            throw new ArgumentNullException(nameof(playerHealth));

        if (enemyHealth == null)
            throw new ArgumentNullException(nameof(enemyHealth));

        _playerHealth = playerHealth;
        _enemyHealth = enemyHealth;
        enabled = true;
    }

    private void Play(AudioClip clip)
    {
        _audioSource.clip = clip;
        _audioSource.Play();
    }

    private void OnDead()
    {
        _audioSource.Stop();
        enabled = false;
    }
}