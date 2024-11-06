using System;
using System.Collections.Generic;
using UnityEngine;

namespace Audio
{
    public class Music : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private MusicSO _music;

        private List<IMortal> _mortals;

        private AudioButton _button;
        private AudioSettingSO _musicOptions;

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

            foreach (IMortal mortal in _mortals)
            {
                mortal.Died -= OnDead;
            }
        }

        public void Init(List<IMortal> mortals, AudioButton button, AudioSettingSO musicOptions)
        {
            _mortals = mortals;
            _button = button;
            _musicOptions = musicOptions;

            _button.Switched += OnSwitch;

            foreach (IMortal mortal in _mortals)
            {
                mortal.Died += OnDead;
            }
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
            if (_musicOptions.IsOn == false)
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
}