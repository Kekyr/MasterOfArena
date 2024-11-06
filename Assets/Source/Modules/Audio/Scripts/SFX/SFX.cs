using UnityEngine;
using Random = UnityEngine.Random;

namespace Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class SFX : MonoBehaviour
    {
        private readonly int _minPitch = 1;
        private readonly int _maxPitch = 4;

        private AudioSettingSO _sfxOptions;
        private AudioButton _button;
        private AudioSource _audioSource;

        private float _defaultVolume;
        private float _defaultPitch;

        private void OnEnable()
        {
            _audioSource = GetComponent<AudioSource>();
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

        public void Init(AudioButton button, AudioSettingSO sfxOptions)
        {
            _button = button;
            _sfxOptions = sfxOptions;
            enabled = true;
        }

        public void Play(SFXSO sfx)
        {
            if (_sfxOptions.IsOn == false)
            {
                return;
            }

            AudioClip randomClip = sfx.GetRandomClip();

            if (randomClip == null)
            {
                return;
            }

            int randomPitch = Random.Range(_minPitch, _maxPitch);

            if (sfx.Volume == 0)
            {
                _audioSource.volume = _defaultVolume;
            }
            else
            {
                _audioSource.volume = sfx.Volume;
            }

            if (sfx.CanPitch)
            {
                _audioSource.pitch = randomPitch;
            }
            else
            {
                _audioSource.pitch = _defaultPitch;
            }

            _audioSource.PlayOneShot(randomClip);
        }

        private void OnSwitch()
        {
            _audioSource.Stop();
        }
    }
}