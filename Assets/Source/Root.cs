using System;
using Cinemachine;
using DG.Tweening;
using Lean.Localization;
using UnityEngine;

public class Root : MonoBehaviour
{
    [SerializeField] private CharactersSO _characters;
    [SerializeField] private ZonesSO _zones;

    [SerializeField] private PlayerSpawnPosition _playerSpawnPosition;
    [SerializeField] private EnemySpawnPosition _enemySpawnPosition;
    [SerializeField] private ZoneSpawnPosition _zoneSpawnPosition;

    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    [SerializeField] private CinemachineTargetGroup _targetGroup;

    [SerializeField] private PlayerRoot _playerRoot;
    [SerializeField] private AIRoot _aiRoot;

    [SerializeField] private Music _music;
    [SerializeField] private AudioSettingsSO _audioSettings;

    [SerializeField] private InterstitialAd _interstitialAd;

    [SerializeField] private LeanLocalizedTextMeshProUGUI _zoneText;
    
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

        if (_zones == null)
        {
            throw new ArgumentNullException(nameof(_zones));
        }

        if (_playerSpawnPosition == null)
        {
            throw new ArgumentNullException(nameof(_characters));
        }

        if (_enemySpawnPosition == null)
        {
            throw new ArgumentNullException(nameof(_enemySpawnPosition));
        }

        if (_zoneSpawnPosition == null)
        {
            throw new ArgumentNullException(nameof(_zoneSpawnPosition));
        }

        if (_virtualCamera == null)
        {
            throw new ArgumentNullException(nameof(_virtualCamera));
        }

        if (_targetGroup == null)
        {
            throw new ArgumentNullException(nameof(_targetGroup));
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

        if (_interstitialAd == null)
        {
            throw new ArgumentNullException(nameof(_interstitialAd));
        }

        if (_zoneText == null)
        {
            throw new ArgumentNullException(nameof(_zoneText));
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
        int weight = 1;
        int radius = 1;

        Validate();

        Zone zone = _zones.Current.Prefab;
        zone = Instantiate(zone, _zoneSpawnPosition.transform.position, _zoneSpawnPosition.transform.rotation,
            _zoneSpawnPosition.transform);

        _zoneText.TranslationName = _zones.Current.TranslationName;

        ArenaSide playerSide = zone.PlayerSide;
        ArenaSide enemySide = zone.EnemySide;

        _targetGroup.AddMember(enemySide.transform, weight, radius);

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

        _music.Init(_interstitialAd);
        _music.Init(_playerHealth, _aiHealth, _musicButton, _audioSettings);
        _inputRouter.Init(_playerHealth, _aiHealth);

        _cubeSpawner.Init(_playerHealth, _aiHealth);

        _playerRoot.Init(playerSide);
        _playerRoot.Init(_inputRouter, _leaderboard, _interstitialAd);
        _playerRoot.Init(player, enemy, _virtualCamera);
        _playerRoot.Init(_order, _sfxButton, _audioSettings);

        _aiRoot.Init(enemySide);
        _aiRoot.Init(_cubeSpawner);
        _aiRoot.Init(enemy, player, _virtualCamera);
        _aiRoot.Init(_order, _sfxButton, _audioSettings);
    }
}