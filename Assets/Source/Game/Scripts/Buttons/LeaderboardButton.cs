using System;
using LeaderboardBase;
using UnityEngine;
using YG;

public class LeaderboardButton : MainButton
{
    [SerializeField] private Leaderboard _leaderboard;
    [SerializeField] private AuthorizationPopup _popup;

    protected override void OnEnable()
    {
        base.OnEnable();

        if (_leaderboard == null)
        {
            throw new ArgumentNullException(nameof(_leaderboard));
        }

        if (_popup == null)
        {
            throw new ArgumentNullException(nameof(_popup));
        }
    }

    protected override void OnClick()
    {
        if (YandexGame.auth == false)
        {
            _popup.gameObject.SetActive(true);
            return;
        }

        _leaderboard.Fill();
    }
}