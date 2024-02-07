using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class SFX : MonoBehaviour
{
    private readonly int minPitch = 1;
    private readonly int maxPitch = 4;

    [SerializeField] private AudioSource _audioSource;

    private float _defaultVolume;

    private void OnEnable()
    {
        if (_audioSource == null)
            throw new ArgumentNullException(nameof(_audioSource));
    }

    private void Start()
    {
        _defaultVolume = _audioSource.volume;
    }

    public void Play(SFXSO sfx)
    {
        int lastIndex = sfx.Clips.Count - 1;
        int randomIndex = Random.Range(0, lastIndex);
        int randomPitch = Random.Range(minPitch, maxPitch);

        if (sfx.Volume == 0)
            _audioSource.volume = _defaultVolume;
        else
            _audioSource.volume = sfx.Volume;

        _audioSource.pitch = randomPitch;
        _audioSource.PlayOneShot(sfx.Clips[randomIndex]);
    }
}