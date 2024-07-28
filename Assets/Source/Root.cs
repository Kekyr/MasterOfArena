using System;
using Cinemachine;
using DG.Tweening;
using Lean.Localization;
using UnityEngine;

public class Root : MonoBehaviour
{
    [SerializeField] private CharactersSO _characters;
    [SerializeField] private ZonesSO _zones;
    [SerializeField] private PlayerDataSO _playerData;
    [SerializeField] private TutorialSO _tutorialData;

    [SerializeField] private SaveLoader _saveLoader;

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
    [SerializeField] private RewardedAd _rewardedAd;

    [SerializeField] private LeanLocalizedTextMeshProUGUI _zoneText;

    [SerializeField] private Tutorial _tutorial;
    [SerializeField] private TutorialHand _tutorialHand;
    [SerializeField] private MusicButton _musicButton;
    [SerializeField] private SFXButton _sfxButton;

    [SerializeField] private YandexLeaderboard _leaderboard;
    [SerializeField] private FocusTracker _focusTracker;

    [SerializeField] private CubeSpawner _cubeSpawner;

    private Sequence _order;
    private PlayerInputRouter _inputRouter;

    private Health _playerHealth;
    private Health _aiHealth;

    private void Validate()
    {
        if (_characters == null)
        {
            throw new ArgumentNullException(nameof(_characters));
        }

        if (_zones == null)
        {
            throw new ArgumentNullException(nameof(_zones));
        }

        if (_playerData == null)
        {
            throw new ArgumentNullException(nameof(_playerData));
        }

        if (_tutorialData == null)
        {
            throw new ArgumentNullException(nameof(_tutorialData));
        }

        if (_saveLoader == null)
        {
            throw new ArgumentNullException(nameof(_saveLoader));
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

        if (_rewardedAd == null)
        {
            throw new ArgumentNullException(nameof(_rewardedAd));
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

        if (_focusTracker == null)
        {
            throw new ArgumentNullException(nameof(_focusTracker));
        }

        if (_cubeSpawner == null)
        {
            throw new ArgumentNullException(nameof(_cubeSpawner));
        }

        if (_tutorial == null)
        {
            throw new ArgumentNullException(nameof(_tutorial));
        }

        if (_tutorialHand == null)
        {
            throw new ArgumentNullException(nameof(_tutorialHand));
        }
    }

    private void Awake()
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

        _rewardedAd.Init(_playerData, _saveLoader);

        _inputRouter = player.GetComponent<PlayerInputRouter>();
        _playerHealth = player.GetComponent<Health>();
        _aiHealth = enemy.GetComponent<Health>();

        _order = DOTween.Sequence();

        _musicButton.Init(_audioSettings);
        _sfxButton.Init(_audioSettings);

        _music.Init(_playerHealth, _aiHealth, _musicButton, _audioSettings);
        _focusTracker.Init(_music);

        _cubeSpawner.Init(_playerHealth, _aiHealth);

        _playerRoot.Init(_inputRouter, _leaderboard, _interstitialAd);
        _playerRoot.Init(_rewardedAd, _saveLoader);

        _aiRoot.Init(_cubeSpawner);

        _playerRoot.Init(player, enemy, _virtualCamera);
        _aiRoot.Init(enemy, player, _virtualCamera);

        _playerRoot.Init(_order, _sfxButton, _audioSettings);
        _aiRoot.Init(_order, _sfxButton, _audioSettings);

        _playerRoot.Init(_playerHealth, playerSide);
        _aiRoot.Init(_aiHealth, enemySide);

        _playerRoot.Init(_aiHealth);
        _aiRoot.Init(_playerHealth);

        _focusTracker.Init(_tutorialHand);
        _tutorialHand.Init(_inputRouter);
        _tutorial.Init(_tutorialData, _tutorialHand);

        _inputRouter.Init(_playerHealth, _aiHealth, _order);
    }
}