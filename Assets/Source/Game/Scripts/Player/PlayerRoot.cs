using System;
using Aiming;
using Cinemachine;
using LeaderboardBase;
using UnityEngine;

public class PlayerRoot : CharacterRoot
{
    [SerializeField] private CinemachineTargetGroup _targetGroup;

    private SaveLoader _saveLoader;
    private PlayerInputRouter _inputRouter;
    private Leaderboard _leaderboard;

    private InterstitialAd _interstitialAd;
    private RewardedAd _rewardedAd;
    private Coins _coins;

    private PlayerDataSO _data;
    private SpawnChancesSO _spawnChancesSO;

    protected override void OnEnable()
    {
        if (_targetGroup == null)
        {
            throw new ArgumentNullException(nameof(_targetGroup));
        }

        base.OnEnable();
    }

    protected override void Start()
    {
        float weight = 1.69f;
        int radius = 1;

        WinPopup winPopup = (WinPopup)Window;
        winPopup.Init(_interstitialAd, _rewardedAd);

        _targetGroup.AddMember(PlatformWithBomb.transform, weight, radius);

        PlayerTargeting targeting = (PlayerTargeting)Aiming;
        targeting.Init(_inputRouter, Person);

        Player player = (Player)Person;
        player.Init(_data, _spawnChancesSO, _saveLoader, _coins);

        _leaderboard.Init(player, _rewardedAd, _data);

        base.Start();
    }

    public void Init(PlayerInputRouter inputRouter, Leaderboard leaderboard, InterstitialAd interstitialAd)
    {
        _inputRouter = inputRouter;
        _leaderboard = leaderboard;
        _interstitialAd = interstitialAd;
    }

    public void Init(RewardedAd rewardedAd, SaveLoader saveLoader, Coins coins)
    {
        _rewardedAd = rewardedAd;
        _saveLoader = saveLoader;
        _coins = coins;
    }

    public void Init(PlayerDataSO data, SpawnChancesSO spawnChancesSO)
    {
        _data = data;
        _spawnChancesSO = spawnChancesSO;
    }
}