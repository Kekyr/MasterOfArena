using System;
using Cinemachine;
using DG.Tweening;
using UnityEngine;

public class Root : MonoBehaviour
{
    [SerializeField] private CharactersSO _characters;
    [SerializeField] private PlayerSpawnPosition _playerSpawnPosition;
    [SerializeField] private EnemySpawnPosition _enemySpawnPosition;
    
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;

    [SerializeField] private PlayerRoot _playerRoot;
    [SerializeField] private AIRoot _aiRoot;

    [SerializeField] private Music _music;
    [SerializeField] private AudioSettingsSO _audioSettings;

    [SerializeField] private MusicButton _musicButton;
    [SerializeField] private SFXButton _sfxButton;

    [SerializeField] private YandexLeaderboard _leaderboard;

    [SerializeField] private CubeSpawner _cubeSpawner;

    private Sequence _order;
    private PlayerInputRouter _inputRouter;

    private Health _playerHealth;
    private Health _aiHealth;

    protected Sequence Order => _order;

    protected PlayerInputRouter InputRouter => _inputRouter;

    protected virtual void Validate()
    {
        if (_characters == null)
        {
            throw new ArgumentNullException(nameof(_characters));
        }

        if (_playerSpawnPosition == null)
        {
            throw new ArgumentNullException(nameof(_characters));
        }

        if (_enemySpawnPosition == null)
        {
            throw new ArgumentNullException(nameof(_enemySpawnPosition));
        }

        if (_virtualCamera == null)
        {
            throw new ArgumentNullException(nameof(_virtualCamera));
        }

        if (_playerRoot == null)
        {
            throw new ArgumentNullException(nameof(_playerRoot));
        }

        if (_aiRoot == null)
        {
            throw new ArgumentNullException(nameof(_aiRoot));
        }

        if (_music == null)
        {
            throw new ArgumentNullException(nameof(_music));
        }

        if (_audioSettings == null)
        {
            throw new ArgumentNullException(nameof(_audioSettings));
        }

        if (_musicButton == null)
        {
            throw new ArgumentNullException(nameof(_musicButton));
        }

        if (_sfxButton == null)
        {
            throw new ArgumentNullException(nameof(_sfxButton));
        }

        if (_leaderboard == null)
        {
            throw new ArgumentNullException(nameof(_leaderboard));
        }

        if (_cubeSpawner == null)
        {
            throw new ArgumentNullException(nameof(_cubeSpawner));
        }
    }

    protected virtual void Awake()
    {
        Validate();

        Character player = _characters.GetRandomPlayerPrefab();
        player = Instantiate(player, _playerSpawnPosition.transform.position, player.transform.rotation,
            _playerSpawnPosition.transform);

        _virtualCamera.Follow = player.transform;

        Character enemy = _characters.GetRandomEnemyPrefab();
        enemy = Instantiate(enemy, _enemySpawnPosition.transform.position, enemy.transform.rotation,
            _enemySpawnPosition.transform);

        _inputRouter = player.GetComponent<PlayerInputRouter>();
        _playerHealth = player.GetComponent<Health>();
        _aiHealth = enemy.GetComponent<Health>();

        _order = DOTween.Sequence();

        _musicButton.Init(_audioSettings);
        _sfxButton.Init(_audioSettings);

        _music.Init(_playerHealth, _aiHealth, _musicButton, _audioSettings);
        _inputRouter.Init(_playerHealth, _aiHealth);

        _cubeSpawner.Init(_playerHealth, _aiHealth);

        _playerRoot.Init(_inputRouter, _leaderboard);
        _playerRoot.Init(player, enemy, _virtualCamera);
        _playerRoot.Init(_order, _sfxButton, _audioSettings);

        _aiRoot.Init(_cubeSpawner);
        _aiRoot.Init(enemy, player, _virtualCamera);
        _aiRoot.Init(_order, _sfxButton, _audioSettings);
    }
}