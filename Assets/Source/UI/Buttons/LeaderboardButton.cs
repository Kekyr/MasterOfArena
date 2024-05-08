using System;
using Agava.YandexGames;
using UnityEngine;

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
#if UNITY_WEBGL && !UNITY_EDITOR
        PlayerAccount.Authorize();

        if (PlayerAccount.IsAuthorized)
            PlayerAccount.RequestPersonalProfileDataPermission();

        if (PlayerAccount.IsAuthorized == false)
            return;
        
        _leaderboard.Fill();
#endif
    }
}