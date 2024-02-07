using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Music : MonoBehaviour
{
    private static Music Instance;

    [SerializeField] private AudioClip[] _tracks;
    [SerializeField] private AudioSource _audioSource;

    private bool _canPlay = true;

    private Health _playerHealth;
    private Health _enemyHealth;

    private void OnEnable()
    {
        if (_audioSource == null)
            throw new ArgumentNullException(nameof(_audioSource));

        if (Instance != null)
            Destroy(gameObject);

        Instance = this;
        DontDestroyOnLoad(gameObject);

        _playerHealth.Died += OnDead;
        _enemyHealth.Died += OnDead;

        StartCoroutine(Play());
    }

    private void OnDisable()
    {
        _playerHealth.Died -= OnDead;
        _enemyHealth.Died -= OnDead;
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

    private IEnumerator Play()
    {
        int currentTrackIndex = Random.Range(0, _tracks.Length);
        int nextTrackIndex = currentTrackIndex;

        while (_canPlay)
        {
            _audioSource.clip = _tracks[currentTrackIndex];
            _audioSource.Play();

            yield return new WaitForSeconds(_tracks[currentTrackIndex].length);

            while (currentTrackIndex == nextTrackIndex)
                nextTrackIndex = Random.Range(0, _tracks.Length);

            currentTrackIndex = nextTrackIndex;
        }
    }

    private void OnDead()
    {
        _canPlay = false;
        _audioSource.Stop();
    }

    private void OnSceneLoaded()
    {
        _canPlay = true;
    }
}