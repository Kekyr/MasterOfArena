using System;
using UnityEngine;
using YG;

public class LeaderboardButton : MainButton
{
    [SerializeField] private YandexLeaderboard _leaderboard;

    protected override void OnEnable()
    {
        base.OnEnable();

        if (_leaderboard == null)
        {
            throw new ArgumentNullException(nameof(_leaderboard));
        }
    }

    protected override void OnClick()
    {
        if (YandexGame.auth == false)
        {
            YandexGame.AuthDialog();
            return;
        }

        _leaderboard.Fill();
    }
}