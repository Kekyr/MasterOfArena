using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new SFXSO", menuName = "SFXSO/Create new SFXSO")]
public class SFXSO : ScriptableObject
{
    [SerializeField] private List<AudioClip> _clips = new List<AudioClip>();
    [SerializeField] private float _volume;
    [SerializeField] private bool _canPitch;

    public IReadOnlyList<AudioClip> Clips => _clips;
    public float Volume => _volume;
    public bool CanPitch => _canPitch;
}