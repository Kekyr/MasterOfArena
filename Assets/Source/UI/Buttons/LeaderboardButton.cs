using System;
using Agava.YandexGames;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardButton : MainButton
{
    [SerializeField] private Button _button;

    private void OnEnable()
    {
        if (_button == null)
            throw new ArgumentNullException(nameof(_button));

        _button.onClick.AddListener(OpenLeaderboard);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OpenLeaderboard);
    }

    private void OpenLeaderboard()
    {
        PlayerAccount.Authorize();

        if (PlayerAccount.IsAuthorized)
            PlayerAccount.RequestPersonalProfileDataPermission();

        if (PlayerAccount.IsAuthorized == false)
            return;
    }
}