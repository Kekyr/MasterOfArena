using System;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using Lean.Localization;
using UnityEngine;
using UnityEngine.UI;

public class Root : MonoBehaviour
{
    [SerializeField] private EnemiesSO _enemies;
    [SerializeField] private ProgressBarSO _progressBarData;
    [SerializeField] private ZonesSO _zones;
    [SerializeField] private PlayerDataSO _playerData;
    [SerializeField] private TutorialSO _tutorialData;
    [SerializeField] private SpawnChancesSO _spawnChancesSO;
    [SerializeField] private SkinsSO _skins;

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
    [SerializeField] private SFX _rewardedAdSFX;

    [SerializeField] private LeanLocalizedTextMeshProUGUI _zoneText;

    [SerializeField] private Tutorial _tutorial;
    [SerializeField] private TutorialHand _tutorialHand;

    [SerializeField] private MusicButton _musicButton;
    [SerializeField] private SFXButton _sfxButton;
    [SerializeField] private Button[] _mainButtons;
    [SerializeField] private ImageSO _musicButtonData;
    [SerializeField] private ImageSO _sfxButtonData;

    [SerializeField] private YandexLeaderboard _leaderboard;
    [SerializeField] private FocusTracker _focusTracker;
    [SerializeField] private ProgressBar _progressBar;

    [SerializeField] private CubeSpawner _cubeSpawner;
    [SerializeField] private Coins _coins;
    [SerializeField] private CoinsView _coinsView;

    [SerializeField] private Shop _shop;
    [SerializeField] private SFX _shopSFX;
    [SerializeField] private ShopPopup _shopPopup;

    private CatchZone _playerCatchZone;
    private CatchZone _enemyCatchZone;

    private Sequence _order;
    private PlayerInputRouter _inputRouter;
    private Character _player;

    private Health _playerHealth;
    private Health _aiHealth;

    private void Validate()
    {
        if (_enemies == null)
        {
            throw new ArgumentNullException(nameof(_enemies));
        }

        if (_progressBarData == null)
        {
            throw new ArgumentNullException(nameof(_progressBarData));
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

        if (_spawnChancesSO == null)
        {
            throw new ArgumentNullException(nameof(_spawnChancesSO));
        }

        if (_skins == null)
        {
            throw new ArgumentNullException(nameof(_skins));
        }

        if (_saveLoader == null)
        {
            throw new ArgumentNullException(nameof(_saveLoader));
        }

        if (_playerSpawnPosition == null)
        {
            throw new ArgumentNullException(nameof(_playerSpawnPosition));
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

        if (_rewardedAdSFX == null)
        {
            throw new ArgumentNullException(nameof(_rewardedAdSFX));
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

        if (_musicButtonData == null)
        {
            throw new ArgumentNullException(nameof(_musicButtonData));
        }

        if (_sfxButtonData == null)
        {
            throw new ArgumentNullException(nameof(_sfxButtonData));
        }

        if (_mainButtons.Length == 0)
        {
            throw new ArgumentOutOfRangeException(nameof(_mainButtons));
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

        if (_progressBar == null)
        {
            throw new ArgumentNullException(nameof(_progressBar));
        }

        if (_coins == null)
        {
            throw new ArgumentNullException(nameof(_coins));
        }

        if (_coinsView == null)
        {
            throw new ArgumentNullException(nameof(_coinsView));
        }

        if (_tutorial == null)
        {
            throw new ArgumentNullException(nameof(_tutorial));
        }

        if (_tutorialHand == null)
        {
            throw new ArgumentNullException(nameof(_tutorialHand));
        }

        if (_shop == null)
        {
            throw new ArgumentNullException(nameof(_shop));
        }

        if (_shopSFX == null)
        {
            throw new ArgumentNullException(nameof(_shopSFX));
        }

        if (_shopPopup == null)
        {
            throw new ArgumentNullException(nameof(_shopPopup));
        }
    }

    private void Awake()
    {
        int weight = 1;
        int radius = 1;

        Validate();

        SkinDataSO[] skinsData = new SkinDataSO[_skins.Skins.Count];

        for (int i = 0; i < _skins.Skins.Count; i++)
        {
            skinsData[i] = _skins.Skins[i].Data;
        }

        _saveLoader.Init(_progressBarData, _playerData, _zones, _spawnChancesSO, skinsData, _tutorialData,
            _audioSettings, _musicButtonData, _sfxButtonData);
        _musicButton.Init(_saveLoader);
        _sfxButton.Init(_saveLoader);

        Zone zone = _zones.Current.Prefab;
        zone = Instantiate(zone, _zoneSpawnPosition.transform.position, _zoneSpawnPosition.transform.rotation,
            _zoneSpawnPosition.transform);

        _zoneText.TranslationName = _zones.Current.TranslationName;

        ArenaSide playerSide = zone.PlayerSide;
        ArenaSide enemySide = zone.EnemySide;

        _playerCatchZone = playerSide.GetComponentInChildren<CatchZone>();
        _enemyCatchZone = enemySide.GetComponentInChildren<CatchZone>();

        _targetGroup.AddMember(enemySide.transform, weight, radius);

        _player = _skins.Skins[_playerData.CurrentSkinIndex].Prefab;
        _player = Instantiate(_player, _playerSpawnPosition.transform.position, _player.transform.rotation,
            _playerSpawnPosition.transform);

        _virtualCamera.Follow = _player.transform;

        Character enemy = _enemies.GetRandom();
        enemy = Instantiate(enemy, _enemySpawnPosition.transform.position, enemy.transform.rotation,
            _enemySpawnPosition.transform);

        _rewardedAd.Init(_saveLoader, _music, _playerData, _coins);
        _rewardedAdSFX.Init(_sfxButton, _audioSettings);

        _coinsView.Init(_coins);
        _coins.Init(_playerData);
        _progressBar.Init(_playerData);

        _inputRouter = _player.GetComponent<PlayerInputRouter>();
        _playerHealth = _player.GetComponent<Health>();
        _aiHealth = enemy.GetComponent<Health>();

        _shopPopup.Init(skinsData, _shop, _rewardedAd);
        _shopPopup.Init(_playerHealth, _aiHealth);
        _shop.Init(_playerData, _coins, _shopPopup, _saveLoader);
        _shopSFX.Init(_sfxButton, _audioSettings);

        _order = DOTween.Sequence();

        _inputRouter.Init(_playerHealth, _aiHealth, _order);

        _musicButton.Init(_audioSettings);
        _sfxButton.Init(_audioSettings);

        _music.Init(_playerHealth, _aiHealth, _musicButton, _audioSettings);
        _focusTracker.Init(_music);

        _cubeSpawner.Init(_playerHealth, _aiHealth);

        _playerRoot.Init(_inputRouter, _leaderboard, _interstitialAd);
        _playerRoot.Init(_rewardedAd, _saveLoader, _coins);
        _playerRoot.Init(_playerData, _spawnChancesSO);

        _aiRoot.Init(_cubeSpawner);

        _playerRoot.Init(_player, enemy, _virtualCamera);
        _aiRoot.Init(enemy, _player, _virtualCamera);

        _playerRoot.Init(_order, _sfxButton, _audioSettings);
        _aiRoot.Init(_order, _sfxButton, _audioSettings);

        _playerRoot.Init(_playerHealth, playerSide, _playerCatchZone);
        _aiRoot.Init(_aiHealth, enemySide, _enemyCatchZone);

        _playerRoot.Init(_aiHealth);
        _aiRoot.Init(_playerHealth);

        _focusTracker.Init(_tutorialHand);
        _tutorialHand.Init(_inputRouter, _mainButtons, _coinsView);
        _tutorial.Init(_tutorialData, _tutorialHand);
    }
}