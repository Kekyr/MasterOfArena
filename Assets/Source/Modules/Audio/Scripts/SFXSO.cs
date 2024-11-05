using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new SFXSO", menuName = "SFXSO/Create new SFXSO")]
public class SFXSO : ScriptableObject
{
    [SerializeField] private List<AudioClip> _clips = new List<AudioClip>();
    [SerializeField] private float _volume;
    [SerializeField] private bool _canPitch;

    public float Volume => _volume;
    public bool CanPitch => _canPitch;

    public AudioClip GetRandomClip()
    {
        int randomIndex = Random.Range(0, _clips.Count - 1);
        return _clips[randomIndex];
    }
}