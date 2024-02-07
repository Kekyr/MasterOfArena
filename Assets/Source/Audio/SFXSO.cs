using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new SFXSO", menuName = "SFXSO/Create new SFXSO")]
public class SFXSO : ScriptableObject
{
    [SerializeField] private List<AudioClip> _clips = new List<AudioClip>();
    [SerializeField] private float _volume;

    public IReadOnlyList<AudioClip> Clips => _clips;
    public float Volume => _volume;
}