using System;
using System.Collections.Generic;
using Ad;
using Arena;
using Audio;
using CharacterBase;
using Cinemachine;
using DG.Tweening;
using HealthSystem;
using LeaderboardBase;
using Lean.Localization;
using Money;
using PlayerBase;
using ProgressBarFeature;
using SaveSystem;
using ShopSystem;
using TutorialFeature;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
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

        [SerializeField] private Transform _playerSpawnPosition;
        [SerializeField] private Transform _enemySpawnPosition;
        [SerializeField] private Transform _zoneSpawnPosition;

        [SerializeField] private CinemachineVirtualCamera _virtualCamera;
        [SerializeField] private CinemachineTargetGroup _targetGroup;

        [SerializeField] private PlayerRoot _playerRoot;
        [SerializeField] private AIRoot _aiRoot;

        [SerializeField] private Music _music;
        [SerializeField] private AudioSettingSO _musicOptions;
        [SerializeField] private AudioSettingSO _sfxOptions;

        [SerializeField] private InterstitialAd _interstitialAd;
        [SerializeField] private RewardedAd _rewardedAd;
        [SerializeField] private SFX _rewardedAdSFX;

        [SerializeField] private LeanLocalizedTextMeshProUGUI _zoneText;

        [SerializeField] private Tutorial _tutorial;
        [SerializeField] private TutorialHand _tutorialHand;

        [SerializeField] private AudioButton _musicButton;
        [SerializeField] private AudioButton _sfxButton;
        [SerializeField] private Button[] _mainButtons;
        [SerializeField] private ImageSO _musicButtonData;
        [SerializeField] private ImageSO _sfxButtonData;

        [SerializeField] private Leaderboard _leaderboard;
        [SerializeField] private FocusTracker _focusTracker;
        [SerializeField] private ProgressBar _progressBar;

        [SerializeField] private CubeSpawner _cubeSpawner;
        [SerializeField] private Coins _coins;
        [SerializeField] private CoinsView _coinsView;

        [SerializeField] private Shop _shop;
        [SerializeField] private SFX _shopSFX;
        [SerializeField] private ShopPopup _shopPopup;

        [SerializeField] private AuthorizationPopup _authorizationPopup;

        private Collider _playerCatchZone;
        private Collider _enemyCatchZone;

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

            if (_musicOptions == null)
            {
                throw new ArgumentNullException(nameof(_musicOptions));
            }

            if (_sfxOptions == null)
            {
                throw new ArgumentNullException(nameof(_sfxOptions));
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

            if (_authorizationPopup == null)
            {
                throw new ArgumentNullException(nameof(_authorizationPopup));
            }
        }

        private void Awake()
        {
            int weight = 1;
            int radius = 1;

            Validate();

            _progressBarData.Init(_zones);

            SkinDataSO[] skinsData = new SkinDataSO[_skins.Skins.Count];

            for (int i = 0; i < _skins.Skins.Count; i++)
            {
                skinsData[i] = _skins.Skins[i].Data;
            }

            _saveLoader.Init(_progressBarData, _playerData, _zones, _spawnChancesSO);
            _saveLoader.Init(skinsData, _tutorialData, _musicOptions, _sfxOptions, _musicButtonData, _sfxButtonData);

            _musicButton.Init(_saveLoader, _musicOptions);
            _sfxButton.Init(_saveLoader, _sfxOptions);
            _authorizationPopup.Init(_leaderboard);

            Zone zone = _zones.Current.Prefab;

            zone = Instantiate(
                zone,
                _zoneSpawnPosition.transform.position,
                _zoneSpawnPosition.transform.rotation,
                _zoneSpawnPosition.transform);

            _zoneText.TranslationName = _zones.Current.TranslationName;

            ArenaSide playerSide = zone.PlayerSide;
            ArenaSide enemySide = zone.EnemySide;

            _playerCatchZone = playerSide.CatchZone;
            _enemyCatchZone = enemySide.CatchZone;

            _targetGroup.AddMember(enemySide.transform, weight, radius);

            _player = _skins.Skins[_playerData.CurrentSkinIndex].Prefab;
            _player = Instantiate(
                _player,
                _playerSpawnPosition.position,
                _player.transform.rotation,
                _playerSpawnPosition);

            _virtualCamera.Follow = _player.transform;

            Character enemy = _enemies.GetRandom();
            enemy = Instantiate(
                enemy,
                _enemySpawnPosition.position,
                enemy.transform.rotation,
                _enemySpawnPosition);

            _rewardedAd.Init(_saveLoader, _music, _playerData, _coins);
            _rewardedAdSFX.Init(_sfxButton, _sfxOptions);

            _coinsView.Init(_coins);
            _coins.Init(_playerData);
            _progressBar.Init(_playerData, _zones);

            _inputRouter = _player.GetComponent<PlayerInputRouter>();
            _playerHealth = _player.GetComponent<Health>();
            _aiHealth = enemy.GetComponent<Health>();

            _shopPopup.Init(skinsData, _shop, _rewardedAd);
            _shopPopup.Init(_playerHealth, _aiHealth);
            _shop.Init(_playerData, _coins, _shopPopup, _saveLoader);
            _shopSFX.Init(_sfxButton, _sfxOptions);

            _order = DOTween.Sequence();

            _inputRouter.Init(_playerHealth, _aiHealth, _order);

            List<Audio.IMortal> mortals = new List<Audio.IMortal>();
            mortals.Add(_playerHealth);
            mortals.Add(_aiHealth);

            _music.Init(mortals, _musicButton, _musicOptions);
            _focusTracker.Init(_music);

            _cubeSpawner.Init(_playerHealth, _aiHealth);

            _playerRoot.Init(_inputRouter, _leaderboard, _interstitialAd);
            _playerRoot.Init(_rewardedAd, _saveLoader, _coins);
            _playerRoot.Init(_playerData, _spawnChancesSO);

            _aiRoot.Init(_cubeSpawner);

            _playerRoot.Init(_player, enemy, _virtualCamera);
            _aiRoot.Init(enemy, _player, _virtualCamera);

            _playerRoot.Init(_order, _sfxButton, _sfxOptions);
            _aiRoot.Init(_order, _sfxButton, _sfxOptions);

            _playerRoot.Init(_playerHealth, playerSide, _playerCatchZone);
            _aiRoot.Init(_aiHealth, enemySide, _enemyCatchZone);

            _playerRoot.Init(_aiHealth);
            _aiRoot.Init(_playerHealth);

            _focusTracker.Init(_tutorialHand);

            CanvasGroup coinsViewCanvasGroup = _coinsView.GetComponent<CanvasGroup>();
            _tutorialHand.Init(_inputRouter, _mainButtons, coinsViewCanvasGroup);
            _tutorial.Init(_tutorialData, _tutorialHand);
        }
    }
}