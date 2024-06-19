using System;
using Cinemachine;
using UnityEngine;

public class PlayerRoot : CharacterRoot
{
    [SerializeField] private CinemachineTargetGroup _targetGroup;
    [SerializeField] private SaveLoader _saveLoader;

    private PlayerInputRouter _inputRouter;
    private YandexLeaderboard _leaderboard;

    protected override void OnEnable()
    {
        if (_targetGroup == null)
        {
            throw new ArgumentNullException(nameof(_targetGroup));
        }

        if (_saveLoader == null)
        {
            throw new ArgumentNullException(nameof(_saveLoader));
        }

        base.OnEnable();
    }

    protected override void Start()
    {
        float weight = 1.69f;
        int radius = 1;

        WinPopup winPopup = (WinPopup)Window;
        winPopup.Init(_saveLoader);

        _targetGroup.AddMember(PlatformWithBomb.transform, weight, radius);

        PlayerTargeting targeting = (PlayerTargeting)Aiming;
        targeting.Init(_inputRouter);

        Player player = (Player)Person;
        _leaderboard.Init(player);

        base.Start();
    }

    public void Init(PlayerInputRouter inputRouter, YandexLeaderboard leaderboard)
    {
        _inputRouter = inputRouter;
        _leaderboard = leaderboard;
    }
}