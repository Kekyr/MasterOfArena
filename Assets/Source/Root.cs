using System;
using DG.Tweening;
using UnityEngine;

public class Root : MonoBehaviour
{
    [SerializeField] private PlayerRoot _playerRoot;
    [SerializeField] private AIRoot _aiRoot;

    [SerializeField] private Music _music;
    [SerializeField] private AudioSettingsSO _audioSettings;

    [SerializeField] private MusicButton _musicButton;
    [SerializeField] private SFXButton _sfxButton;

    [SerializeField] private PlayerInputRouter _inputRouter;

    [SerializeField] private CubeSpawner _cubeSpawner;

    [SerializeField] private Health _playerHealth;
    [SerializeField] private Health _aiHealth;

    private Sequence _order;

    protected Sequence Order => _order;

    protected PlayerInputRouter InputRouter => _inputRouter;

    protected virtual void Validate()
    {
        if (_playerRoot == null)
            throw new ArgumentNullException(nameof(_playerRoot));

        if (_aiRoot == null)
            throw new ArgumentNullException(nameof(_aiRoot));

        if (_music == null)
            throw new ArgumentNullException(nameof(_music));

        if (_audioSettings == null)
            throw new ArgumentNullException(nameof(_audioSettings));

        if (_musicButton == null)
            throw new ArgumentNullException(nameof(_musicButton));

        if (_sfxButton == null)
            throw new ArgumentNullException(nameof(_sfxButton));

        if (_inputRouter == null)
            throw new ArgumentNullException(nameof(_inputRouter));

        if (_cubeSpawner == null)
            throw new ArgumentNullException(nameof(_cubeSpawner));

        if (_playerHealth == null)
            throw new ArgumentNullException(nameof(_playerHealth));

        if (_aiHealth == null)
            throw new ArgumentNullException(nameof(_aiHealth));
    }

    protected virtual void Awake()
    {
        Validate();

        _order = DOTween.Sequence();

        _musicButton.Init(_audioSettings);
        _sfxButton.Init(_audioSettings);

        _music.Init(_playerHealth, _aiHealth, _musicButton, _audioSettings);
        _inputRouter.Init(_playerHealth, _aiHealth);

        _cubeSpawner.Init(_playerHealth, _aiHealth);

        _playerRoot.Init(_inputRouter);
        _playerRoot.Init(_order, _sfxButton, _audioSettings);

        _aiRoot.Init(_cubeSpawner);
        _aiRoot.Init(_order, _sfxButton, _audioSettings);
    }
}