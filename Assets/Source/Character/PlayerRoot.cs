using System;
using Cinemachine;
using UnityEngine;

public class PlayerRoot : CharacterRoot
{
    [SerializeField] private CinemachineTargetGroup _targetGroup;

    private SaveLoader _saveLoader;
    private PlayerInputRouter _inputRouter;
    private YandexLeaderboard _leaderboard;

    private InterstitialAd _interstitialAd;
    private RewardedAd _rewardedAd;

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
        winPopup.Init(_saveLoader, _interstitialAd, _rewardedAd);

        _targetGroup.AddMember(PlatformWithBomb.transform, weight, radius);

        PlayerTargeting targeting = (PlayerTargeting)Aiming;
        targeting.Init(_inputRouter);

        Player player = (Player)Person;
        player.Init(_saveLoader);

        _leaderboard.Init(player, _rewardedAd);

        base.Start();
    }

    public void Init(PlayerInputRouter inputRouter, YandexLeaderboard leaderboard, InterstitialAd interstitialAd)
    {
        _inputRouter = inputRouter;
        _leaderboard = leaderboard;
        _interstitialAd = interstitialAd;
    }

    public void Init(RewardedAd rewardedAd, SaveLoader saveLoader)
    {
        _rewardedAd = rewardedAd;
        _saveLoader = saveLoader;
    }
}